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

            /*if (GenericClass.dicDict["TestCaseName"].Equals("TCPWidenAPI_GetAll_MediaAssets_Based_on_ComponentCode"))
            {
                objParam.TCPWidenAPIGetAll(APIAddParamUtility.componentCode, restRequest);
            }

            if (GenericClass.dicDict["TestCaseName"].Equals("FeatureImage_GET_Image_URL"))
            {
                objParam.APIFeatureImageGet(APIAddParamUtility.remoteSourceCode, APIAddParamUtility.featureCode, restRequest);
            }*/


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
            //AddAdditionalHeaders();
            /*if (GenericClass.dicDict["TestCaseName"].Equals("RSSQLRestService_PUT_UpdateUser"))
            {
                objParam.AddParamsuserid(APIAddParamUtility.userID, restRequest);
            }*/
            restRequest.AddJsonBody(jBody);           
            restRequest.RequestFormat = DataFormat.Json;
            return restRequest;

           
        }

        public string GenerateDynamicTokenRSSQLRest()
        {
            string UserName = "AccutracIntegrationUser";
            string Password = "QASecret";
            string jsonData = "{\"userName\": \"" + UserName + "\",\"password\": \"" + Password + "\"}";

            restClient = new RestClient();
            restRequest = new RestRequest("url");
            //restClient.Post(restRequest);

            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddJsonBody(jsonData);
            IRestResponse restResp = restClient.Post(restRequest);

            if (restResp.IsSuccessful)
            {
                Console.WriteLine("Status code: " + restResp.StatusCode);
                Console.WriteLine("Response Content: " + restResp.Content);
            }

            return restResp.Content;
        }

        public string ImageUploadUrl()
        {
            restClient = new RestClient();
            string url = "https://localhost:44369/WidenAsset/TestUploadAssetFromUrl";
            url = url + "?" + "url=http://images.triseptsolutions.com/Hotels/HbsImg/42992/slideshow/42992_m01.jpg";
            restRequest = new RestRequest(url);
            //restClient.Post(restRequest);
            //restRequest.AddHeader("Authorization", "bearer trisept/06f048232db05b17fc8dd994f1eefae0");
            //restRequest.AddHeader("Content-Type", "multipart/form-data");
            //restRequest.AddHeader("Content-Type", "image/png");
            //restRequest.AddHeader("Accept", "application/json");
            //restRequest.AddParameter("url", @"http://images.triseptsolutions.com/Hotels/HbsImg/42992/slideshow/42992_m01.jpg");
            //restRequest.AddParameter("url", "http%3A%2F%2Fimages.triseptsolutions.com%2FHotels%2FHbsImg%2F42992%2Fslideshow%2F42992_m01.jpg");
            restRequest.AddParameter("filename", "");
            //restRequest.AddJsonBody(jsonData);
            //restRequest.RequestFormat = DataFormat.Json;
            IRestResponse restResp = restClient.Post(restRequest);

            if (restResp.IsSuccessful)
            {
                Console.WriteLine("Status code: " + restResp.StatusCode);
                Console.WriteLine("Response Content: " + restResp.Content);
            }
            else
            {
                Console.WriteLine("Error exception: " + restResp.Content);
                Console.WriteLine("Error Message: " + restResp.ErrorMessage);
                Console.WriteLine("Error exception: " + restResp.ErrorException);
            }

            return restResp.Content;
        }
    }
}
