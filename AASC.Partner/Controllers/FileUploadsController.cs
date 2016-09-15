using AASC.FW.Repositories;
using AASC.FW.UnitOfWork;
using AASC.Partner.API.ErrorHelpers;
using AASC.Partner.API.Models;
using AASC.Partner.API.Providers;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AASC.Partner.API.Controllers
{
    [Authorize(Roles = "ContentProvider")]
    [RoutePrefix("api/files")]
    public class FileUploadsController : BaseApiController
    {
        protected IRepository<FileUpload> _repository;
        protected IUnitOfWorkAsync _unitOfWork;

        public FileUploadsController(IRepository<FileUpload> repository, IUnitOfWorkAsync unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Route("upload")]
        public async Task<IHttpActionResult> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            var root = "~/App_Data/Tmp/FileUploads/";

            try
            {
                var provider = GetMultipartProvider(root);
                var result = await Request.Content.ReadAsMultipartAsync(provider);
                var fileStore = GetFormData(result);
                
                _unitOfWork.BeginTransaction();
                _unitOfWork.Repository<FileUpload>().Insert(fileStore);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();

                var originalFileName = result.FileData.FirstOrDefault().LocalFileName;
                var path = Path.GetDirectoryName(originalFileName);

                // change file name; file name will be stored as fileStore.Id + "_" + fileStore.FileName in folder
                try
                {
                    path = path.Replace("temp", fileStore.FileFolder);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    File.Move(originalFileName, Path.Combine(path, fileStore.Id + "_" + fileStore.FileName));
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
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var error in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation.", error.Entry.Entity.GetType());
                    sb.AppendLine();
                    foreach (var ve in error.ValidationErrors)
                    {
                        sb.AppendFormat("{0} -{1}", ve.PropertyName, ve.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                var errMessage = sb.ToString();
                Console.WriteLine(errMessage);
                _unitOfWork.Rollback();
                throw new ApiDataException(1001, errMessage, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }            

            return Ok();
        }

        private CustomMultipartFormDataStreamProvider GetMultipartProvider(string root)
        {
            var tempFolder = HttpContext.Current.Server.MapPath(root + "temp/");
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);
            return new CustomMultipartFormDataStreamProvider(tempFolder);
        }

        private FileUpload GetFormData(CustomMultipartFormDataStreamProvider result)
        {
            var fileStore = new FileUpload();
            if (result.FormData != null)
            {
                var subFolder = result.FormData.GetValues("folder").FirstOrDefault();
                if (subFolder.IndexOf('/') == 0)
                    subFolder = subFolder.Substring(1, subFolder.Length - 1);

                var note = result.FormData.GetValues("note").FirstOrDefault();

                var userId = User.Identity.GetUserId();

                using (var fs = File.OpenRead(result.FileData.FirstOrDefault().LocalFileName))
                {
                    var size = (int)fs.Length;
                    var data = new byte[size];
                    fs.Read(data, 0, size);
                    // save file to server
                    fileStore = new FileUpload
                    {
                        Id = Guid.NewGuid().ToString(),
                        FileContent = data,
                        FileFolder = Uri.UnescapeDataString(subFolder),
                        FileName = result.FileData.FirstOrDefault().Headers.ContentDisposition.FileName.Replace("\"", string.Empty),
                        MimeType = result.FileData.FirstOrDefault().Headers.ContentType.MediaType,
                        Note = Uri.UnescapeDataString(note),
                        CreatedDate = DateTime.UtcNow,
                        CreatedById = userId
                    };
                }
            }

            return fileStore;
        }
    }
}
