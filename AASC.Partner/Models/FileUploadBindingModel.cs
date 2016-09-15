using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public class FileUploadBindingModel
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }
        //public Guid Id { get; set; }

        //[Required]
        //[Display(Name = "File Content")]
        //public byte[] FileContent { get; set; }

        [Required]
        [Display(Name = "Mime Type")]
        [MaxLength(100)]
        public string MimeType { get; set; }

        [Required]
        [Display(Name = "File Folder")]
        [MaxLength(100)]
        public string FileFolder { get; set; }

        [Required]
        [Display(Name = "File Name")]
        [MaxLength(100)]
        public string FileName { get; set; }

        [Required]
        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; }

        [Display(Name = "File Note")]
        public string Note { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Created By Id")]
        public string CreatedById { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public DisplayUserBindingModel CreatedBy { get; set; }
    }
}