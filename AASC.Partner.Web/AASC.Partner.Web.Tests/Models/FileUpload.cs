using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.Partner.Web.Tests.Models
{
    public class FileUpload
    {
        public string Id { get; set; }

        public byte[] FileContent { get; set; }
        
        public string MimeType { get; set; }

        public string FileFolder { get; set; }

        public string FileName { get; set; }

        public bool IsPublished { get; set; }

        public string Note { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }
    }
}
