using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServices.Tests.DemoRestAPIs.Models
{
    public partial class ObjGetUserById
    {
        public Data Data { get; set; }
        public Support Support { get; set; }
    }

    public partial class Data
    {
        public long Id { get; set; }
        public string Email { get; set; }
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
        public Uri Avatar { get; set; }
    }

    public partial class Support
    {
        public Uri Url1 { get; set; }
        public string Text1 { get; set; }
    }
}
