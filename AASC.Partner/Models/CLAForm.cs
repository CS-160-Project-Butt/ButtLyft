using AASC.FW.EF6;
using System;
using System.ComponentModel.DataAnnotations;

namespace AASC.Partner.API.Models
{

    public class CLAForm : Entity
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Customer Tax ID")]
        public string TaxID { get; set; }

        [Required]
        [Display(Name = "Sales Contact")]
        public string SalesContact { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Required]
        [Display(Name = "Contract Signer First Name")]
        public string SignerFirstName { get; set; }

        [Required]
        [Display(Name = "Contract Signer Last Name")]
        public string SignerLastName { get; set; }

        [Required]
        [Display(Name = "Contract Signer Email")]
        public string SignerEmail { get; set; }

        [Required]
        [Display(Name = "Contract Signer Job Title")]
        public string SignerJobTitle { get; set; }

        [Required]
        [Display(Name = "Contract Signer Phone Number")]
        public string SignerPhoneNumber { get; set; }

        [Required]
        [Display(Name = "Technical Contact First Name")]
        public string TechnicalFirstName { get; set; }

        [Required]
        [Display(Name = "Technical Contact Last Name")]
        public string TechnicalastName { get; set; }

        [Required]
        [Display(Name = "Technical Contact Email")]
        public string TechnicalEmail { get; set; }

        [Required]
        [Display(Name = "Technical Contact Job Title")]
        public string TechnicalJobTitle { get; set; }

        [Required]
        [Display(Name = "Technical Contact Phone Number")]
        public string TechnicalPhoneNumber { get; set; }

        [Required]
        [Display(Name = "Device Categories")]
        public string DeviceCategories { get; set; }

        [Required]
        [Display(Name = "Product List")]
        public string ProductList { get; set; }

        [Display(Name = "Other Type")]
        public string OtherType { get; set; }

        [Display(Name = "Other Quantity")]
        public int OtherQuantity { get; set; }

        [Display(Name = "CLA Number")]
        public string CLANumber { get; set; }

        [Required]
        [Display(Name = "CLA Status")]
        public string CLAStatus { get; set; }

        [Display(Name = "CLA Status Date")]
        public string CLAStatusDate { get; set; }

        [Display(Name = "Customer ERPID")]
        public string CustomerERPID { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

    }


}