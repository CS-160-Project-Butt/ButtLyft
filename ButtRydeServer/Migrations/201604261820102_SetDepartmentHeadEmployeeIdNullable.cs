namespace AASC.Partner.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SetDepartmentHeadEmployeeIdNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Departments", new[] { "DepartmentHeadEmployeeId" });
            AlterColumn("dbo.Departments", "DepartmentHeadEmployeeId", c => c.Guid());
            CreateIndex("dbo.Departments", "DepartmentHeadEmployeeId");
        }

        public override void Down()
        {
            DropIndex("dbo.Departments", new[] { "DepartmentHeadEmployeeId" });
            AlterColumn("dbo.Departments", "DepartmentHeadEmployeeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Departments", "DepartmentHeadEmployeeId");
        }
    }
}
