using System;
using System.Collections.Generic;

namespace AASC.Partner.API.Models
{
    public class CLAFormViewModel
    {
        public string Id { get; set; }

        public string CompanyName { get; set; }

        public string TaxID { get; set; }

        public string SalesContact { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PostCode { get; set; }

        public string SignerFirstName { get; set; }

        public string SignerLastName { get; set; }

        public string SignerEmail { get; set; }

        public string SignerJobTitle { get; set; }

        public string SignerPhoneNumber { get; set; }

        public string TechnicalFirstName { get; set; }

        public string TechnicalLastName { get; set; }

        public string TechnicalEmail { get; set; }

        public string TechnicalJobTitle { get; set; }

        public string TechnicalPhoneNumber { get; set; }

        //public string DeviceCategories { get; set; }

        //public string ProductList { get; set; }

        public List<DeviceSelected> DeviceCategories { get; set; }

        public List<ProductChosen> ProductList { get; set; }

        public string OtherType { get; set; }

        public int OtherQuantity { get; set; }

        public string CLANumber { get; set; }

        public string CLAStatus { get; set; }

        public string CLAStatusDate { get; set; }

        public string CustomerERPID { get; set; }

        public DateTime CreatedDate { get; set; }

    }
    public class CLAFormListViewModel
    {
        public string Id { get; set; }

        public string CompanyName { get; set; }

        public string TaxID { get; set; }

        public string SalesContact { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PostCode { get; set; }

        public string SignerFirstName { get; set; }

        public string SignerLastName { get; set; }

        public string SignerEmail { get; set; }

        public string SignerJobTitle { get; set; }

        public string SignerPhoneNumber { get; set; }

        public string TechnicalFirstName { get; set; }

        public string TechnicalLastName { get; set; }

        public string TechnicalEmail { get; set; }

        public string TechnicalJobTitle { get; set; }

        public string TechnicalPhoneNumber { get; set; }

        public string DeviceCategories { get; set; }

        public string ProductList { get; set; }

        //public List<DeviceSelected> DeviceCategories { get; set; }

        //public List<ProductChosen> ProductList { get; set; }

        public string OtherType { get; set; }

        public int OtherQuantity { get; set; }

        public string CLANumber { get; set; }

        public string CLAStatus { get; set; }

        public string CLAStatusDate { get; set; }

        public string CustomerERPID { get; set; }

        public DateTime CreatedDate { get; set; }

    }

    public class DeviceSelected
    {
        public string Device { get; set; }
        public bool Selected { get; set; }
        public override string ToString()
        {
            return "{" + string.Format("\"device\": \"{0}\", \"selected\": {1}", Device, Selected.ToString().ToLower()) + "},";
        }

    }

    public class ProductChosen
    {
        public string ProductType { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public override string ToString()
        {
            return "{" + string.Format("\"productType\": \"{0}\", \"product\": \"{1}\", \"quantity\": {2}", ProductType, Product, Quantity) + "},";
        }
    }

    public class CFAFormDisplayViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

    }

}