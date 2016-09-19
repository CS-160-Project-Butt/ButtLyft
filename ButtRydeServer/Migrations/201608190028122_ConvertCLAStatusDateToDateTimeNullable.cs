namespace AASC.Partner.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConvertCLAStatusDateToDateTimeNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CLAForms", "CLAStatusDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CLAForms", "CLAStatusDate", c => c.String());
        }
    }
}
