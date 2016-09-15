namespace AASC.Partner.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addParentForeignKeytoDepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "ParentDepartmentId", c => c.Guid());
            CreateIndex("dbo.Departments", "ParentDepartmentId");
            AddForeignKey("dbo.Departments", "ParentDepartmentId", "dbo.Departments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Departments", "ParentDepartmentId", "dbo.Departments");
            DropIndex("dbo.Departments", new[] { "ParentDepartmentId" });
            DropColumn("dbo.Departments", "ParentDepartmentId");
        }
    }
}
