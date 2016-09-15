using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AASC.Partner.API.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            var userName = this.RequestContext.Principal.Identity.Name;
            return new string[] { "value1", userName };
        }

        // GET api/values/5
        public string Get(int id)
        {
            var userName = this.RequestContext.Principal.Identity.Name;
            return string.Format("Hello, {0}", userName);
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
