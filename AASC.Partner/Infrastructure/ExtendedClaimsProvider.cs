using AASC.Partner.API.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace AASC.Partner.API.Infrastructure
{
    public class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>();

            /*
            var daysInWork = (DateTime.Now.Date - user.RegisterDate).TotalDays;

            if (daysInWork > 90)
            {
                claims.Add(CreateClaim("FTE", "1"));
            }
            else
            {
                claims.Add(CreateClaim("FTE", "0"));
            }
            */
            if (user.IsActive)
                claims.Add(CreateClaim("Active", "1"));
            else
                claims.Add(CreateClaim("Active", "0"));
            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }
    }
}