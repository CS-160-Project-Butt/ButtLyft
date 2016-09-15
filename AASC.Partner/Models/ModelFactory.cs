using LinqKit;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace AASC.Partner.API.Models
{
    public class ModelFactory
    {
        private UrlHelper _urlHelper;
        private ApplicationUserManager _appUserManager;

        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _urlHelper = new UrlHelper(request);
            _appUserManager = appUserManager;
        }

        public UserReturnModel Create(ApplicationUser appUser)
        {
            return new UserReturnModel
            {
                Url = _urlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                FullName = string.Format("{0} {1}", appUser.FirstName, appUser.LastName),
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                RegisterDate = appUser.RegisterDate,
                Roles = _appUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _appUserManager.GetClaimsAsync(appUser.Id).Result
            };
        }

        public RoleReturnModel Create(IdentityRole appRole)
        {
            return new RoleReturnModel
            {
                Url = _urlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };
        }

        public FileUploadBindingModel Create(FileUpload fileUpload)
        {
            return new FileUploadBindingModel
            {
                Id = fileUpload.Id,
                FileFolder = fileUpload.FileFolder,
                //FileContent = fileUpload.FileContent,
                FileName = fileUpload.FileName,
                MimeType = fileUpload.MimeType,
                Note = fileUpload.Note,
                IsPublished = fileUpload.IsPublished,
                CreatedById = fileUpload.CreatedById,
                CreatedBy = new DisplayUserBindingModel
                {
                    //Id = Guid.Parse(fileUpload.CreatedBy.Id),
                    Id = fileUpload.CreatedBy.Id,
                    UserName = fileUpload.CreatedBy.UserName,
                    Email = fileUpload.CreatedBy.Email,
                    FirstName = fileUpload.CreatedBy.FirstName,
                    LastName = fileUpload.CreatedBy.LastName
                },
                CreatedDate = fileUpload.CreatedDate
            };
        }

        public DisplayUserBindingModel CreateDisplayUserBindingModel(ApplicationUser user)
        {
            return new DisplayUserBindingModel
            {
                //Id = Guid.Parse(user.Id),
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public CompanyViewModel Create(Company company)
        {
            return new CompanyViewModel
            {
                Id = company.Id,
                Name = company.Name,
                CreatedById = company.CreatedById,
                CreatedBy = CreateDisplayUserBindingModel(company.CreatedBy),
                CreatedDate = company.CreatedDate,
            };
        }

        public CompanyDisplayViewModel CreateDisplay(Company company)
        {
            return new CompanyDisplayViewModel
            {
                Id = company.Id,
                Name = company.Name,                
            };
        }

        public DepartmentViewModel Create(Department department)
        {
            var childrenDepartments = new List<DepartmentViewModel>();
            foreach(var d in department.ChildrenDepartments)
            {
                childrenDepartments.Add(Create(d));
            }
            department.ChildrenDepartments.ForEach(x => Create(x));
            return new DepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
                CompanyId = department.CompanyId,
                Company = Create(department.Company),
                ParentDepartmentId = department.ParentDepartmentId,
                ParentDepartment = new DepartmentDisplayViewModel
                {
                    Id = department.ParentDepartmentId,
                    Name = department.ParentDepartment.Name
                },
                DepartmentHeadEmployeeId = department.DepartmentHeadEmployeeId,
                DepartmentHead = Create(department.DepartmentHead),
                CreatedBy = CreateDisplayUserBindingModel(department.CreatedBy),
                CreatedDate = department.CreatedDate,
                CreatedById = department.CreatedById,
                ChildrenDepartments = childrenDepartments
            };
        }

        public EmployeeViewModel Create(Employee employee)
        {
            return new EmployeeViewModel
            {
                Id = employee.Id,
                JobTitle = employee.JobTitle,
                CompanyId = employee.CompanyId,
                Company = CreateDisplay(employee.Company),
                CreatedDate = employee.CreatedDate,
                CreatedBy = CreateDisplayUserBindingModel(employee.CreatedBy),
                CreatedById = employee.CreatedById,
                ApplicationUserId = employee.ApplicationUserId,
                ApplicationUser = CreateDisplayUserBindingModel(employee.ApplicationUser)
            };
        }

        public EmployeeDisplayViewModel Create(EmployeeViewModel employee)
        {
            return new EmployeeDisplayViewModel
            {
                Id = employee.Id,
                JobTitle = employee.JobTitle,
                UserName = employee.ApplicationUser.UserName,
                ApplicationUserId = employee.ApplicationUserId,
                Email = employee.ApplicationUser.Email,
                FirstName = employee.ApplicationUser.FirstName,
                LastName = employee.ApplicationUser.LastName
            };
        }
    }

    public class UserReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Level { get; set; }
        public DateTime RegisterDate { get; set; }
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }

    }

    public class RoleReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}