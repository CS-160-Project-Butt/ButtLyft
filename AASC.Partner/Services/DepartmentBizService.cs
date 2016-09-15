using AASC.FW.DataMapper;
using AASC.FW.UnitOfWork;
using AASC.Partner.API.ErrorHelpers;
using AASC.Partner.API.Helpers;
using AASC.Partner.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace AASC.Partner.API.Services
{
    public class DepartmentBizService : IDepartmentBizService
    {
        protected readonly IDepartmentDataService _departmentService;

        protected readonly ICompanyDataService _companyService;

        protected readonly IEmployeeDataService _employeeService;

        protected readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public DepartmentBizService(
            IUnitOfWorkAsync unitOfWorkAsync,
            IDepartmentDataService departmentService,
            IEmployeeDataService employeeService,
            ICompanyDataService companyService
            )
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _departmentService = departmentService;
            _employeeService = employeeService;
            _companyService = companyService;
        }

        private IQueryable<DepartmentListViewModel> GetQueryable(string order)
        {
            var queryDepartment = _departmentService.Query()
                .Include(x => x.Company)
                    .Include(x => x.ParentDepartment)
                    .Include(x => x.DepartmentHead)
                    .Include(x => x.ChildrenDepartments)
                    .Include(x => x.CreatedBy).Select().AsQueryable().ToList();

            var queryEmployee = _employeeService.Query()
                .Include(x => x.ApplicationUser).Select().AsQueryable().ToList();

            try
            {
                var projection = (from d in queryDepartment
                                  join e in queryEmployee on d.DepartmentHeadEmployeeId equals e.Id into grp
                                  from g in grp.DefaultIfEmpty()
                                  select new DepartmentListViewModel
                                  {
                                      Id = d.Id,
                                      Name = d.Name,
                                      CompanyId = d.CompanyId,
                                      CompanyName = d.Company.Name,
                                      CreatedById = d.CreatedById,
                                      CreatedByUserName = d.CreatedBy == null ? "" : d.CreatedBy.UserName,
                                      ParentDepartmentId = d.ParentDepartmentId,
                                      ParentDepartmentName = d.ParentDepartment == null ? "" : d.ParentDepartment.Name,
                                      DepartmentHeadEmployeeId = d.DepartmentHeadEmployeeId,
                                      DepartmentHeadUserName = g == null ? "" : g.ApplicationUser.UserName,
                                      CreatedDate = d.CreatedDate,
                                  }).ToList();

                return projection.AsQueryable();                
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public OperationResult<DepartmentListViewModel> Create(DepartmentListViewModel model, string createdById)
        {
            var company = _companyService.Find(model.CompanyId);

            if (company == null)
                return new OperationResult<DepartmentListViewModel>
                {
                    Data = default(DepartmentListViewModel),
                    Status = OperationResult.NotFound,
                    Message = string.Format("CompanyId {0} Not Found", model.CompanyId)
                };

            if (!string.IsNullOrEmpty(model.ParentDepartmentId))
            {
                var parentDepartment = _departmentService.Find(model.ParentDepartmentId);
                if (parentDepartment == null)
                {
                    return new OperationResult<DepartmentListViewModel>
                    {
                        Data = default(DepartmentListViewModel),
                        Status = OperationResult.NotFound,
                        Message = string.Format("ParentDepartment Id {0} Not Found", model.ParentDepartmentId)
                    };
                }
            }

            if (!string.IsNullOrEmpty(model.DepartmentHeadEmployeeId))
            {
                var departmentHead = _employeeService.Find(model.DepartmentHeadEmployeeId);
                if (departmentHead == null)
                {
                    return new OperationResult<DepartmentListViewModel>
                    {
                        Data = default(DepartmentListViewModel),
                        Status = OperationResult.NotFound,
                        Message = string.Format("DepartmentHead Id {0} Not Found", model.DepartmentHeadEmployeeId)
                    };
                }
            }

            var department = new Department()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                CompanyId = model.CompanyId,
                ParentDepartmentId = model.ParentDepartmentId,
                DepartmentHeadEmployeeId = model.DepartmentHeadEmployeeId,
                CreatedById = createdById,
                CreatedDate = DateTime.UtcNow
            };            

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _departmentService.Insert(department);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();

                var data = Get(department.Id).Data.FirstOrDefault();

                return new OperationResult<DepartmentListViewModel>
                {
                    Data = data,
                    Status = OperationResult.Success,
                    Message = "Company saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<DepartmentListViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<DepartmentListViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public OperationResult<DepartmentListViewModel> Delete(string id)
        {
            var department = _departmentService.Find(id);

            if (department == null)
            {
                return new OperationResult<DepartmentListViewModel>
                {
                    Data = default(DepartmentListViewModel),
                    Status = OperationResult.NotFound,
                    Message = "Not Found"
                };
            }

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _departmentService.Delete(department.Id);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return new OperationResult<DepartmentListViewModel>
                {
                    Data = default(DepartmentListViewModel),
                    Status = OperationResult.Success,
                    Message = "Department deleted."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<DepartmentListViewModel>
                {
                    Data = default(DepartmentListViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<DepartmentListViewModel>
                {
                    Data = default(DepartmentListViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public Result<DepartmentListViewModel> Get(string id)
        {
            List<DepartmentListViewModel> results = new List<DepartmentListViewModel>();

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order).Where(x => string.Compare(x.Id, id, true) == 0).FirstOrDefault();

            if (data != null)
                results.Add(data);

            total = results.Count();

            return new Result<DepartmentListViewModel> { Data = results, Total = total };
        }

        public Result<DepartmentListViewModel> Get(int pageSize, int page, int skip, int take, string sorting, string filter)
        {
            List<DepartmentListViewModel> results = new List<DepartmentListViewModel>();

            int total = 0;

            string order = "Id";

            Filtering filtering = null;

            // order by
            if (sorting != null && sorting != "undefined")
            {
                List<Sort> sort = JsonConvert.DeserializeObject<List<Sort>>(sorting);
                List<string> sorts = new List<string>();
                sort.ForEach(x => sorts.Add(string.Format("{0} {1}", x.field, x.dir)));
                order = string.Join(", ", sorts.ToArray());
            }

            var data = GetQueryable(order);

            var tmp = data.ToList();
            // filtered by
            if (!string.IsNullOrEmpty(filter) && filter != "null")
            {
                filtering = JsonConvert.DeserializeObject<Filtering>(filter);

                var predicate = new DynamicLinqHelper<DepartmentListViewModel>().CreateFilterPredicate(filtering.filters, true);

                total = data.Where(predicate).Count();

                results = data.Where(predicate).Skip(skip).Take(take).ToList(); ;

                return new Result<DepartmentListViewModel> { Data = results, Total = total };
            }
            else
            {
                total = 0;

                results = data.Skip(0).Take(0).ToList();

                return new Result<DepartmentListViewModel> { Data = results, Total = total };
            }
        }

        public Result<DepartmentDisplayViewModel> GetDisplayList(string companyId)
        {
            List<DepartmentDisplayViewModel> results = new List<DepartmentDisplayViewModel>();

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order).Where(x => string.Compare(x.CompanyId, companyId, true) == 0);

            results.Add(new DepartmentDisplayViewModel { Id = "", Name = "" });

            data.ToList().ForEach(x =>
            {
                results.Add(DataMapper.Map<DepartmentListViewModel, DepartmentDisplayViewModel>(x));
            });
            //results.AddRange(data);

            total = results.Count();

            return new Result<DepartmentDisplayViewModel> { Data = results, Total = total };
        }

        public Result<DepartmentDisplayViewModel> GetDisplayListExceptSelf(string companyId, string id)
        {
            List<DepartmentDisplayViewModel> results = new List<DepartmentDisplayViewModel>();

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order).Where(x => string.Compare(x.CompanyId, companyId, true) == 0 && string.Compare(x.Id, id, true) != 0);

            results.Add(new DepartmentDisplayViewModel { Id = "", Name = "" });

            data.ToList().ForEach(x =>
            {
                results.Add(DataMapper.Map<DepartmentListViewModel, DepartmentDisplayViewModel>(x));
            });
            //results.AddRange(data);

            total = results.Count();

            return new Result<DepartmentDisplayViewModel> { Data = results, Total = total };
        }

        public Result<DepartmentListViewModel> GetChildren(string id)
        {
            List<DepartmentListViewModel> results = new List<DepartmentListViewModel>();

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order).Where(x => x.ParentDepartmentId == id);

            results.Add(new DepartmentListViewModel { Id = "", Name = "" });

            results.AddRange(data);

            total = results.Count();

            return new Result<DepartmentListViewModel> { Data = results, Total = total };
        }

        public OperationResult<DepartmentListViewModel> Update(DepartmentListViewModel model)
        {
            var department = _departmentService.Find(model.Id);

            if (department == null)
                return new OperationResult<DepartmentListViewModel>
                {
                    Data = default(DepartmentListViewModel),
                    Status = OperationResult.NotFound,
                    Message = string.Format("Department Id {0} Not Found", model.Id)
                };

            var company = _companyService.Find(model.CompanyId);

            if (company == null)
                return new OperationResult<DepartmentListViewModel>
                {
                    Data = default(DepartmentListViewModel),
                    Status = OperationResult.NotFound,
                    Message = string.Format("CompanyId {0} Not Found", model.CompanyId)
                };

            if (!string.IsNullOrEmpty(model.ParentDepartmentId))
            {
                var parentDepartment = _departmentService.Find(model.ParentDepartmentId);
                if (parentDepartment == null)
                {
                    return new OperationResult<DepartmentListViewModel>
                    {
                        Data = default(DepartmentListViewModel),
                        Status = OperationResult.NotFound,
                        Message = string.Format("ParentDepartmentId {0} Not Found", model.ParentDepartmentId)
                    };
                }
            }
            else
                model.ParentDepartmentId = null;

            if (!string.IsNullOrEmpty(model.DepartmentHeadEmployeeId))
            {
                var departmentHead = _employeeService.Find(model.DepartmentHeadEmployeeId);
                if (departmentHead == null)
                {
                    return new OperationResult<DepartmentListViewModel>
                    {
                        Data = default(DepartmentListViewModel),
                        Status = OperationResult.NotFound,
                        Message = string.Format("DepartmentHead Id {0} Not Found", model.DepartmentHeadEmployeeId)
                    };
                }
            }
            else
                model.DepartmentHeadEmployeeId = null;

            department.Name = model.Name;
            department.CompanyId = model.CompanyId;
            department.ParentDepartmentId = model.ParentDepartmentId;
            department.DepartmentHeadEmployeeId = model.DepartmentHeadEmployeeId;

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _departmentService.Update(department);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();

                var data = Get(model.Id).Data.FirstOrDefault();

                return new OperationResult<DepartmentListViewModel>
                {
                    Data = data,
                    Status = OperationResult.Success,
                    Message = "Department saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<DepartmentListViewModel>
                {
                    Data = default(DepartmentListViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<DepartmentListViewModel>
                {
                    Data = default(DepartmentListViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        private DepartmentListViewModel ConvertFrom(Department department)
        {
            var data = DataMapper.Map<Department, DepartmentListViewModel>(department);

            if (department.ParentDepartment != null)
                data.ParentDepartmentName = department.ParentDepartment.Name;
            else
            {
                data.ParentDepartmentName = "";
            }

            data.CompanyName = department.Company.Name;

            if (department.DepartmentHead != null)
            {
                var employeeId = department.DepartmentHeadEmployeeId;

                var employee = _employeeService.Query(x => x.Id == employeeId).Include(x => x.ApplicationUser).Select().FirstOrDefault();

                if (employee != null)
                    data.DepartmentHeadUserName = employee.ApplicationUser.UserName;
            }                
            else
            {
                data.DepartmentHeadUserName = "";
            }

            var departments = new List<DepartmentListViewModel>();

            if (department.ChildrenDepartments != null)
                department.ChildrenDepartments.ToList().ForEach(x => {
                    departments.Add(ConvertFrom(x));
                });

            data.ChildrenDepartments = departments;

            data.CreatedById = department.CreatedById;

            data.CreatedByUserName = department.CreatedBy.UserName;

            return data;
            //var data = DataMapper.Map<Department, DepartmentViewModel>(department);

            //if (department.ParentDepartment != null)
            //    data.ParentDepartment = DataMapper.Map<Department, DepartmentDisplayViewModel>(department.ParentDepartment);
            //else
            //{
            //    data.ParentDepartment = new DepartmentDisplayViewModel { Id = "", Name = "" };
            //}

            //data.Company = DataMapper.Map<Company, CompanyViewModel>(department.Company);

            //if (department.DepartmentHead != null)
            //    data.DepartmentHead = DataMapper.Map<Employee, EmployeeViewModel>(department.DepartmentHead);
            //else
            //{
            //    data.DepartmentHead = new EmployeeViewModel
            //    {
            //        Id = "",
            //        ApplicationUser = new DisplayUserBindingModel
            //        {
            //            Id = "", UserName = "", FirstName = "", LastName = "", Email = ""
            //        }
            //    };
            //}

            //var departments = new List<DepartmentViewModel>();

            //if (department.ChildrenDepartments != null)
            //    department.ChildrenDepartments.ToList().ForEach(x => {
            //        departments.Add(ConvertFrom(x));
            //    });

            //data.ChildrenDepartments = departments;

            //data.CreatedBy = DataMapper.Map<ApplicationUser, DisplayUserBindingModel>(department.CreatedBy);

            //return data;
        }
    }

    public interface IDepartmentBizService
    {
        Result<DepartmentListViewModel> Get(int pageSize, int page, int skip, int take, string sorting, string filter);

        Result<DepartmentListViewModel> Get(string id);

        Result<DepartmentDisplayViewModel> GetDisplayList(string companyId);

        Result<DepartmentDisplayViewModel> GetDisplayListExceptSelf(string companyId, string id);

        Result<DepartmentListViewModel> GetChildren(string id);

        OperationResult<DepartmentListViewModel> Create(DepartmentListViewModel model, string createdById);

        OperationResult<DepartmentListViewModel> Update(DepartmentListViewModel model);

        OperationResult<DepartmentListViewModel> Delete(string id);

    }
}