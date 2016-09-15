using AASC.FW.Repositories;
using AASC.FW.UnitOfWork;
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
    [Authorize(Roles = "ContentProvider")]
    [RoutePrefix("api/resources")]
    public class ResourcesController : BaseApiController
    {
        protected IRepository<FileUpload> _repository;

        protected IUnitOfWork _unitOfWork;

        public ResourcesController(IRepository<FileUpload> repository, IUnitOfWorkAsync unitOfWork)
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
                List<FileUploadBindingModel> results = new List<FileUploadBindingModel>();

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
                    var predicate = new DynamicLinqHelper<FileUpload>().CreateFilterPredicate(filtering.filters, true);
                    total = _unitOfWork.Repository<FileUpload>().Query(predicate).Select().Count();
                    var files = _unitOfWork.Repository<FileUpload>().Query(predicate).Include(x => x.CreatedBy).Select().AsQueryable()
                        .OrderBy(order.ToString()).Skip(skip).Take(take);

                    foreach (var f in files)
                    {
                        results.Add(this.TheModelFactory.Create(f));
                    }
                    return Ok(new { data = results, total = total });
                }
                else
                {
                    total = _unitOfWork.Repository<FileUpload>().Query().Select().Count();
                    var files = _unitOfWork.Repository<FileUpload>().Query().Include(x => x.CreatedBy).Select().AsQueryable()
                        .OrderBy(order.ToString()).Skip(skip).Take(take);

                    foreach (var f in files)
                    {
                        results.Add(this.TheModelFactory.Create(f));
                    }
                    return Ok(new { data = results, total = total });
                }
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [Route("download/{id}")]
        public HttpResponseMessage Get([FromUri] string id)
        {
            HttpResponseMessage result = null;
            Guid resourceId = Guid.Parse(id);
            var fileToDownload = _unitOfWork.Repository<FileUpload>().Find(resourceId);
            string path = HttpContext.Current.Server
                    .MapPath("~/App_Data/Tmp/FileUploads/" + fileToDownload.FileFolder + "/" + fileToDownload.Id + "_" + fileToDownload.FileName);

            if (!File.Exists(path))
            {
                result = new HttpResponseMessage();
                result.StatusCode = HttpStatusCode.NotFound;
                throw new ApiException()
                {
                    ErrorCode = (int)HttpStatusCode.NotFound,
                    ErrorDescription = "File not found!"
                };
            }
            else
            {
                try
                {
                    result = Request.CreateResponse(HttpStatusCode.OK);
                    
                    result.Content = new StreamContent(new FileStream(path, FileMode.Open, FileAccess.Read));                                        
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    result.Content.Headers.Add("x-filename", fileToDownload.FileName);
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileToDownload.FileName;
                    return result;
                }
                catch (Exception ex)
                {
                    throw new ApiException()
                    {
                        ErrorCode = (int)HttpStatusCode.BadRequest,
                        ErrorDescription = string.Format("Bad Request...{0}", ex.Message)
                    };
                }
            }
        }

        [Route("put/{id}")]
        [HttpPut]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult PutResource(FileUploadBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = _unitOfWork.Repository<FileUpload>().Find(model.Id);
            if (resource == null)
            {
                return NotFound();
            }

            resource.IsPublished = model.IsPublished;
            resource.Note = model.Note;

            var result = _unitOfWork.SaveChanges();

            if (result != 1)
                return BadRequest();
            else
                return Ok(model);
        }

        [Route("{id:guid}")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult DeleteResource(string id)
        {
            Guid resourceId = Guid.Parse(id);
            var resource = _unitOfWork.Repository<FileUpload>().Find(resourceId);

            if (resource != null)
            {
                _unitOfWork.Repository<FileUpload>().Delete(resource);
                _unitOfWork.SaveChanges();
                return Ok();
            }

            return NotFound();
        }
    }
}
