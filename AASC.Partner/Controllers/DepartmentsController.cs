using AASC.FW.Repositories;
using AASC.FW.UnitOfWork;
using AASC.Partner.API.ErrorHelpers;
using AASC.Partner.API.Helpers;
using AASC.Partner.API.Models;
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
using AASC.Partner.API.Services;
using System.Data.Entity.Validation;

namespace AASC.Partner.API.Controllers
{
    [RoutePrefix("api/departments")]
    public class DepartmentsController : BaseApiController
    {
        protected readonly IDepartmentBizService _departmentService;

        public DepartmentsController(IDepartmentBizService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                List<DepartmentListViewModel> results = new List<DepartmentListViewModel>();

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

                var result = _departmentService.Get(pageSize, page, skip, take, sorting, filter);

                return Ok(new { data = result.Data, total = result.Total });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }            
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            try
            {
                var result = _departmentService.Get(id);

                return Ok(new { data = result.Data, total = result.Total });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }            
        }

        [HttpGet]
        [Route("getdepartmentlist/{companyId}")]
        public IHttpActionResult GetDepartmentList(string companyId)
        {
            try
            {
                List<DepartmentDisplayViewModel> results = new List<DepartmentDisplayViewModel>();

                var result = _departmentService.GetDisplayList(companyId);

                return Ok(new { data = result.Data, total = result.Total });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [HttpGet]
        [Route("getdepartmentlistexcludeself/{companyId}/{departmentId}")]
        public IHttpActionResult GetDepartmentListExcludeSelf(string companyId, string departmentId)
        {
            try
            {                
                List<DepartmentDisplayViewModel> results = new List<DepartmentDisplayViewModel>();

                var result = _departmentService.GetDisplayListExceptSelf(companyId, departmentId);

                return Ok(new { data = result.Data, total = result.Total });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [HttpGet]
        [Route("getchildren/{departmentId}")]
        public IHttpActionResult GetChildren(string departmentId)
        {
            try
            {                
                List<DepartmentListViewModel> results = new List<DepartmentListViewModel>();

                var result = _departmentService.GetChildren(departmentId);

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
        public IHttpActionResult PostDepartment(DepartmentListViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.Identity.GetUserId();

            var result = _departmentService.Create(model, userId);

            if (result.Status == OperationResult.Success)
                return Ok(result.Data);
            else
                return BadRequest(result.Message);
        }

        [Route("{id}")]
        [HttpPut]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult PutDepartment(DepartmentListViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _departmentService.Update(model);

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
        public IHttpActionResult DeleteDepartment(string id)
        {
            var result = _departmentService.Delete(id);

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
