using  Common.TestFramework.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace  WebServices.Tests
{
    public class APIGenericClass<T>
    {
        public RestClient restClient;
        public RestRequest restRequest;
        APIAddParameters objParam = new APIAddParameters();      

        public RestRequest SetAuthorization(string bearerToken, RestRequest restRequest)
        {
            //RestRequest _resReq = new RestRequest();
            //return _resReq.AddHeader("Authorization", bearerToken);
            restRequest.AddHeader("Authorization", bearerToken);
            //var _resReq = new RestRequest();
            //_resReq.AddHeader("Authorization", bearerToken);
            return restRequest;
        }

        public RestClient SetUrl(string baseUrl, string resourceUrl)
        {
            var url = string.Concat(baseUrl, resourceUrl);
            //var url = Path.Combine(baseUrl, resourceUrl);
            var restClient = new RestClient(url);
            return restClient;
        }

        private RestRequest AddAdditionalHeaders(RestRequest restRequest)
        {
            //var _resReq = new RestRequest();
            restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
            restRequest.AddHeader("Accept", "application/json");
            return restRequest;
        }

        public RestRequest CreatePOSTRequest(string auth, string jBody)
        {            
            var restRequest = new RestRequest(Method.POST);
            //restRequest.AddHeader("Authorization", auth);
            SetAuthorization(auth,restRequest);
            //restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
            //restRequest.AddHeader("Accept", "application/json");
            AddAdditionalHeaders(restRequest);
            //AddAdditionalHeaders();
            restRequest.AddJsonBody(jBody);
            return restRequest;
        }

        public RestRequest CreateGETRequestWithoutAuth()
        {
            var restRequest = new RestRequest(Method.GET);
            AddAdditionalHeaders(restRequest);

            if (GenericClass.dicDict["TestCaseName"].Equals("GetUser_API_GetUsers"))
            {
                objParam.APIGetUsersTest(APIAddParamUtility.pageNum, restRequest);
            }

            restRequest.RequestFormat = DataFormat.Json;
            return restRequest;
        }

        public RestRequest CreatePOSTRequestWithoutAuth(string jBody)
        {
            var restRequest = new RestRequest(Method.POST);
            AddAdditionalHeaders(restRequest);
            restRequest.AddJsonBody(jBody);
            return restRequest;
        }

        public RestRequest CreatePATCHRequestWithoutAuth(string jBody)
        {
            var restRequest = new RestRequest(Method.PATCH);
            AddAdditionalHeaders(restRequest);
            restRequest.AddJsonBody(jBody);
            return restRequest;
        }

        public IRestResponse ExecuteAPIAndGetResponse(RestClient restClient, RestRequest restRequest)
        {
            return restClient.Execute(restRequest);
        }

        public DTO GetResponseContent<DTO>(IRestResponse restResponse)
        {
            var content = restResponse.Content;
            //DTO deserializeObject = JsonConvert.DeserializeObject<DTO>(content);
            DTO deserializeObject = JsonConvert.DeserializeObject<DTO>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return deserializeObject;
        }

        public List<DTO> GetResponseContentList<DTO>(IRestResponse restResponse)
        {
            var content = restResponse.Content;
            //List<DTO> deserializeObject = JsonConvert.DeserializeObject<List<DTO>>(content);
            List<DTO> deserializeObject = JsonConvert.DeserializeObject<List<DTO>>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return deserializeObject;
        }

        public string FormatJsonResponse(string JsonRespImproperFormat)
        {
            string JsonRespProperFormat =  JToken.Parse(JsonRespImproperFormat).ToString(Formatting.Indented);
            return JsonRespProperFormat;
        }

        public IRestResponse ExecuteAPIAndGetResponseWithoutToken(RestClient restClientToken)
        {
            return restClientToken.Execute(restRequest);
        }

        public RestRequest CreatePOSTRequestWithoutToken(string jBodywithoutToken)
        {
            var restRequestWT = new RestRequest("resource", Method.POST);
            AddAdditionalHeaders(restRequestWT);
            //AddAdditionalHeaders();
            restRequestWT.AddJsonBody(jBodywithoutToken);
            return restRequestWT;
        }

        public RestRequest CreatePUTRequest(string auth, string jBody)
        {
            var restRequest = new RestRequest("resource", Method.PUT);
            //restRequest.AddHeader("Authorization", auth);
            SetAuthorization(auth, restRequest);
            //restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
            //restRequest.AddHeader("Accept", "application/json");
            AddAdditionalHeaders(restRequest);  
            restRequest.AddJsonBody(jBody);           
            restRequest.RequestFormat = DataFormat.Json;
            return restRequest;

           
        }

    }
}
