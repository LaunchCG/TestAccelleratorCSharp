using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServices.Tests.DemoRestAPIs.Models
{
    public partial class ObjGetUsers
    {
        public long Page { get; set; }
        [JsonProperty(PropertyName = "per_page")]
        public long PerPage { get; set; }
        public long Total { get; set; }
        [JsonProperty(PropertyName = "total_pages")]
        public long TotalPages { get; set; }
        public Datum[] Data { get; set; }
        public Support Support { get; set; }
    }

    public partial class Datum
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
        public Uri Url { get; set; }
        public string Text { get; set; }
    }
}
