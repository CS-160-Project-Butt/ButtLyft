using AASC.Partner.API.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Helpers
{
    public class Mapper
    {
        public static FileUploadBindingModel Create(FileUpload fileUpload)
        {
            return new FileUploadBindingModel
            {
                Id = fileUpload.Id,
                FileFolder = fileUpload.FileFolder,
                FileName = fileUpload.FileName,
                MimeType = fileUpload.MimeType,
                Note = fileUpload.Note,
                IsPublished = fileUpload.IsPublished,
                CreatedById = fileUpload.CreatedById,
                CreatedBy = new DisplayUserBindingModel
                {
                    Id = fileUpload.CreatedBy.Id,
                    UserName = fileUpload.CreatedBy.UserName,
                    Email = fileUpload.CreatedBy.Email,
                    FirstName = fileUpload.CreatedBy.FirstName,
                    LastName = fileUpload.CreatedBy.LastName
                },
                CreatedDate = fileUpload.CreatedDate
            };
        }

        public static DisplayUserBindingModel CreateDisplayUserBindingModel(ApplicationUser user)
        {
            return new DisplayUserBindingModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public static CompanyViewModel Create(Company company)
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

        public static CompanyDisplayViewModel CreateDisplay(Company company)
        {
            return new CompanyDisplayViewModel
            {
                Id = company.Id,
                Name = company.Name,
            };
        }

        public static CompanyDisplayViewModel ConvertTo(CompanyViewModel company)
        {
            return new CompanyDisplayViewModel
            {
                Id = company.Id,
                Name = company.Name,
            };
        }

        

        public static DepartmentViewModel Create(Department department)
        {
            var childrenDepartments = new List<DepartmentViewModel>();
            foreach (var d in department.ChildrenDepartments)
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

        public static EmployeeViewModel Create(Employee employee)
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

        public static EmployeeDisplayViewModel Create(EmployeeViewModel employee)
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
}