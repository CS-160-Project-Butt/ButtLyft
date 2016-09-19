using AASC.FW.DataMapper;
using AASC.Partner.API.Utilities;
using AASC.Partner.API.ErrorHelpers;
using AASC.Partner.API.Models;
using AASC.Partner.API.Services;
using System;
using System.Collections.Generic;
using System.Net;
using AASC.Partner.API.Configuration.Cla;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.Web.Http;

namespace AASC.Partner.API.Controllers
{       

    [Authorize(Roles = "ActiveUser, Sales, SalesManager, ProductManager")]
    [RoutePrefix("api/cla")]
    public class CLAController : BaseApiController
    {
        protected readonly ICLABizService _claFormService;

        public CLAController(
            ICLABizService claFormService
            )
        {
            _claFormService = claFormService;
        }
        
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
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

                var result = _claFormService.Get(pageSize, page, skip, take, sorting, filter);

                return Ok(new { data = result.Data, total = result.Total });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [Route("updateclaform")]
        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> UpdateCLAForm(CLAFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = _claFormService.Update(model);

            if (result.Status == OperationResult.Success) {
            await ContactSales(result.Data.SalesContact, result.Data.Id);

            return Ok(result.Data); }
            else
                return BadRequest(result.Message);
        }

        public async Task ContactSales(string salesRep, string id)
        {
            // send out email
            List<string> recipients = new List<string>();

            var salesEmail = CLAConfig.getEmail(salesRep);
            var pmEmail = CLAConfig.getPMEmail();
            var buyEmail = ConfigurationManager.AppSettings["emailService:Follower"];
            var adminEmail = ConfigurationManager.AppSettings["emailService:Admin"];
            var testEmail = "jeff.li@sjsu.edu";
            //recipients.Add(salesEmail);
            //recipients.Add(pmEmail);
            //recipients.Add(buyEmail);
            //recipients.Add(adminEmail);
            recipients.Add(testEmail);

            EmailServer emailServer = new EmailServer();
            var body = _claFormService.GetEmailBody(id);
            await emailServer.Send("CLA Form Application Notification", body, recipients);
        }

        [Route("addcla")]
        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> AddCLAForm(CLAFormViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _claFormService.Create(model);

            if (result.Status == OperationResult.Success)
            {
                await ContactSales(result.Data.SalesContact, result.Data.Id);

                return Ok(result.Data);
            }
            else
                return BadRequest(result.Message);
        }
        [AllowAnonymous]
        [Route("getcla")]
        [HttpGet]
        public IHttpActionResult GetCLAForm(String Id)
        {
            try
            {
                List<CLAFormListViewModel> results = new List<CLAFormListViewModel>();

                var result = _claFormService.GetDisplayList();

                result.Data.ForEach(x => {
                    if (x.Id == Id)
                        results.Add(x);
                });
                if (results.Count <= 0)
                {
                    throw new Exception("Form not found using given Id.");
                }
                return Ok(new { data = results, total = result.Total });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("getstatuses")]
        public IHttpActionResult GetStatuses()
        {
            try
            {
                var statuses = CLAConfig.GetStatuses();

                return Ok(new { data = statuses, total = statuses.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("getdevices")]
        public IHttpActionResult GetDevices()
        {
            try
            {
                var results = new List<object>();
                var devices = CLAConfig.GetDevices();

                foreach (var d in devices)
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
        [Route("getproducttypes")]
        public IHttpActionResult GetProductTypes()
        {
            try
            {
                var types = CLAConfig.GetProductTypes();

                return Ok(new { data = types, total = types.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getproducts")]
        public IHttpActionResult GetProducts()
        {
            try
            {
                var products = CLAConfig.GetProducts();

                return Ok(new { data = products, total = products.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getsalesreps")]
        public IHttpActionResult GetSalesReps()
        {
            try
            {
                var reps = CLAConfig.GetSalesReps();

                return Ok(new { data = reps, total = reps.Count });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }
    }

}
