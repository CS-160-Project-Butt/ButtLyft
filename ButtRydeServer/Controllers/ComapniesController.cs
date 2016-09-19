using AASC.FW.DataMapper;
using AASC.FW.Repositories;
using AASC.FW.UnitOfWork;
using AASC.Partner.API.ErrorHelpers;
using AASC.Partner.API.Helpers;
using AASC.Partner.API.Models;
using AASC.Partner.API.Services;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AASC.Partner.API.Controllers
{
    [Authorize(Roles = "ActiveUser")]
    [RoutePrefix("api/companies")]
    public class ComapniesController : BaseApiController
    {
        protected readonly ICompanyBizService _companyService;

        public ComapniesController(
            ICompanyBizService companyService
            )
        {
            _companyService = companyService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                List<CompanyListViewModel> results = new List<CompanyListViewModel>();

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

                var result = _companyService.Get(pageSize, page, skip, take, sorting, filter);

                return Ok(new { data = result.Data, total = result.Total });                
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [Route("")]
        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult PostCompany(CompanyListViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.Identity.GetUserId();

            var result = _companyService.Create(model, userId);

            if (result.Status == OperationResult.Success)
                return Ok(result.Data);
            else
                return BadRequest(result.Message);            
        }

        [HttpGet]
        [Route("getcompanylist")]
        public IHttpActionResult GetCompanyList()
        {
            try
            {
                List<CompanyDisplayViewModel> results = new List<CompanyDisplayViewModel>();

                var result = _companyService.GetDisplayList();

                result.Data.ForEach(x => {
                    if (x == null)
                        results.Add(new CompanyDisplayViewModel {
                            Id = "",
                            Name = ""
                        });
                    else
                        results.Add(DataMapper.Map<CompanyListViewModel, CompanyDisplayViewModel>(x));
                });

                return Ok(new { data = results, total = result.Total });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }            
        }

        [Route("{id}")]
        [HttpPut]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult PutCompany(CompanyListViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _companyService.Update(model);

            if (result.Status == OperationResult.NotFound)
                return NotFound();
            else
            {
                if (result.Status == OperationResult.Failed)
                    return BadRequest(result.Message);
                else
                    return Ok(result.Data);
            }              
        }

        [Route("{id:guid}")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult DeleteCompany(string id)
        {
            var result = _companyService.Delete(id);

            if (result.Status == OperationResult.NotFound)
                return NotFound();
            else
            {
                if (result.Status == OperationResult.Failed)
                    return BadRequest(result.Message);
                else
                    return Ok();
            }            
        }
    }
}
