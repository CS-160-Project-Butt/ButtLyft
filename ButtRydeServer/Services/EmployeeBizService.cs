using AASC.FW.DataMapper;
using AASC.FW.Repositories;
using AASC.FW.UnitOfWork;
using AASC.Partner.API.ErrorHelpers;
using AASC.Partner.API.Helpers;
using AASC.Partner.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Services
{
    public class EmployeeBizService : IEmployeeBizService
    {
        protected readonly IEmployeeDataService _employeeService;

        protected readonly ICompanyDataService _companyService;

        protected readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public EmployeeBizService(
            IUnitOfWorkAsync unitOfWorkAsync,
            IEmployeeDataService employeeService,
            ICompanyDataService companyService)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _employeeService = employeeService;
            _companyService = companyService;
        }

        private IQueryable<EmployeeListViewModel> GetQueryable(string order)
        {
            var projection = _employeeService
                .Query()
                .Include(x => x.Company)
                .Include(x => x.ApplicationUser)
                .Include(x => x.CreatedBy).Select().AsQueryable().ToList();

            var data = new List<EmployeeListViewModel>();

            projection.ForEach(x => {
                var d = ConvertFrom(x);
                data.Add(d);
            });

            return data.AsQueryable();
        }

        public Result<EmployeeListViewModel> Get(int pageSize, int page, int skip, int take, string sorting, string filter)
        {
            List<EmployeeListViewModel> results = new List<EmployeeListViewModel>();
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

            var data = GetQueryable(order);

            // filtered by
            if (!string.IsNullOrEmpty(filter) && filter != "null")
            {
                filtering = JsonConvert.DeserializeObject<Filtering>(filter);

                var predicate = new DynamicLinqHelper<EmployeeListViewModel>().CreateFilterPredicate(filtering.filters, true);

                total = data.Where(predicate).Count();
                foreach (var d in data.Where(predicate).Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<EmployeeListViewModel> { Data = results, Total = total };
            }
            else
            {

                total = data.Count();
                foreach (var d in data.Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<EmployeeListViewModel> { Data = results, Total = total };
            }
        }        

        public Result<EmployeeListViewModel> GetEmployee(string id)
        {
            List<EmployeeListViewModel> results = new List<EmployeeListViewModel>();

            Guid employeeGuid = new Guid(id);

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order).Where(x => string.Compare(x.Id, id, true) == 0);

            foreach (var d in data)
            {
                results.Add(d);
            }

            total = results.Count();
            return new Result<EmployeeListViewModel> { Data = results, Total = total };
        }

        public Result<EmployeeDisplayViewModel> GetDisplayList(string companyId)
        {
            List<EmployeeDisplayViewModel> results = new List<EmployeeDisplayViewModel>();

            int total = 0;

            results = GetEmployeeDisplayList(companyId);

            total = results.Count();

            return new Result<EmployeeDisplayViewModel> { Data = results, Total = total };
        }

        private List<EmployeeDisplayViewModel> GetEmployeeDisplayList(string companyId)
        {
            List<EmployeeDisplayViewModel> results = new List<EmployeeDisplayViewModel>();

            string order = "Id";

            var data = GetQueryable(order)
                .Where(x => string.Compare(x.CompanyId, companyId, true) == 0 &&
                x.ActiveFrom <= DateTime.UtcNow && x.DeactiveFrom >= DateTime.UtcNow).ToList();

            data.ForEach(x => {
                results.Add(new EmployeeDisplayViewModel
                {
                    Id = x.Id,
                    ApplicationUserId = x.ApplicationUserId,
                    JobTitle = x.JobTitle,
                    Email = x.ApplicationUserEmail,
                    FirstName = x.ApplicationUserFirstName,
                    LastName = x.ApplicationUserLastName,
                    UserName = x.ApplicationUserName
                });
            });

            return results;
        }

        public Result<EmployeeDisplayViewModel> GetDisplayListExceptSelf(string companyId, string id)
        {
            List<EmployeeDisplayViewModel> results = new List<EmployeeDisplayViewModel>();

            int total = 0;

            var data = GetEmployeeDisplayList(companyId).Where(x => string.Compare(x.Id, id, true) != 0).ToList();

            total = results.Count();

            return new Result<EmployeeDisplayViewModel> { Data = results, Total = total };
        }

        public OperationResult<EmployeeListViewModel> Create(EmployeeListViewModel model, string createdById)
        {
            var company = _companyService.Find(model.CompanyId);

            if (company == null)
                return new OperationResult<EmployeeListViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = string.Format("Company Id {0} not exists.", model.CompanyId)
                };

            var employee = new Employee()
            {
                Id = Guid.NewGuid().ToString(),
                JobTitle = model.JobTitle,
                ApplicationUserId = model.ApplicationUserId,
                CompanyId = model.CompanyId,
                CreatedById = createdById,
                CreatedDate = DateTime.UtcNow,
                ActiveFrom = model.ActiveFrom,
                DeactiveFrom = model.DeactiveFrom
            };

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _employeeService.Insert(employee);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();

                var data = GetEmployee(employee.Id).Data.FirstOrDefault();  

                return new OperationResult<EmployeeListViewModel>
                {
                    Data = data,
                    Status = OperationResult.Success,
                    Message = "Employee saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<EmployeeListViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<EmployeeListViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public OperationResult<EmployeeListViewModel> Update(EmployeeListViewModel model)
        {
            var company = _companyService.Find(model.CompanyId);

            var employee = _employeeService.Find(model.Id);

            if (employee == null)
            {
                return new OperationResult<EmployeeListViewModel>
                {
                    Data = default(EmployeeListViewModel),
                    Status = OperationResult.NotFound,
                    Message = "Not Found"
                };
            }

            if (company == null)
                return new OperationResult<EmployeeListViewModel>
                {
                    Data = default(EmployeeListViewModel),
                    Status = OperationResult.Failed,
                    Message = string.Format("Company Id {0} not exists.", model.CompanyId)
                };

            employee.CompanyId = company.Id;
            employee.JobTitle = model.JobTitle;
            employee.ApplicationUserId = model.ApplicationUserId;
            employee.ActiveFrom = model.ActiveFrom.Date; // to Utc
            employee.DeactiveFrom = model.DeactiveFrom.Date; // to Utc

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _employeeService.Update(employee);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();

                var data = GetEmployee(model.Id).Data.FirstOrDefault();

                return new OperationResult<EmployeeListViewModel>
                {
                    Data = data,
                    Status = OperationResult.Success,
                    Message = "Employee saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<EmployeeListViewModel>
                {
                    Data = default(EmployeeListViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<EmployeeListViewModel>
                {
                    Data = default(EmployeeListViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public OperationResult<EmployeeListViewModel> Delete(string employeeId)
        {
            var employee = _employeeService.Find(employeeId);

            if (employee == null)
            {
                return new OperationResult<EmployeeListViewModel>
                {
                    Data = default(EmployeeListViewModel),
                    Status = OperationResult.NotFound,
                    Message = "Not Found"
                };
            }

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _employeeService.Delete(employee);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return new OperationResult<EmployeeListViewModel>
                {
                    Data = default(EmployeeListViewModel),
                    Status = OperationResult.Success,
                    Message = "Employee deleted."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<EmployeeListViewModel>
                {
                    Data = default(EmployeeListViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<EmployeeListViewModel>
                {
                    Data = default(EmployeeListViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        private EmployeeListViewModel ConvertFrom(Employee employee)
        {
            var data = DataMapper.Map<Employee, EmployeeListViewModel>(employee);

            data.ApplicationUserEmail = employee.ApplicationUser.Email;

            data.ApplicationUserFirstName = employee.ApplicationUser.FirstName;

            data.ApplicationUserLastName = employee.ApplicationUser.LastName;

            data.ApplicationUserName = employee.ApplicationUser.UserName;

            data.CompanyName = employee.Company.Name;

            data.CreatedByUserName = employee.CreatedBy.UserName;
            
            return data;
        }
    }

    public interface IEmployeeBizService
    {
        Result<EmployeeListViewModel> Get(int pageSize, int page, int skip, int take, string sorting, string filter);

        Result<EmployeeListViewModel> GetEmployee(string id);

        Result<EmployeeDisplayViewModel> GetDisplayList(string companyId);

        Result<EmployeeDisplayViewModel> GetDisplayListExceptSelf(string companyId, string id);

        OperationResult<EmployeeListViewModel> Create(EmployeeListViewModel model, string createdById);

        OperationResult<EmployeeListViewModel> Update(EmployeeListViewModel model);

        OperationResult<EmployeeListViewModel> Delete(string employeeId);



    }
}