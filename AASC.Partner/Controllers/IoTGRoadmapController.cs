using AASC.FW.Repositories;
using AASC.FW.UnitOfWork;
using AASC.Partner.API.Configuration.Intel;
using AASC.Partner.API.ErrorHelpers;
using AASC.Partner.API.Helpers;
using AASC.Partner.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace AASC.Partner.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/IoTGRoadmap")]
    public class IoTGRoadmapController : BaseApiController
    {
        protected IRepository<IoTGRoadmap> _repository;

        protected IUnitOfWork _unitOfWork;

        public IoTGRoadmapController(IRepository<IoTGRoadmap> repository, IUnitOfWorkAsync unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                List<IoTGRoadmap> results = new List<IoTGRoadmap>();

                var request = HttpContext.Current.Request;
                int pageSize = 10;
                int.TryParse(request["pageSize"], out pageSize);
                int take = pageSize;
                int.TryParse(request["take"], out take);
                if (take == 0) take = 10;
                int skip = 0;
                int.TryParse(request["skip"], out skip);
                int page = 0;
                int.TryParse(request["page"], out page);

                var sorting = request["sorting"];
                var filter = request["filter"];

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

                // filtered by
                if (!string.IsNullOrEmpty(filter) && filter != "null")
                {
                    filtering = JsonConvert.DeserializeObject<Filtering>(filter);
                    var predicate = new DynamicLinqHelper<IoTGRoadmap>().CreateFilterPredicate(filtering.filters, true);
                    total = _unitOfWork.Repository<IoTGRoadmap>().Query(predicate).Select().Count();
                    var files = _unitOfWork.Repository<IoTGRoadmap>().Query(predicate).Include(x => x.Contact).Select().AsQueryable()
                        .OrderBy(order.ToString()).Skip(skip).Take(take);

                    foreach (var f in files)
                    {
                        results.Add(f);
                    }
                    return Ok(new { data = results, total = total });
                }
                else
                {
                    total = _unitOfWork.Repository<IoTGRoadmap>().Query().Select().Count();
                    var files = _unitOfWork.Repository<IoTGRoadmap>().Query().Include(x => x.Contact).Select().AsQueryable()
                        .OrderBy(order.ToString()).Skip(skip).Take(take);

                    foreach (var f in files)
                    {
                        results.Add(f);
                    }
                    return Ok(new { data = results, total = total });
                }
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [Route("")]
        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult PostIoTGRoadmap(IoTGRoadmap model)
        {            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.Id = Guid.NewGuid().ToString();
            _unitOfWork.BeginTransaction();
            _unitOfWork.Repository<IoTGRoadmap>().Insert(model);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

            return Ok(model);
        }

        [Route("put/{id}")]
        [HttpPut]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult PutIoTGRoadmap(IoTGRoadmap model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roadmap = _unitOfWork.Repository<IoTGRoadmap>().Find(model.Id);
            if (roadmap == null)
            {
                return NotFound();
            }

            roadmap.AvailabilityESSample = model.AvailabilityESSample;
            roadmap.AvailabilityMP = model.AvailabilityMP;
            roadmap.Category = model.Category;
            roadmap.CodeName = model.CodeName;
            roadmap.ContactEmployeeId = model.ContactEmployeeId;
            roadmap.FileUploadId = model.FileUploadId;
            roadmap.Level = model.Level;
            roadmap.Link = model.Link;
            roadmap.MarketSegment = model.MarketSegment;
            roadmap.Model = model.Model;
            roadmap.Platform = model.Platform;
            roadmap.Status = model.Status;
            roadmap.Subcategory = model.Subcategory;

            var result = _unitOfWork.SaveChanges();

            if (result != 1)
                return BadRequest();
            else
                return Ok(model);
        }

        [Route("{id:guid}")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult DeleteIoTGRoadmap(string id)
        {
            Guid resourceId = Guid.Parse(id);
            var roadmap = _unitOfWork.Repository<IoTGRoadmap>().Find(resourceId);

            if (roadmap != null)
            {
                _unitOfWork.Repository<IoTGRoadmap>().Delete(roadmap);
                _unitOfWork.SaveChanges();
                return Ok();
            }

            return NotFound();
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("getplatforms")]
        public IHttpActionResult GetPlatforms()
        {
            try
            {
                //var results = new List<string>();
                var results = new List<PlatformBindingModel>();
               
                var platforms = IntelRoadmapConfig.GetPlatforms();

                foreach (var p in platforms)
                {
                    //results.Add(p);
                    results.Add(new PlatformBindingModel { Platform = p });
                }

                return Ok(new { data = results, total = results.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getplatformtrims")]
        public IHttpActionResult GetPlatformTrims()
        {
            try
            {
                //var results = new List<string>();
                var results = new List<PlatformTrimBindingModel>();

                var platforms = IntelRoadmapConfig.GetPlatforms();

                foreach (var p in platforms)
                {
                    var trims = IntelRoadmapConfig.GetTrims(p);
                    foreach(var t in trims)
                    {
                        //results.Add(p);
                        results.Add(new PlatformTrimBindingModel { Platform = p, Trim = t });
                    }
                }

                return Ok(new { data = results, total = results.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getplatformtrimcodenames")]
        public IHttpActionResult GetPlatformTrimCodeNames()
        {
            try
            {
                //var results = new List<string>();
                var results = new List<PlatformTrimCodeNameBindingModel>();

                var platforms = IntelRoadmapConfig.GetPlatforms();

                foreach (var p in platforms)
                {
                    var trims = IntelRoadmapConfig.GetTrims(p);
                    foreach (var t in trims)
                    {
                        var codenames = IntelRoadmapConfig.GetCodeNames(p, t);
                        foreach (var c in codenames)
                        {
                            //results.Add(d);
                            results.Add(new PlatformTrimCodeNameBindingModel { Platform = p, Trim = t, CodeName = c });
                        }
                    }
                }

                return Ok(new { data = results, total = results.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("gettrims/{platform}")]
        public IHttpActionResult GetTrims([FromUri] string platform)
        {
            try
            {
                var results = new List<string>();

                var data = IntelRoadmapConfig.GetTrims(platform);

                foreach (var d in data)
                {
                    results.Add(d);
                }

                return Ok(new { data = results, total = results.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getcategories/{level}")]
        public IHttpActionResult GetCategories([FromUri] string level)
        {
            try
            {
                var results = new List<string>();

                var data = IntelRoadmapConfig.GetCategories(level);

                foreach (var d in data)
                {
                    results.Add(d);
                }

                return Ok(new { data = results, total = results.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getsubcategories/{level}/{category}")]
        public IHttpActionResult GetSubcategories([FromUri] string level, [FromUri] string category)
        {
            try
            {
                var results = new List<string>();

                var data = IntelRoadmapConfig.GetSubcategories(level, category);

                foreach (var d in data)
                {
                    results.Add(d);
                }

                return Ok(new { data = results, total = results.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getmarketsegments")]
        public IHttpActionResult GetMarketSegments()
        {
            try
            {
                var results = new List<string>();

                var data = IntelRoadmapConfig.GetMarketSegments();

                foreach (var d in data)
                {
                    results.Add(d);
                }

                return Ok(new { data = results, total = results.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getlevels")]
        public IHttpActionResult GetLevels()
        {
            try
            {
                //var results = new List<string>();
                var results = new List<ModelLevelBindingModel>();

                var data = IntelRoadmapConfig.GetLevels();

                foreach (var d in data)
                {
                    //results.Add(d);
                    results.Add(new ModelLevelBindingModel { Level = d });
                }

                return Ok(new { data = results, total = results.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getlevelcategories")]
        public IHttpActionResult GetLevelCategories()
        {
            try
            {
                //var results = new List<string>();
                var results = new List<ModelLevelCategoryBindingModel>();

                var data = IntelRoadmapConfig.GetLevels();

                foreach (var d in data)
                {
                    var categories = IntelRoadmapConfig.GetCategories(d);
                    foreach(var c in categories)
                    {
                        //results.Add(d);
                        results.Add(new ModelLevelCategoryBindingModel { Level = d, Category = c });
                    }
                }

                return Ok(new { data = results, total = results.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("getlevelcategorysubcategories")]
        public IHttpActionResult GetLevelCategorySubcategories()
        {
            try
            {
                //var results = new List<string>();
                var results = new List<ModelLevelCategorySubcategoryBindingModel>();

                var data = IntelRoadmapConfig.GetLevels();

                foreach (var d in data)
                {
                    var categories = IntelRoadmapConfig.GetCategories(d);
                    foreach (var c in categories)
                    {
                        var subcategories = IntelRoadmapConfig.GetSubcategories(d, c);
                        foreach(var s in subcategories)
                        {
                            //results.Add(d);
                            results.Add(new ModelLevelCategorySubcategoryBindingModel { Level = d, Category = c, Subcategory = s });
                        }
                    }
                }

                return Ok(new { data = results, total = results.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getroadmapstatus")]
        public IHttpActionResult GetRoadmapStatus()
        {
            try
            {
                var results = new List<string>();

                var data = IntelRoadmapConfig.GetRoadmapStatus();

                foreach (var d in data)
                {
                    results.Add(d);
                }

                return Ok(new { data = results, total = results.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }
    }
}
