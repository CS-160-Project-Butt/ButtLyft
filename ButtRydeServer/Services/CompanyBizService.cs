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
using System.Linq.Dynamic;
using System.Web;

namespace AASC.Partner.API.Services
{
    public class CompanyBizService : ICompanyBizService
    {
        protected readonly ICompanyDataService _companyService;

        protected readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public CompanyBizService(
            IUnitOfWorkAsync unitOfWorkAsync,
            ICompanyDataService companyService
        )
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _companyService = companyService;
        }

        private IQueryable<CompanyListViewModel> GetQueryable(string order)
        {
            var projection = _companyService.Query()
                .Include(x => x.Partners).Include(x => x.Departments).Include(x => x.CreatedBy).Select().AsQueryable()
                .OrderBy(order).ToList();

            var data = new List<CompanyListViewModel>();

            projection.ForEach(x => {
                var d = ConvertFrom(x);
                data.Add(d);
            });

            return data.AsQueryable();
        }        

        public OperationResult<CompanyListViewModel> Create(CompanyListViewModel model, string createdById)
        {
            var company = new Company()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                CreatedById = createdById,
                CreatedDate = DateTime.UtcNow
            };
            
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _companyService.Insert(company);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();

                var data = Get(company.Id).Data.FirstOrDefault();

                return new OperationResult<CompanyListViewModel>
                {
                    Data = data,
                    Status = OperationResult.Success,
                    Message = "Company saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<CompanyListViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<CompanyListViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public OperationResult<CompanyListViewModel> Delete(string id)
        {
            var company = _companyService.Find(id);

            if (company == null)
            {
                return new OperationResult<CompanyListViewModel>
                {
                    Data = default(CompanyListViewModel),
                    Status = OperationResult.NotFound,
                    Message = "Not Found"
                };
            }

            try
            {                
                _unitOfWorkAsync.BeginTransaction();
                _companyService.Delete(company.Id);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return new OperationResult<CompanyListViewModel>
                {
                    Data = default(CompanyListViewModel),
                    Status = OperationResult.Success,
                    Message = "Company deleted."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<CompanyListViewModel>
                {
                    Data = default(CompanyListViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<CompanyListViewModel>
                {
                    Data = default(CompanyListViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public Result<CompanyListViewModel> Get(string id)
        {
            List<CompanyListViewModel> results = new List<CompanyListViewModel>();

            int total = 0;

            string order = "Id";

            Guid companyGuid = Guid.Parse(id);

            var query = GetQueryable(order);

            var data1 = query.ToList();

            var data = query.Where(x=>string.Compare(x.Id, companyGuid.ToString(), true) == 0).FirstOrDefault();

            results.Add(data);

            total = results.Count();

            return new Result<CompanyListViewModel> { Data = results, Total = total };
        }

        public Result<CompanyListViewModel> Get(int pageSize, int page, int skip, int take, string sorting, string filter)
        {
            List<CompanyListViewModel> results = new List<CompanyListViewModel>();

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

            // filtered by
            if (!string.IsNullOrEmpty(filter) && filter != "null")
            {
                filtering = JsonConvert.DeserializeObject<Filtering>(filter);

                var predicate = new DynamicLinqHelper<CompanyListViewModel>().CreateFilterPredicate(filtering.filters, true);

                total = data.Where(predicate).Count();
                foreach (var d in data.Where(predicate).Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<CompanyListViewModel> { Data = results, Total = total };
            }
            else
            {

                total = data.Count();
                foreach (var d in data.Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<CompanyListViewModel> { Data = results, Total = total };
            }
        }

        public OperationResult<CompanyListViewModel> Update(CompanyListViewModel model)
        {
            var company = _companyService.Find(model.Id);            

            if (company == null)
                return new OperationResult<CompanyListViewModel>
                {
                    Data = default(CompanyListViewModel),
                    Status = OperationResult.Failed,
                    Message = string.Format("Company {0} not exists.", model.Id)
                };

            company.Name = model.Name;

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _companyService.Update(company);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();

                var data = Get(model.Id).Data.FirstOrDefault();

                return new OperationResult<CompanyListViewModel>
                {
                    Data = data,
                    Status = OperationResult.Success,
                    Message = "Company saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<CompanyListViewModel>
                {
                    Data = default(CompanyListViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<CompanyListViewModel>
                {
                    Data = default(CompanyListViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public Result<CompanyListViewModel> GetDisplayList()
        {
            List<CompanyListViewModel> results = new List<CompanyListViewModel>();

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order);

            //results.Add(default(CompanyViewModel));

            results.AddRange(data);

            total = results.Count();

            return new Result<CompanyListViewModel> { Data = results, Total = total };
        }

        private CompanyListViewModel ConvertFrom(Company company)
        {
            var data = DataMapper.Map<Company, CompanyListViewModel>(company);            
            
            var departments = new List<DepartmentListViewModel>();

            company.Departments.ToList().ForEach(x => {
                departments.Add(DataMapper.Map<Department, DepartmentListViewModel>(x));
            });

            data.Departments = departments;

            // TODO: data.Partners

            data.CreatedByUserName = company.CreatedBy.UserName;            

            return data;
        }
    }

    public interface ICompanyBizService
    {
        Result<CompanyListViewModel> Get(int pageSize, int page, int skip, int take, string sorting, string filter);

        Result<CompanyListViewModel> Get(string id);

        Result<CompanyListViewModel> GetDisplayList();

        OperationResult<CompanyListViewModel> Create(CompanyListViewModel model, string createdById);

        OperationResult<CompanyListViewModel> Update(CompanyListViewModel model);

        OperationResult<CompanyListViewModel> Delete(string id);

    }
}