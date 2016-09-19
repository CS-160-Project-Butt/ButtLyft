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
//using AASC.Partner.API.Models;

//namespace AASC.Partner.API.Controllers
//{
//    public class PartnerAgreementsController : BaseApiController
//    {
//        protected IRepository<PartnerAgreement> _repository;

//        protected IUnitOfWork _unitOfWork;

//        public PartnerAgreementsController(IRepository<PartnerAgreement> repository, IUnitOfWorkAsync unitOfWork)
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
//                List<PartnerAgreement> results = new List<PartnerAgreement>();

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
//                    var predicate = new DynamicLinqHelper<PartnerAgreement>().CreateFilterPredicate(filtering.filters, true);
//                    total = _unitOfWork.Repository<PartnerAgreement>().Query(predicate).Select().Count();
//                    var data = _unitOfWork.Repository<PartnerAgreement>().Query(predicate)
//                        .Include(x => x.Partner).Select().AsQueryable()
//                        .OrderBy(order.ToString()).Skip(skip).Take(take);

//                    foreach (var d in data)
//                    {
//                        results.Add(d);
//                    }
//                    return Ok(new { data = results, total = total });
//                }
//                else
//                {
//                    total = _unitOfWork.Repository<PartnerAgreement>().Query().Select().Count();
//                    var data = _unitOfWork.Repository<PartnerAgreement>().Query()
//                        .Include(x => x.Partner).Select().AsQueryable()
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
//        public IHttpActionResult PostPartnerAgreement(PartnerAgreement model)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            model.Id = Guid.NewGuid();
//            _unitOfWork.BeginTransaction();
//            _unitOfWork.Repository<PartnerAgreement>().Insert(model);
//            _unitOfWork.SaveChanges();
//            _unitOfWork.Commit();

//            return Ok(model);
//        }

//        [Route("put/{id}")]
//        [HttpPut]
//        [System.Web.Mvc.ValidateAntiForgeryToken]
//        public IHttpActionResult PutPartnerAgreement(PartnerAgreement model)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var agreement = _unitOfWork.Repository<PartnerAgreement>().Find(model.Id);
//            if (agreement == null)
//            {
//                return NotFound();
//            }

//            agreement.Description = model.Description;
//            agreement.TransactionType = model.TransactionType;
//            agreement.InvoiceCycle = model.InvoiceCycle;
//            agreement.InvoiceDayOn = model.InvoiceDayOn;
//            agreement.PartnerId = model.PartnerId;            
//            agreement.StartDate = model.StartDate;
//            agreement.ExpireDate = model.ExpireDate;
//            agreement.FileUploadId = model.FileUploadId;

//            var result = _unitOfWork.SaveChanges();

//            if (result != 1)
//                return BadRequest();
//            else
//                return Ok(model);
//        }

//        [Route("{id:guid}")]
//        [System.Web.Mvc.ValidateAntiForgeryToken]
//        public IHttpActionResult DeletePartnerAgreement(string id)
//        {
//            Guid resourceId = Guid.Parse(id);
//            var roadmap = _unitOfWork.Repository<PartnerAgreement>().Find(resourceId);

//            if (roadmap != null)
//            {
//                _unitOfWork.Repository<PartnerAgreement>().Delete(roadmap);
//                _unitOfWork.SaveChanges();
//                return Ok();
//            }

//            return NotFound();
//        }
//    }
//}
