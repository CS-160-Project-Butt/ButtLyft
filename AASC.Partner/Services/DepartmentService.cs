using AASC.Partner.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AASC.FW.Repositories;
using AASC.FW.UnitOfWork;
using AASC.Partner.API.Helpers;
using Newtonsoft.Json;

namespace AASC.Partner.API.Services
{
    public class DepartmentService
    {
        protected IRepository<Department> _repository;

        protected IUnitOfWork _unitOfWork;

        public DepartmentService(
            IRepository<Department> repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        private IQueryable<DepartmentViewModel> GetQueryable(string order)
        {
            var projection = _unitOfWork.Repository<Department>().Query()
                    .Include(x => x.Company)
                    .Include(x => x.ParentDepartment)
                    .Include(x => x.DepartmentHead)
                    .Include(x => x.ChildrenDepartments)
                    .Include(x => x.CreatedBy).Select().AsQueryable();

            var data = (from d in projection
                        select new DepartmentViewModel
                        {
                            Id = d.Id,
                            Name = d.Name,
                            CompanyId = d.CompanyId,
                            Company = new CompanyViewModel
                            {
                                Id = d.CompanyId,
                                Name = d.Company.Name
                            },
                            ParentDepartmentId = d.ParentDepartmentId,
                            ParentDepartment = new DepartmentDisplayViewModel
                            {
                                Id = d.ParentDepartmentId,
                                Name = d.ParentDepartment.Name
                            },
                            DepartmentHeadEmployeeId = d.DepartmentHeadEmployeeId,
                            DepartmentHead = new EmployeeViewModel
                            {
                                Id = d.DepartmentHeadEmployeeId,
                                ApplicationUser = new DisplayUserBindingModel
                                {
                                    Id = d.DepartmentHead.ApplicationUserId,
                                    UserName = d.DepartmentHead.ApplicationUser.UserName,
                                    FirstName = d.DepartmentHead.ApplicationUser.FirstName,
                                    LastName = d.DepartmentHead.ApplicationUser.LastName,
                                    Email = d.DepartmentHead.ApplicationUser.Email
                                }
                            },
                            CreatedDate = d.CreatedDate,
                            CreatedById = d.CreatedById,
                            CreatedBy = new DisplayUserBindingModel
                            {
                                Id = d.CreatedById,
                                UserName = d.CreatedBy.UserName,
                                FirstName = d.CreatedBy.FirstName,
                                LastName = d.CreatedBy.LastName,
                                Email = d.CreatedBy.Email
                            },
                        }).OrderBy(order.ToString());
            return data;
        }

        public Result<DepartmentViewModel> Get(int pageSize, int page, int skip, int take, string sorting, string filter)
        {
            List<DepartmentViewModel> results = new List<DepartmentViewModel>();
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
               
                var predicate = new DynamicLinqHelper<DepartmentViewModel>().CreateFilterPredicate(filtering.filters, true);

                total = data.Where(predicate).Count();
                foreach (var d in data.Where(predicate).Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<DepartmentViewModel> { Data = results, Total = total };
            }
            else
            {
                
                total = data.Count();
                foreach (var d in data.Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<DepartmentViewModel> { Data = results, Total = total };
            }
        }

        public Result<DepartmentViewModel> Get(string companyId)
        {
            List<DepartmentViewModel> results = new List<DepartmentViewModel>();

            int total = 0;

            string order = "Id";            

            var data = GetQueryable(order);

            Guid companyGuid = new Guid(companyId);

            foreach(var d in data.Where(x => x.CompanyId == companyGuid.ToString()))
            {               
                results.Add(d);
            }

            total = results.Count();
            return new Result<DepartmentViewModel> { Data = results, Total = total };            
        }

        public Result<DepartmentDisplayViewModel> GetChildren(string departmentId)
        {
            List<DepartmentDisplayViewModel> results = new List<DepartmentDisplayViewModel>();

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order);

            Guid departmentGuid = new Guid(departmentId);

            foreach (var d in data.Where(x => x.ParentDepartmentId == departmentGuid.ToString()))
            {
                results.Add(new DepartmentDisplayViewModel() { Id = d.Id, Name = d.Name });
            }

            total = results.Count();
            return new Result<DepartmentDisplayViewModel> { Data = results, Total = total };
        }

        public Result<DepartmentDisplayViewModel> GetDepartmentList(string companyId)
        {
            List<DepartmentDisplayViewModel> results = new List<DepartmentDisplayViewModel>();

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order);

            Guid companyGuid = new Guid(companyId);

            results.Add(new DepartmentDisplayViewModel() { Id = null, Name = "" });

            foreach (var d in data.Where(x => x.CompanyId == companyGuid.ToString()))
            {
                results.Add(new DepartmentDisplayViewModel() { Id = d.Id, Name = d.Name });
            }

            total = results.Count();
            return new Result<DepartmentDisplayViewModel> { Data = results, Total = total };
        }

        public Result<DepartmentDisplayViewModel> GetDepartmentListExcludeSelf(string companyId, string departmentId)
        {
            List<DepartmentDisplayViewModel> results = new List<DepartmentDisplayViewModel>();

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order);

            Guid companyGuid = new Guid(companyId);

            Guid departmentGuid = new Guid(departmentId);

            results.Add(new DepartmentDisplayViewModel() { Id = null, Name = "" });

            foreach (var d in data.Where(x => x.CompanyId == companyGuid.ToString() && x.Id != departmentGuid.ToString()))
            {
                results.Add(new DepartmentDisplayViewModel() { Id = d.Id, Name = d.Name });
            }

            total = results.Count();
            return new Result<DepartmentDisplayViewModel> { Data = results, Total = total };
        }
    }
}