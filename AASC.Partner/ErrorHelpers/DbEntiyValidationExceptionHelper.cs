using System.Data.Entity.Validation;

namespace AASC.Partner.API.ErrorHelpers
{
    public class DbEntiyValidationExceptionHelper
    {
        public static string RetrieveMessage(DbEntityValidationException ex)
        {
            string errorMessage = "";
            foreach (var error in ex.EntityValidationErrors)
            {
                foreach (var err in error.ValidationErrors)
                {
                    errorMessage += string.Format("{0} - {1}", err.PropertyName, err.ErrorMessage);
                }
            }
            return errorMessage;
        }
    }
}