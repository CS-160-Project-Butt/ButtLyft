namespace AASC.Partner.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveFrom_DeactiveFrom_for_Employee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "ActiveFrom", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employees", "DeactiveFrom", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "DeactiveFrom");
            DropColumn("dbo.Employees", "ActiveFrom");
        }
    }
}
