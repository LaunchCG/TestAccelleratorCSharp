using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  WebServices.Tests
{
    class APIAddParameters
    {
        public RestRequest APIGetUsersTest(string pageNumber, RestRequest restRequest)
        {
            restRequest.AddParameter("searchString", pageNumber);
            return restRequest;
        }
    }
}
