using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServices.Tests.DemoRestAPIs.Models
{
    public partial class ObjUpdateUser
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
