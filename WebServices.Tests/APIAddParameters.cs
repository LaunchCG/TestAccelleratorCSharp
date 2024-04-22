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

        public RestRequest APIWidenConcurrentUsersTest(string usersCount, string imagesFetchLimit, RestRequest restRequest)
        {
            restRequest.AddParameter("count", usersCount);
            restRequest.AddParameter("fetchLimit", imagesFetchLimit);
            return restRequest;
        }

        public RestRequest TCPWidenAPIGetAll(string compCode, RestRequest restRequest)
        {
            restRequest.AddParameter("componentCode", compCode);
            return restRequest;
        }

        public RestRequest TCPWidenAPIGetByMediaAssetID(string medAssetId, RestRequest restRequest)
        {
            restRequest.AddParameter("id", medAssetId);
            return restRequest;
        }

        public RestRequest APIFeatureImageGet(string remSrcCode, string featCode, RestRequest restRequest)
        {
            restRequest.AddParameter("remoteSourceCode", remSrcCode);
            restRequest.AddParameter("featureCode", featCode);
            return restRequest;
        }

    }
}
