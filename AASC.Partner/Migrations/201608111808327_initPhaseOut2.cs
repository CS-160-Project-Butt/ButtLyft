namespace AASC.Partner.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initPhaseOut2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhaseOutPreps",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Phased = c.Boolean(nullable: false),
                        PartNumber = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        PlmStatus = c.String(nullable: false),
                        ProductFamily = c.String(nullable: false),
                        LastBuyTime = c.String(nullable: false),
                        Replacement = c.String(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PhaseOutPreps");
        }
    }
}
