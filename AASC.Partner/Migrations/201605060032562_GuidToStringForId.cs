namespace AASC.Partner.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GuidToStringForId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Departments", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Partners", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.PartnerAgreements", "FileUploadId", "dbo.FileUploads");
            DropForeignKey("dbo.IoTGRoadmaps", "FileUploadId", "dbo.FileUploads");
            DropForeignKey("dbo.Departments", "ParentDepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Employees", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.Departments", "DepartmentHeadEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeRoleInPartners", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.IoTGRoadmaps", "ContactEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeRoleInPartners", "PartnerId", "dbo.Partners");
            DropForeignKey("dbo.PartnerAgreements", "PartnerId", "dbo.Partners");
            DropForeignKey("dbo.PartnerGateways", "PartnerId", "dbo.Partners");
            DropForeignKey("dbo.ApplicationUsers", "Partner_Id", "dbo.Partners");
            DropIndex("dbo.ApplicationUsers", new[] { "Partner_Id" });
            DropIndex("dbo.Departments", new[] { "CompanyId" });
            DropIndex("dbo.Departments", new[] { "ParentDepartmentId" });
            DropIndex("dbo.Departments", new[] { "DepartmentHeadEmployeeId" });
            DropIndex("dbo.Employees", new[] { "CompanyId" });
            DropIndex("dbo.Employees", new[] { "Department_Id" });
            DropIndex("dbo.Partners", new[] { "Company_Id" });
            DropIndex("dbo.EmployeeRoleInPartners", new[] { "EmployeeId" });
            DropIndex("dbo.EmployeeRoleInPartners", new[] { "PartnerId" });
            DropIndex("dbo.PartnerAgreements", new[] { "FileUploadId" });
            DropIndex("dbo.PartnerAgreements", new[] { "PartnerId" });
            DropIndex("dbo.PartnerGateways", new[] { "PartnerId" });
            DropIndex("dbo.IoTGRoadmaps", new[] { "FileUploadId" });
            DropIndex("dbo.IoTGRoadmaps", new[] { "ContactEmployeeId" });
            DropPrimaryKey("dbo.Companies");
            DropPrimaryKey("dbo.FileUploads");
            DropPrimaryKey("dbo.Departments");
            DropPrimaryKey("dbo.Employees");
            DropPrimaryKey("dbo.Partners");
            DropPrimaryKey("dbo.EmployeeRoleInPartners");
            DropPrimaryKey("dbo.PartnerAgreements");
            DropPrimaryKey("dbo.PartnerGateways");
            DropPrimaryKey("dbo.IoTGRoadmaps");
            AlterColumn("dbo.Companies", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ApplicationUsers", "Partner_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.FileUploads", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Departments", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Departments", "CompanyId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Departments", "ParentDepartmentId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Departments", "DepartmentHeadEmployeeId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Employees", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Employees", "CompanyId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Employees", "Department_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Partners", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Partners", "Company_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.EmployeeRoleInPartners", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.EmployeeRoleInPartners", "EmployeeId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.EmployeeRoleInPartners", "PartnerId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.PartnerAgreements", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.PartnerAgreements", "FileUploadId", c => c.String(maxLength: 128));
            AlterColumn("dbo.PartnerAgreements", "PartnerId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.PartnerGateways", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.PartnerGateways", "PartnerId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.IoTGRoadmaps", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.IoTGRoadmaps", "FileUploadId", c => c.String(maxLength: 128));
            AlterColumn("dbo.IoTGRoadmaps", "ContactEmployeeId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Companies", "Id");
            AddPrimaryKey("dbo.FileUploads", "Id");
            AddPrimaryKey("dbo.Departments", "Id");
            AddPrimaryKey("dbo.Employees", "Id");
            AddPrimaryKey("dbo.Partners", "Id");
            AddPrimaryKey("dbo.EmployeeRoleInPartners", "Id");
            AddPrimaryKey("dbo.PartnerAgreements", "Id");
            AddPrimaryKey("dbo.PartnerGateways", "Id");
            AddPrimaryKey("dbo.IoTGRoadmaps", "Id");
            CreateIndex("dbo.ApplicationUsers", "Partner_Id");
            CreateIndex("dbo.Departments", "CompanyId");
            CreateIndex("dbo.Departments", "ParentDepartmentId");
            CreateIndex("dbo.Departments", "DepartmentHeadEmployeeId");
            CreateIndex("dbo.Employees", "CompanyId");
            CreateIndex("dbo.Employees", "Department_Id");
            CreateIndex("dbo.Partners", "Company_Id");
            CreateIndex("dbo.EmployeeRoleInPartners", "EmployeeId");
            CreateIndex("dbo.EmployeeRoleInPartners", "PartnerId");
            CreateIndex("dbo.PartnerAgreements", "FileUploadId");
            CreateIndex("dbo.PartnerAgreements", "PartnerId");
            CreateIndex("dbo.PartnerGateways", "PartnerId");
            CreateIndex("dbo.IoTGRoadmaps", "FileUploadId");
            CreateIndex("dbo.IoTGRoadmaps", "ContactEmployeeId");
            AddForeignKey("dbo.Departments", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.Employees", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.Partners", "Company_Id", "dbo.Companies", "Id");
            AddForeignKey("dbo.PartnerAgreements", "FileUploadId", "dbo.FileUploads", "Id");
            AddForeignKey("dbo.IoTGRoadmaps", "FileUploadId", "dbo.FileUploads", "Id");
            AddForeignKey("dbo.Departments", "ParentDepartmentId", "dbo.Departments", "Id");
            AddForeignKey("dbo.Employees", "Department_Id", "dbo.Departments", "Id");
            AddForeignKey("dbo.Departments", "DepartmentHeadEmployeeId", "dbo.Employees", "Id");
            AddForeignKey("dbo.EmployeeRoleInPartners", "EmployeeId", "dbo.Employees", "Id");
            AddForeignKey("dbo.IoTGRoadmaps", "ContactEmployeeId", "dbo.Employees", "Id");
            AddForeignKey("dbo.EmployeeRoleInPartners", "PartnerId", "dbo.Partners", "Id");
            AddForeignKey("dbo.PartnerAgreements", "PartnerId", "dbo.Partners", "Id");
            AddForeignKey("dbo.PartnerGateways", "PartnerId", "dbo.Partners", "Id");
            AddForeignKey("dbo.ApplicationUsers", "Partner_Id", "dbo.Partners", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUsers", "Partner_Id", "dbo.Partners");
            DropForeignKey("dbo.PartnerGateways", "PartnerId", "dbo.Partners");
            DropForeignKey("dbo.PartnerAgreements", "PartnerId", "dbo.Partners");
            DropForeignKey("dbo.EmployeeRoleInPartners", "PartnerId", "dbo.Partners");
            DropForeignKey("dbo.IoTGRoadmaps", "ContactEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeRoleInPartners", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Departments", "DepartmentHeadEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.Departments", "ParentDepartmentId", "dbo.Departments");
            DropForeignKey("dbo.IoTGRoadmaps", "FileUploadId", "dbo.FileUploads");
            DropForeignKey("dbo.PartnerAgreements", "FileUploadId", "dbo.FileUploads");
            DropForeignKey("dbo.Partners", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Departments", "CompanyId", "dbo.Companies");
            DropIndex("dbo.IoTGRoadmaps", new[] { "ContactEmployeeId" });
            DropIndex("dbo.IoTGRoadmaps", new[] { "FileUploadId" });
            DropIndex("dbo.PartnerGateways", new[] { "PartnerId" });
            DropIndex("dbo.PartnerAgreements", new[] { "PartnerId" });
            DropIndex("dbo.PartnerAgreements", new[] { "FileUploadId" });
            DropIndex("dbo.EmployeeRoleInPartners", new[] { "PartnerId" });
            DropIndex("dbo.EmployeeRoleInPartners", new[] { "EmployeeId" });
            DropIndex("dbo.Partners", new[] { "Company_Id" });
            DropIndex("dbo.Employees", new[] { "Department_Id" });
            DropIndex("dbo.Employees", new[] { "CompanyId" });
            DropIndex("dbo.Departments", new[] { "DepartmentHeadEmployeeId" });
            DropIndex("dbo.Departments", new[] { "ParentDepartmentId" });
            DropIndex("dbo.Departments", new[] { "CompanyId" });
            DropIndex("dbo.ApplicationUsers", new[] { "Partner_Id" });
            DropPrimaryKey("dbo.IoTGRoadmaps");
            DropPrimaryKey("dbo.PartnerGateways");
            DropPrimaryKey("dbo.PartnerAgreements");
            DropPrimaryKey("dbo.EmployeeRoleInPartners");
            DropPrimaryKey("dbo.Partners");
            DropPrimaryKey("dbo.Employees");
            DropPrimaryKey("dbo.Departments");
            DropPrimaryKey("dbo.FileUploads");
            DropPrimaryKey("dbo.Companies");
            AlterColumn("dbo.IoTGRoadmaps", "ContactEmployeeId", c => c.Guid(nullable: false));
            AlterColumn("dbo.IoTGRoadmaps", "FileUploadId", c => c.Guid());
            AlterColumn("dbo.IoTGRoadmaps", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.PartnerGateways", "PartnerId", c => c.Guid(nullable: false));
            AlterColumn("dbo.PartnerGateways", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.PartnerAgreements", "PartnerId", c => c.Guid(nullable: false));
            AlterColumn("dbo.PartnerAgreements", "FileUploadId", c => c.Guid());
            AlterColumn("dbo.PartnerAgreements", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.EmployeeRoleInPartners", "PartnerId", c => c.Guid(nullable: false));
            AlterColumn("dbo.EmployeeRoleInPartners", "EmployeeId", c => c.Guid(nullable: false));
            AlterColumn("dbo.EmployeeRoleInPartners", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Partners", "Company_Id", c => c.Guid());
            AlterColumn("dbo.Partners", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Employees", "Department_Id", c => c.Guid());
            AlterColumn("dbo.Employees", "CompanyId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Employees", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Departments", "DepartmentHeadEmployeeId", c => c.Guid());
            AlterColumn("dbo.Departments", "ParentDepartmentId", c => c.Guid());
            AlterColumn("dbo.Departments", "CompanyId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Departments", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.FileUploads", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.ApplicationUsers", "Partner_Id", c => c.Guid());
            AlterColumn("dbo.Companies", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.IoTGRoadmaps", "Id");
            AddPrimaryKey("dbo.PartnerGateways", "Id");
            AddPrimaryKey("dbo.PartnerAgreements", "Id");
            AddPrimaryKey("dbo.EmployeeRoleInPartners", "Id");
            AddPrimaryKey("dbo.Partners", "Id");
            AddPrimaryKey("dbo.Employees", "Id");
            AddPrimaryKey("dbo.Departments", "Id");
            AddPrimaryKey("dbo.FileUploads", "Id");
            AddPrimaryKey("dbo.Companies", "Id");
            CreateIndex("dbo.IoTGRoadmaps", "ContactEmployeeId");
            CreateIndex("dbo.IoTGRoadmaps", "FileUploadId");
            CreateIndex("dbo.PartnerGateways", "PartnerId");
            CreateIndex("dbo.PartnerAgreements", "PartnerId");
            CreateIndex("dbo.PartnerAgreements", "FileUploadId");
            CreateIndex("dbo.EmployeeRoleInPartners", "PartnerId");
            CreateIndex("dbo.EmployeeRoleInPartners", "EmployeeId");
            CreateIndex("dbo.Partners", "Company_Id");
            CreateIndex("dbo.Employees", "Department_Id");
            CreateIndex("dbo.Employees", "CompanyId");
            CreateIndex("dbo.Departments", "DepartmentHeadEmployeeId");
            CreateIndex("dbo.Departments", "ParentDepartmentId");
            CreateIndex("dbo.Departments", "CompanyId");
            CreateIndex("dbo.ApplicationUsers", "Partner_Id");
            AddForeignKey("dbo.ApplicationUsers", "Partner_Id", "dbo.Partners", "Id");
            AddForeignKey("dbo.PartnerGateways", "PartnerId", "dbo.Partners", "Id");
            AddForeignKey("dbo.PartnerAgreements", "PartnerId", "dbo.Partners", "Id");
            AddForeignKey("dbo.EmployeeRoleInPartners", "PartnerId", "dbo.Partners", "Id");
            AddForeignKey("dbo.IoTGRoadmaps", "ContactEmployeeId", "dbo.Employees", "Id");
            AddForeignKey("dbo.EmployeeRoleInPartners", "EmployeeId", "dbo.Employees", "Id");
            AddForeignKey("dbo.Departments", "DepartmentHeadEmployeeId", "dbo.Employees", "Id");
            AddForeignKey("dbo.Employees", "Department_Id", "dbo.Departments", "Id");
            AddForeignKey("dbo.Departments", "ParentDepartmentId", "dbo.Departments", "Id");
            AddForeignKey("dbo.IoTGRoadmaps", "FileUploadId", "dbo.FileUploads", "Id");
            AddForeignKey("dbo.PartnerAgreements", "FileUploadId", "dbo.FileUploads", "Id");
            AddForeignKey("dbo.Partners", "Company_Id", "dbo.Companies", "Id");
            AddForeignKey("dbo.Employees", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.Departments", "CompanyId", "dbo.Companies", "Id");
        }
    }
}
