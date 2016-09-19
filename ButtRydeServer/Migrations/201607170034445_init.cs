namespace AASC.Partner.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.CLAForms SET OtherQuantity = '0' WHERE OtherQuantity IS NULL");
            Sql("UPDATE dbo.CLAForms SET DeviceCategories = REPLACE(DeviceCategories,'\"false\"','false')");
            Sql("UPDATE dbo.CLAForms SET DeviceCategories = REPLACE(DeviceCategories,'\"true\"','true')");
            Sql("UPDATE dbo.CLAForms SET DeviceCategories = REPLACE(DeviceCategories,'\"Device\"','\"device\"')");
            Sql("UPDATE dbo.CLAForms SET DeviceCategories = REPLACE(DeviceCategories,'\"Selected\"','\"selected\"')");
            Sql("UPDATE dbo.CLAForms SET ProductList = REPLACE(ProductList,'\"ProductType\"','\"productType\"')");
            Sql("UPDATE dbo.CLAForms SET ProductList = REPLACE(ProductList,'\"Product\"','\"product\"')");
            Sql("UPDATE dbo.CLAForms SET ProductList = REPLACE(ProductList,'\"Quantity\"','\"quantity\"')");
            AlterColumn("dbo.CLAForms", "OtherQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CLAForms", "OtherQuantity", c => c.String());
        }
    }
}
