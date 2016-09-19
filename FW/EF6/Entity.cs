using AASC.FW.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.FW.EF6
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}
