using AASC.Partner.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AASC.FW.Repositories;
using AASC.FW.UnitOfWork;
using AASC.Partner.API.Helpers;
using Newtonsoft.Json;
using System.Data.Entity.Validation;
using AASC.Partner.API.ErrorHelpers;

namespace AASC.Partner.API.Services
{
    public class EmployeeService
    {
        protected IRepository<Employee> _repository;

        protected IRepository<Company> _companyRepository;

        protected IUnitOfWork _unitOfWork;

        protected ModelFactory _factory;

        public EmployeeService(ModelFactory factory,
            IRepository<Employee> repository,
            IRepository<Company> companyRepository,
            IUnitOfWork unitOfWork)
        {
            _factory = factory;
            _repository = repository;
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        private IQueryable<EmployeeViewModel> GetQueryable(string order)
        {
            var projection = _unitOfWork.Repository<Employee>().Query()
                    .Include(x => x.Company)
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.CreatedBy).Select().AsQueryable().ToList();
            var data = new List<EmployeeViewModel>();
            projection.ForEach(x => { data.Add(_factory.Create(x)); });            
            return data.AsQueryable();
        }

        private IQueryable<EmployeeViewModel> GetQueryable(string order, string companyId)
        {
            var queryable = GetQueryable(order);

            Guid companyGuid = new Guid(companyId);

            return queryable.Where(x => x.CompanyId == companyGuid.ToString());           
        }

        public Result<EmployeeViewModel> Get(string companyId, int pageSize, int page, int skip, int take, string sorting, string filter)
        {
            List<EmployeeViewModel> results = new List<EmployeeViewModel>();
            string order = "Id";
            Filtering filtering = null;

            int total = 0;

            // order by
            if (sorting != null && sorting != "undefined")
            {
                List<Sort> sort = JsonConvert.DeserializeObject<List<Sort>>(sorting);
                List<string> sorts = new List<string>();
                sort.ForEach(x => sorts.Add(string.Format("{0} {1}", x.field, x.dir)));
                order = string.Join(", ", sorts.ToArray());
            }

            var data = GetQueryable(order, companyId);

            // filtered by
            if (!string.IsNullOrEmpty(filter) && filter != "null")
            {
                filtering = JsonConvert.DeserializeObject<Filtering>(filter);

                var predicate = new DynamicLinqHelper<EmployeeViewModel>().CreateFilterPredicate(filtering.filters, true);

                total = data.Where(predicate).Count();
                foreach (var d in data.Where(predicate).Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<EmployeeViewModel> { Data = results, Total = total };
            }
            else
            {

                total = data.Count();
                foreach (var d in data.Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<EmployeeViewModel> { Data = results, Total = total };
            }
        }

        public Result<EmployeeViewModel> Get(string companyId)
        {
            List<EmployeeViewModel> results = new List<EmployeeViewModel>();

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order, companyId);

            Guid companyGuid = new Guid(companyId);

            foreach (var d in data)
            {
                results.Add(d);
            }

            total = results.Count();
            return new Result<EmployeeViewModel> { Data = results, Total = total };
        }

        public Result<EmployeeViewModel> GetEmployee(string id)
        {
            List<EmployeeViewModel> results = new List<EmployeeViewModel>();

            Guid employeeGuid = new Guid(id);

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order).Where(x => x.Id == employeeGuid.ToString()).ToList();

            foreach (var d in data)
            {
                results.Add(d);
            }

            total = results.Count();
            return new Result<EmployeeViewModel> { Data = results, Total = total };
        }
        
        public Result<EmployeeViewModel> GetDisplayList(string companyId)
        {
            List<EmployeeViewModel> results = new List<EmployeeViewModel>();

            int total = 0;

            results = GetEmployeeDisplayList(companyId);            

            total = results.Count();

            return new Result<EmployeeViewModel> { Data = results, Total = total };
        }

        private List<EmployeeViewModel> GetEmployeeDisplayList(string companyId)
        {
            List<EmployeeViewModel> results = new List<EmployeeViewModel>();

            string order = "Id";

            var data = GetQueryable(order, companyId);

            results.Add(new EmployeeViewModel
            {
                Id = null,
                JobTitle = "",
                ApplicationUserId = null,
                CompanyId = (Guid.Parse(companyId)).ToString(),
                ApplicationUser = new DisplayUserBindingModel {
                    Id = "",
                    Email = "",
                    FirstName = "",
                    LastName = "",
                    UserName = ""
                },
                CreatedDate = null,
                CreatedById = null,
                CreatedBy = new DisplayUserBindingModel
                {
                    Id = "",
                    Email = "",
                    FirstName = "",
                    LastName = "",
                    UserName = ""
                }
            });

            foreach (var d in data)
            {
                results.Add(new EmployeeViewModel
                {
                    Id = d.Id,
                    JobTitle = d.JobTitle,
                    ApplicationUserId = d.ApplicationUserId,
                    CompanyId = d.CompanyId,
                    ApplicationUser = new DisplayUserBindingModel
                    {
                        Id = d.ApplicationUser.Id,
                        UserName = d.ApplicationUser.UserName,
                        Email = d.ApplicationUser.Email,
                        FirstName = d.ApplicationUser.FirstName,
                        LastName = d.ApplicationUser.LastName
                    },
                    CreatedDate = d.CreatedDate,
                    CreatedById = d.CreatedById,
                    CreatedBy = d.CreatedBy
                });
            }
            return results;
        }

        public OperationResult<EmployeeViewModel> Create(EmployeeViewModel model, string createdById)
        {
            var company = _companyRepository.Find(model.CompanyId);

            if (company == null)
                return new OperationResult<EmployeeViewModel> {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = string.Format("Company {0} not exists.", model.CompanyId)
                };            
            
            var employee = new Employee()
            {
                Id = Guid.NewGuid().ToString(),
                JobTitle = model.JobTitle,
                ApplicationUserId = model.ApplicationUserId,
                CompanyId = model.CompanyId,
                CreatedById = createdById,
                CreatedDate = DateTime.UtcNow,
            };

            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.Repository<Employee>().Insert(employee);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();

                employee = _unitOfWork.Repository<Employee>().Query(x => x.Id == employee.Id)
                    .Include(x => x.Company)
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.CreatedBy).Select().FirstOrDefault();
                
                return new OperationResult<EmployeeViewModel>
                {
                    Data = _factory.Create(employee),
                    Status = OperationResult.Success,
                    Message = "Employee saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = "";
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var err in error.ValidationErrors)
                    {
                        errorMessage += string.Format("{0} - {1}", err.PropertyName, err.ErrorMessage);
                    }
                }
                return new OperationResult<EmployeeViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<EmployeeViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public OperationResult<EmployeeViewModel> Update(EmployeeViewModel model)
        {
            var company = _companyRepository.Find(model.CompanyId);

            var employee = _repository.Find(model.Id);

            if (employee == null)
            {
                return new OperationResult<EmployeeViewModel>
                {
                    Data = default(EmployeeViewModel),
                    Status = OperationResult.NotFound,
                    Message = "Not Found"
                };
            }

            if (company == null)
                return new OperationResult<EmployeeViewModel>
                {
                    Data = default(EmployeeViewModel),
                    Status = OperationResult.Failed,
                    Message = string.Format("Company {0} not exists.", model.CompanyId)
                };

            employee.CompanyId = company.Id;
            employee.JobTitle = model.JobTitle;
            employee.ApplicationUserId = model.ApplicationUserId;
                   
            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
                return new OperationResult<EmployeeViewModel>
                {
                    Data = _factory.Create(employee),
                    Status = OperationResult.Success,
                    Message = "Employee saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = "";
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var err in error.ValidationErrors)
                    {
                        errorMessage += string.Format("{0} - {1}", err.PropertyName, err.ErrorMessage);
                    }
                }
                return new OperationResult<EmployeeViewModel>
                {
                    Data = default(EmployeeViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<EmployeeViewModel>
                {
                    Data = default(EmployeeViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public OperationResult<EmployeeViewModel> Delete(string employeeId)
        {
            var employee = _repository.Find(employeeId);

            if (employee == null)
            {
                return new OperationResult<EmployeeViewModel>
                {
                    Data = default(EmployeeViewModel),
                    Status = OperationResult.NotFound,
                    Message = "Not Found"
                };
            }

            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.Repository<Employee>().Delete(employee);
                _unitOfWork.Commit();
                return new OperationResult<EmployeeViewModel>
                {
                    Data = default(EmployeeViewModel),
                    Status = OperationResult.Success,
                    Message = "Employee deleted."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = "";
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var err in error.ValidationErrors)
                    {
                        errorMessage += string.Format("{0} - {1}", err.PropertyName, err.ErrorMessage);
                    }
                }
                return new OperationResult<EmployeeViewModel>
                {
                    Data = default(EmployeeViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<EmployeeViewModel>
                {
                    Data = default(EmployeeViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }
    }
}