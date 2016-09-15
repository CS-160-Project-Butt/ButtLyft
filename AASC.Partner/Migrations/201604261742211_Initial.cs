namespace AASC.Partner.API.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(nullable: false, maxLength: 100,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                {
                                    "Idx_Name",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { Name: Idx_Name, IsUnique: True }")
                                },
                            }),
                    CreatedDate = c.DateTime(nullable: false),
                    CreatedById = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .Index(t => t.Name, unique: true)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    FirstName = c.String(nullable: false, maxLength: 100),
                    LastName = c.String(nullable: false, maxLength: 100),
                    IsActive = c.Boolean(nullable: false),
                    RegisterDate = c.DateTime(nullable: false),
                    Email = c.String(),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(),
                    Partner_Id = c.Guid(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Partners", t => t.Partner_Id)
                .Index(t => t.Partner_Id);

            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                    ApplicationUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);

            CreateTable(
                "dbo.FileUploads",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    FileContent = c.Binary(nullable: false),
                    MimeType = c.String(nullable: false, maxLength: 100),
                    FileFolder = c.String(nullable: false, maxLength: 100),
                    FileName = c.String(nullable: false, maxLength: 100),
                    IsPublished = c.Boolean(nullable: false),
                    Note = c.String(),
                    CreatedDate = c.DateTime(nullable: false),
                    CreatedById = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    LoginProvider = c.String(),
                    ProviderKey = c.String(),
                    ApplicationUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);

            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                {
                    RoleId = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                    ApplicationUser_Id = c.String(maxLength: 128),
                    IdentityRole_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);

            CreateTable(
                "dbo.Departments",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(nullable: false, maxLength: 100),
                    CompanyId = c.Guid(nullable: false),
                    DepartmentHeadEmployeeId = c.Guid(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    CreatedById = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.Employees", t => t.DepartmentHeadEmployeeId)
                .Index(t => t.CompanyId)
                .Index(t => t.DepartmentHeadEmployeeId)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.Employees",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    JobTitle = c.String(nullable: false, maxLength: 100),
                    CompanyId = c.Guid(nullable: false),
                    ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    CreatedDate = c.DateTime(nullable: false),
                    CreatedById = c.String(nullable: false, maxLength: 128),
                    Department_Id = c.Guid(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .Index(t => t.CompanyId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.CreatedById)
                .Index(t => t.Department_Id);

            CreateTable(
                "dbo.Partners",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(nullable: false, maxLength: 100),
                    LandingPage = c.String(),
                    Logo = c.String(),
                    Theme = c.String(),
                    Roles = c.String(nullable: false, maxLength: 100),
                    Address = c.String(maxLength: 256),
                    City = c.String(maxLength: 100),
                    ZipCode = c.String(maxLength: 50),
                    Region = c.String(maxLength: 50),
                    Country = c.String(maxLength: 50),
                    IsActive = c.Boolean(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    CreatedById = c.String(nullable: false, maxLength: 128),
                    Company_Id = c.Guid(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.Companies", t => t.Company_Id)
                .Index(t => t.CreatedById)
                .Index(t => t.Company_Id);

            CreateTable(
                "dbo.EmployeeRoleInPartners",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Role = c.Int(nullable: false),
                    EmployeeId = c.Guid(nullable: false),
                    PartnerId = c.Guid(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    CreatedById = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .ForeignKey("dbo.Partners", t => t.PartnerId)
                .Index(t => t.EmployeeId)
                .Index(t => t.PartnerId)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.PartnerAgreements",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    TransactionType = c.Int(nullable: false),
                    Description = c.String(nullable: false, maxLength: 256),
                    StartDate = c.DateTime(nullable: false),
                    ExpireDate = c.DateTime(nullable: false),
                    InvoiceCycle = c.Int(nullable: false),
                    InvoiceDayOn = c.Int(nullable: false),
                    FileUploadId = c.Guid(),
                    PartnerId = c.Guid(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    CreatedById = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.FileUploads", t => t.FileUploadId)
                .ForeignKey("dbo.Partners", t => t.PartnerId)
                .Index(t => t.FileUploadId)
                .Index(t => t.PartnerId)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.PartnerGateways",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    TransactionType = c.Int(nullable: false),
                    Gateway = c.String(maxLength: 256),
                    GatewayUserId = c.String(maxLength: 100),
                    GatewayPassword = c.String(maxLength: 100),
                    Inbound = c.String(maxLength: 100),
                    Outbound = c.String(maxLength: 100),
                    TestGateway = c.String(maxLength: 256),
                    TestGatewayUserId = c.String(maxLength: 100),
                    TestGatewayPassword = c.String(maxLength: 100),
                    TestInbound = c.String(maxLength: 100),
                    TestOutbound = c.String(maxLength: 100),
                    PartnerId = c.Guid(nullable: false),
                    StartDate = c.DateTime(nullable: false),
                    ExpireDate = c.DateTime(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    CreatedById = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.Partners", t => t.PartnerId)
                .Index(t => t.PartnerId)
                .Index(t => t.CreatedById);

            CreateTable(
                "dbo.IoTGRoadmaps",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Model = c.String(nullable: false),
                    Status = c.String(),
                    Platform = c.String(nullable: false),
                    Trim = c.String(nullable: false),
                    Year = c.Int(nullable: false),
                    CodeName = c.String(nullable: false),
                    Level = c.String(nullable: false),
                    Category = c.String(nullable: false),
                    Subcategory = c.String(),
                    Link = c.String(),
                    FileUploadId = c.Guid(),
                    MarketSegment = c.String(),
                    AvailabilityESSample = c.String(),
                    AvailabilityMP = c.String(),
                    ContactEmployeeId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.ContactEmployeeId)
                .ForeignKey("dbo.FileUploads", t => t.FileUploadId)
                .Index(t => t.FileUploadId)
                .Index(t => t.ContactEmployeeId);

            CreateTable(
                "dbo.IdentityRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.IoTGRoadmaps", "FileUploadId", "dbo.FileUploads");
            DropForeignKey("dbo.IoTGRoadmaps", "ContactEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Partners", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.ApplicationUsers", "Partner_Id", "dbo.Partners");
            DropForeignKey("dbo.PartnerGateways", "PartnerId", "dbo.Partners");
            DropForeignKey("dbo.PartnerGateways", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.PartnerAgreements", "PartnerId", "dbo.Partners");
            DropForeignKey("dbo.PartnerAgreements", "FileUploadId", "dbo.FileUploads");
            DropForeignKey("dbo.PartnerAgreements", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.EmployeeRoleInPartners", "PartnerId", "dbo.Partners");
            DropForeignKey("dbo.EmployeeRoleInPartners", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeRoleInPartners", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Partners", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Employees", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.Departments", "DepartmentHeadEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Employees", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Departments", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Departments", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Companies", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.FileUploads", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.IoTGRoadmaps", new[] { "ContactEmployeeId" });
            DropIndex("dbo.IoTGRoadmaps", new[] { "FileUploadId" });
            DropIndex("dbo.PartnerGateways", new[] { "CreatedById" });
            DropIndex("dbo.PartnerGateways", new[] { "PartnerId" });
            DropIndex("dbo.PartnerAgreements", new[] { "CreatedById" });
            DropIndex("dbo.PartnerAgreements", new[] { "PartnerId" });
            DropIndex("dbo.PartnerAgreements", new[] { "FileUploadId" });
            DropIndex("dbo.EmployeeRoleInPartners", new[] { "CreatedById" });
            DropIndex("dbo.EmployeeRoleInPartners", new[] { "PartnerId" });
            DropIndex("dbo.EmployeeRoleInPartners", new[] { "EmployeeId" });
            DropIndex("dbo.Partners", new[] { "Company_Id" });
            DropIndex("dbo.Partners", new[] { "CreatedById" });
            DropIndex("dbo.Employees", new[] { "Department_Id" });
            DropIndex("dbo.Employees", new[] { "CreatedById" });
            DropIndex("dbo.Employees", new[] { "ApplicationUserId" });
            DropIndex("dbo.Employees", new[] { "CompanyId" });
            DropIndex("dbo.Departments", new[] { "CreatedById" });
            DropIndex("dbo.Departments", new[] { "DepartmentHeadEmployeeId" });
            DropIndex("dbo.Departments", new[] { "CompanyId" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.FileUploads", new[] { "CreatedById" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUsers", new[] { "Partner_Id" });
            DropIndex("dbo.Companies", new[] { "CreatedById" });
            DropIndex("dbo.Companies", new[] { "Name" });
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.IoTGRoadmaps");
            DropTable("dbo.PartnerGateways");
            DropTable("dbo.PartnerAgreements");
            DropTable("dbo.EmployeeRoleInPartners");
            DropTable("dbo.Partners");
            DropTable("dbo.Employees");
            DropTable("dbo.Departments");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.FileUploads");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.Companies",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "Name",
                        new Dictionary<string, object>
                        {
                            { "Idx_Name", "IndexAnnotation: { Name: Idx_Name, IsUnique: True }" },
                        }
                    },
                });
        }
    }
}
