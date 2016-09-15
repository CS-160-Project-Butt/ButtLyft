using AASC.Partner.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace AASC.Partner.API.Controllers
{
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {
        [Authorize]
        [Route("{id:guid}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(string id)
        {
            var role = await this.AppRoleManager.FindByIdAsync(id);

            if (role != null)
                return Ok(TheModelFactory.Create(role));

            return NotFound();
        }

        [Authorize]
        [Route("", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = this.AppRoleManager.Roles;

            return Ok(roles);
        }

        [Authorize(Roles = "Admin")]
        [Route("create")]
        public async Task<IHttpActionResult> Create(RoleBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = new IdentityRole { Name = model.Name };

            var result = await this.AppRoleManager.CreateAsync(role);

            if (!result.Succeeded)
                return GetErrorResult(result);

            Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));

            return Created(locationHeader, TheModelFactory.Create(role));
        }

        [Authorize(Roles = "Admin")]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(string id)
        {
            var role = await this.AppRoleManager.FindByIdAsync(id);

            if (role != null)
            {
                IdentityResult result = await this.AppRoleManager.DeleteAsync(role);

                if (!result.Succeeded)
                    return GetErrorResult(result);

                return Ok();
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [Route("ManageUsersInRole")]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            var role = await this.AppRoleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (string user in model.EnrolledUsers)
            {
                var appUser = await this.AppUserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", string.Format("User: {0} does not exists", user));
                    continue;
                }

                if (!this.AppUserManager.IsInRole(user, role.Name))
                {
                    IdentityResult result = await this.AppUserManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", string.Format("User: {0} could not be added to role", user));
                    }
                }
            }

            foreach (string user in model.RemovedUsers)
            {
                var appUser = await this.AppUserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", string.Format("User: {0} does not exists", user));
                    continue;
                }

                IdentityResult result = await this.AppUserManager.RemoveFromRolesAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", string.Format("User: {0} could not be removed from role", user));
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }

        //[Authorize]
        [Route("GetUserRolesAsnyc")]
        public async Task<IHttpActionResult> GetUserRolesAsnyc()
        {
            string userId = User.Identity.GetUserId();

            if (!string.IsNullOrEmpty(userId))
            {
                var userRoles = await this.AppUserManager.GetRolesAsync(User.Identity.GetUserId());

                if (userRoles != null)
                    return Ok(userRoles);

            }

            return NotFound();
        }

        //[Authorize]
        [Route("GetUserRoles")]
        public IHttpActionResult GetUserRoles()
        {
            string userId = User.Identity.GetUserId();

            if (!string.IsNullOrEmpty(userId))
            {
                var userRoles = this.AppUserManager.GetRoles(userId);

                if (userRoles != null)
                    return Ok(userRoles);

            }
            return NotFound();
        }
    }
}
