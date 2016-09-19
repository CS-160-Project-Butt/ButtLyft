namespace AASC.Partner.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CLAForm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CLAForms",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CompanyName = c.String(nullable: false),
                        TaxID = c.String(nullable: false),
                        SalesContact = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        PostCode = c.String(nullable: false),
                        SignerFirstName = c.String(nullable: false),
                        SignerLastName = c.String(nullable: false),
                        SignerEmail = c.String(nullable: false),
                        SignerJobTitle = c.String(nullable: false),
                        SignerPhoneNumber = c.String(nullable: false),
                        TechnicalFirstName = c.String(nullable: false),
                        TechnicalastName = c.String(nullable: false),
                        TechnicalEmail = c.String(nullable: false),
                        TechnicalJobTitle = c.String(nullable: false),
                        TechnicalPhoneNumber = c.String(nullable: false),
                        DeviceCategories = c.String(nullable: false),
                        ProductList = c.String(nullable: false),
                        OtherType = c.String(),
                        OtherQuantity = c.String(),
                        CLANumber = c.String(),
                        CLAStatus = c.String(nullable: false),
                        CLAStatusDate = c.String(),
                        CustomerERPID = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CLAForms");
        }
    }
}
