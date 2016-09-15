using AASC.FW.DataMapper;
using AASC.Partner.API.ErrorHelpers;
using AASC.Partner.API.Infrastructure;
using AASC.Partner.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace AASC.Partner.API.Controllers
{
    [ClaimsAuthorization(ClaimType = "Active", ClaimValue = "1")]
    [RoutePrefix("api/applicationusers")]
    public class ApplicationUsersController : BaseApiController
    {
        [HttpGet]
        [Route("getavailableuserslist")]
        public IHttpActionResult GetAvailableUsersList()
        {
            try
            {
                List<DisplayUserBindingModel> results = new List<DisplayUserBindingModel>();

                var users = this.AppUserManager.Users.Where(x => x.IsActive).ToList();

                users.ForEach(x =>
                {
                    results.Add(DataMapper.Map<ApplicationUser, DisplayUserBindingModel>(x));
                });

                return Ok(new { data = results, total = results.Count() });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                List<ApplicationUserViewModel> results = new List<ApplicationUserViewModel>();

                var users = this.AppUserManager.Users.ToList();

                users.ForEach(x =>
                {
                    results.Add(DataMapper.Map<ApplicationUser, ApplicationUserViewModel>(x));
                });

                return Ok(new { data = results, total = results.Count() });
            }
            catch (Exception ex)
            {
                throw new ApiException { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = string.Format("Bad Request...{0}", ex.Message) };
            }
        }

        [Route("{id}")]
        [HttpPut]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult PutApplicationUser(ApplicationUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var taskUser = this.AppUserManager.FindByIdAsync(model.Id);

            if (taskUser.Result == null)
                return NotFound();

            var user = taskUser.Result;

            user.Email = model.Email;
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.IsActive = model.IsActive;

            var result = this.AppUserManager.UpdateAsync(user);

            if (result.Result.Succeeded)
                return Ok(DataMapper.Map<ApplicationUser, ApplicationUserViewModel>(user));
            else
                return BadRequest();
            
        }
    }
}
