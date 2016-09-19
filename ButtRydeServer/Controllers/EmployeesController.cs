using AASC.Partner.API.ErrorHelpers;
using AASC.Partner.API.Models;
using AASC.Partner.API.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Http;

namespace AASC.Partner.API.Controllers
{
    //[Authorize(Roles = "ActiveUser")]
    [RoutePrefix("api/employees")]
    public class EmployeesController : BaseApiController
    {
        protected readonly IEmployeeBizService _employeeService;

        public EmployeesController(
            IEmployeeBizService employeeService
            )
        {
            _employeeService = employeeService;
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

                var result = _employeeService.Get(pageSize, page, skip, take, sorting, filter);

                return Ok(new { data = result.Data, total = result.Total });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [HttpGet]
        [Route("getemployee/{employeeId}")]
        public IHttpActionResult GetEmployee(string employeeId)
        {
            try
            {                
                var result = _employeeService.GetEmployee(employeeId);

                return Ok(new { data = result.Data, total = result.Total });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [HttpGet]
        [Route("getemployeelist/{companyId}")]
        public IHttpActionResult GetEmployeeList(string companyId)
        {
            try
            {
                List<EmployeeDisplayViewModel> results = new List<EmployeeDisplayViewModel>();

                results = _employeeService.GetDisplayList(companyId).Data;

                return Ok(new { data = results, total = results.Count() });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [HttpGet]
        [Route("getemployeelistexcludeself/{companyId}/{employeeId}")]
        public IHttpActionResult GetEmployeeListExcludeSelf(string companyId, string employeeId)
        {
            try
            {
                List<EmployeeDisplayViewModel> results = new List<EmployeeDisplayViewModel>();

                var result = _employeeService.GetDisplayListExceptSelf(companyId, employeeId);

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
        public IHttpActionResult PostEmployee(EmployeeListViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.Identity.GetUserId();            

            var result = _employeeService.Create(model, userId);

            if (result.Status == OperationResult.Success)
                return Ok(result.Data);
            else
                return BadRequest(result.Message);            
        }

        [Route("{id}")]
        [HttpPut]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult PutEmployee(EmployeeListViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _employeeService.Update(model);

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
        public IHttpActionResult DeleteEmployee(string id)
        {
            var result = _employeeService.Delete(id);

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
