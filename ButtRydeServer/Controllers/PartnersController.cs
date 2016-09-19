//using AASC.FW.Repositories;
//using AASC.FW.UnitOfWork;
//using AASC.Partner.API.ErrorHelpers;
//using AASC.Partner.API.Helpers;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Dynamic;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Web.Http;

//namespace AASC.Partner.API.Controllers
//{
//    public class PartnersController : BaseApiController
//    {
//        protected IRepository<AASC.Partner.API.Models.Partner> _repository;

//        protected IUnitOfWork _unitOfWork;

//        public PartnersController(IRepository<AASC.Partner.API.Models.Partner> repository, IUnitOfWorkAsync unitOfWork)
//        {
//            _repository = repository;
//            _unitOfWork = unitOfWork;
//        }

//        [HttpGet]
//        [Route("")]
//        public IHttpActionResult Get()
//        {
//            try
//            {
//                List<AASC.Partner.API.Models.Partner> results = new List<AASC.Partner.API.Models.Partner>();

//                var request = HttpContext.Current.Request;
//                int pageSize = 10;
//                int.TryParse(request["pageSize"], out pageSize);
//                int take = pageSize;
//                int.TryParse(request["take"], out take);
//                if (take == 0) take = 10;
//                int skip = 0;
//                int.TryParse(request["skip"], out skip);
//                int page = 0;
//                int.TryParse(request["page"], out page);

//                var sorting = request["sorting"];
//                var filter = request["filter"];

//                string order = "Id";
//                Filtering filtering = null;

//                int total = 0;
//                // order by
//                if (sorting != null && sorting != "undefined")
//                {
//                    List<Sort> sort = JsonConvert.DeserializeObject<List<Sort>>(sorting);
//                    List<string> sorts = new List<string>();
//                    sort.ForEach(x => sorts.Add(string.Format("{0} {1}", x.field, x.dir)));
//                    order = string.Join(", ", sorts.ToArray());
//                }

//                // filtered by
//                if (!string.IsNullOrEmpty(filter) && filter != "null")
//                {
//                    filtering = JsonConvert.DeserializeObject<Filtering>(filter);
//                    var predicate = new DynamicLinqHelper<AASC.Partner.API.Models.Partner>().CreateFilterPredicate(filtering.filters, true);
//                    total = _unitOfWork.Repository<AASC.Partner.API.Models.Partner>().Query(predicate).Select().Count();
//                    var data = _unitOfWork.Repository<AASC.Partner.API.Models.Partner>().Query(predicate)
//                        .Include(x => x.UserAccounts).Include(x=>x.PartmentAgreements).Select().AsQueryable()
//                        .OrderBy(order.ToString()).Skip(skip).Take(take);

//                    foreach (var d in data)
//                    {
//                        results.Add(d);
//                    }
//                    return Ok(new { data = results, total = total });
//                }
//                else
//                {
//                    total = _unitOfWork.Repository<AASC.Partner.API.Models.Partner>().Query().Select().Count();
//                    var data = _unitOfWork.Repository<AASC.Partner.API.Models.Partner>().Query()
//                        .Include(x => x.UserAccounts).Include(x=>x.PartmentAgreements).Select().AsQueryable()
//                        .OrderBy(order.ToString()).Skip(skip).Take(take);

//                    foreach (var d in data)
//                    {
//                        results.Add(d);
//                    }
//                    return Ok(new { data = results, total = total });
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
//            }
//        }

//        [Route("")]
//        [HttpPost]
//        [System.Web.Mvc.ValidateAntiForgeryToken]
//        public IHttpActionResult PostPartner(AASC.Partner.API.Models.Partner model)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            model.Id = Guid.NewGuid();
//            _unitOfWork.BeginTransaction();
//            _unitOfWork.Repository<AASC.Partner.API.Models.Partner>().Insert(model);
//            _unitOfWork.SaveChanges();
//            _unitOfWork.Commit();

//            return Ok(model);
//        }

//        [Route("put/{id}")]
//        [HttpPut]
//        [System.Web.Mvc.ValidateAntiForgeryToken]
//        public IHttpActionResult PutPartner(AASC.Partner.API.Models.Partner model)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var company = _unitOfWork.Repository<AASC.Partner.API.Models.Partner>().Find(model.Id);
//            if (company == null)
//            {
//                return NotFound();
//            }

//            company.Name = model.Name;

//            var result = _unitOfWork.SaveChanges();

//            if (result != 1)
//                return BadRequest();
//            else
//                return Ok(model);
//        }

//        [Route("{id:guid}")]
//        [System.Web.Mvc.ValidateAntiForgeryToken]
//        public IHttpActionResult DeletePartner(string id)
//        {
//            Guid resourceId = Guid.Parse(id);
//            var roadmap = _unitOfWork.Repository<AASC.Partner.API.Models.Partner>().Find(resourceId);

//            if (roadmap != null)
//            {
//                _unitOfWork.Repository<AASC.Partner.API.Models.Partner>().Delete(roadmap);
//                _unitOfWork.SaveChanges();
//                return Ok();
//            }

//            return NotFound();
//        }
//    }
//}
