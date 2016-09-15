using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.ErrorHelpers
{
    public class InnerExceptionHandler
    {
        public static string Retrieve(Exception innerException)
        {
            string message = "";
            if (innerException.InnerException != null)
                message += Retrieve(innerException.InnerException);
            else
                message += innerException.Message;
            return message;
        }
    }
}