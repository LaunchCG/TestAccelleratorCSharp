using  Common.TestFramework.Core;
using  UI.Tests.PageActions;
using  WebServices.Tests.DemoRestAPIs.Models;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Web;

namespace  WebServices.Tests.DemoRestAPIs.TestSuite
{
    [TestFixture]
    class APIRegressionTestSuite : ConfigClass
    {
        string reportStepDesc;
        //string reportStepVerify;
        RestClient restClient = new RestClient();
        RestRequest restRequest = new RestRequest();
        ActionClass actionClass = new ActionClass();
        //APIGenericClass gc = new APIGenericClass();
        GenericClass gClass = new GenericClass();
        DBValidationTest dbValTest = new DBValidationTest();

        #region GetUser_API_GetUsers --> GET
        public static IEnumerable<Dictionary<string, string>> ExcelDATA_API_GetUsers()
        {
            var testCaseDataList = new GenericClass().AddTestDataforApplicationNew("GetUser_API_GetUsers");
            return testCaseDataList;
        }

        [Test, TestCaseSource("ExcelDATA_API_GetUsers"), Category("APISmoke")]
        public void GetUser_API_GetUsers(Dictionary<string, string> TestCaseData)
        {

            string strBaseURL = GenericClass.dicApplicationUrl["baseUrl"];
            string strResourceURL = GenericClass.dicApplicationUrl["resourceUrlGetUser"];

            string pageNum = TestCaseData["Page"];
            APIAddParamUtility.pageNum = pageNum;


            string APIMethodType = TestCaseData["MethodType"];
            Reports.APIMethodType = APIMethodType;



            APIGenericClass<ObjGetUsers> objGetAllUsers = new APIGenericClass<ObjGetUsers>();
            var url = objGetAllUsers.SetUrl(strBaseURL, strResourceURL);
            var req = objGetAllUsers.CreateGETRequestWithoutAuth();
            GenericClass.sStepStartTime = DateTime.Now;
            var resp = objGetAllUsers.ExecuteAPIAndGetResponse(url, req);


            if (resp.IsSuccessful)
            {
                ObjGetUsers respContentDS = objGetAllUsers.GetResponseContent<ObjGetUsers>(resp);

                if (respContentDS.PerPage.ToString() !="0")
                {
                    string prettyJsonResp = objGetAllUsers.FormatJsonResponse(resp.Content);
                    reporter.ReportStep("", "***************************************GET all Users*********************************", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Response URI ", "<b>" + resp.ResponseUri + "</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Status code: ", "<b>'" + resp.StatusCode + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Response Content: ", "<b>'" + prettyJsonResp + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Page Number:", "<b>'" + respContentDS.Page.ToString() + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Per_Page count :", "<b>'" + respContentDS.PerPage + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Total id's per page :", "<b>'" + respContentDS.Total.ToString() + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Total pages :", "<b>'" + respContentDS.TotalPages.ToString() + "'</b>", "Pass", GenericClass.sStepStartTime);
                    Console.WriteLine(prettyJsonResp);

                    for (int i=0;i< respContentDS.Data.Length; i++)
                    {
                        //reporter.ReportStep("ID val:", ""+respContentDS.Data[i]+"", "Pass", GenericClass.sStepStartTime);
                        reporter.ReportStep("Id:", "<b>'" + respContentDS.Data[i].Id.ToString() + "'</b>", "Pass", GenericClass.sStepStartTime);
                        reporter.ReportStep("Email:", "<b>'" + respContentDS.Data[i].Email + "'</b>", "Pass", GenericClass.sStepStartTime);
                        reporter.ReportStep("First Name:", "<b>'" + respContentDS.Data[i].FirstName + "'</b>", "Pass", GenericClass.sStepStartTime);
                        reporter.ReportStep("Last Name:", "<b>'" + respContentDS.Data[i].LastName + "'</b>", "Pass", GenericClass.sStepStartTime);
                        reporter.ReportStep("Avatar:", "<b>'" + respContentDS.Data[i].Avatar + "'</b>", "Pass", GenericClass.sStepStartTime);

                    }
                }
                else
                {
                    reporter.ReportStep("Response Content: ", "<b>'" + resp.Content + "'</b>", "Fail", GenericClass.sStepStartTime);
                    reporter.ReportStep("Response URI ", "<b>" + resp.ResponseUri + "</b>", "Fail", GenericClass.sStepStartTime);
                    reporter.ReportStep("Status code: ", "<b>'" + resp.StatusCode + "'</b>", "Fail", GenericClass.sStepStartTime);                    
                }
            }
            else
            {
                reporter.ReportStep("Response Content: ", "<b>'" + resp.Content + "'</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Response URI ", "<b>" + resp.ResponseUri + "</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Status code: ", "<b>'" + resp.StatusCode + "'</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Error exception ", "<b>'" + resp.Content + "'</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Error message ", "<b>'" + resp.ErrorMessage + "'</b>", "Fail", GenericClass.sStepStartTime);
            }
        }
        #endregion

        #region GetUserById_API_GetUserById --> GET
        public static IEnumerable<Dictionary<string, string>> ExcelDATA_API_GetUserById()
        {
            var testCaseDataList = new GenericClass().AddTestDataforApplicationNew("GetUserById_API_GetUserById");
            return testCaseDataList;
        }

        [Test, TestCaseSource("ExcelDATA_API_GetUserById"), Category("APISmoke")]
        public void GetUserById_API_GetUserById(Dictionary<string, string> TestCaseData)
        {

            string strBaseURL = GenericClass.dicApplicationUrl["baseUrl"];
            string strResourceURL = GenericClass.dicApplicationUrl["resourceUrlGetUser"];

            string userId = TestCaseData["Id"];
            //APIAddParamUtility.pageNum = pageNum;


            string APIMethodType = TestCaseData["MethodType"];
            Reports.APIMethodType = APIMethodType;

            APIGenericClass<ObjGetUserById> objGetAllUsers = new APIGenericClass<ObjGetUserById>();

            string url = string.Concat(strBaseURL, strResourceURL);
            url = url + "/" + "" + userId + "";
            restRequest = new RestRequest(url);
            GenericClass.sStepStartTime = DateTime.Now;            
            IRestResponse resp = restClient.Get(restRequest);

            APIGenericClass<ObjGetUserById> objUserById = new APIGenericClass<ObjGetUserById>();

            if (resp.IsSuccessful)
            {
                ObjGetUserById respContentDS = objUserById.GetResponseContent<ObjGetUserById>(resp);

                if (respContentDS.Data.Id.ToString() != "0")
                {
                    string prettyJsonResp = objGetAllUsers.FormatJsonResponse(resp.Content);
                    //reporter.ReportStep("", "***************************************GET User By Id*********************************", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Response URI ", "<b>" + resp.ResponseUri + "</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Status code: ", "<b>'" + resp.StatusCode + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Response Content: ", "<b>'" + prettyJsonResp + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Id:", "<b>'" + respContentDS.Data.Id.ToString() + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Email:", "<b>'" + respContentDS.Data.Email + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("First Name:", "<b>'" + respContentDS.Data.FirstName + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Last Name:", "<b>'" + respContentDS.Data.LastName + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Avatar:", "<b>'" + respContentDS.Data.Avatar + "'</b>", "Pass", GenericClass.sStepStartTime);
                    Console.WriteLine(prettyJsonResp);

                }
                else
                {
                    reporter.ReportStep("Response Content: ", "<b>'" + resp.Content + "'</b>", "Fail", GenericClass.sStepStartTime);
                    reporter.ReportStep("Response URI ", "<b>" + resp.ResponseUri + "</b>", "Fail", GenericClass.sStepStartTime);
                    reporter.ReportStep("Status code: ", "<b>'" + resp.StatusCode + "'</b>", "Fail", GenericClass.sStepStartTime);
                }
            }
            else
            {
                reporter.ReportStep("Response Content: ", "<b>'" + resp.Content + "'</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Response URI ", "<b>" + resp.ResponseUri + "</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Status code: ", "<b>'" + resp.StatusCode + "'</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Error exception ", "<b>'" + resp.Content + "'</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Error message ", "<b>'" + resp.ErrorMessage + "'</b>", "Fail", GenericClass.sStepStartTime);
            }
        }
        #endregion

        #region CreateUser_API_CreateUser --> POST
        public static IEnumerable<Dictionary<string, string>> ExcelDATA_API_CreateUser()
        {
            var testCaseDataList = new GenericClass().AddTestDataforApplicationNew("CreateUser_API_CreateUser");
            return testCaseDataList;
        }

        [Test, TestCaseSource("ExcelDATA_API_CreateUser"), Category("APISmoke")]
        public void CreateUser_API_CreateUser(Dictionary<string, string> TestCaseData)
        {

            string strBaseURL = GenericClass.dicApplicationUrl["baseUrl"];
            string strResourceURL = GenericClass.dicApplicationUrl["resourceUrlGetUser"];
            string userName = TestCaseData["Username"];
            string job = TestCaseData["Job"];


            string APIMethodType = TestCaseData["MethodType"];
            Reports.APIMethodType = APIMethodType;

            string jData = "{\"name\":\""+userName+"\",\"job\":\""+job+"\"}";


            APIGenericClass<ObjCreateUser> objCreateUser = new APIGenericClass<ObjCreateUser>();
            var url = objCreateUser.SetUrl(strBaseURL, strResourceURL);
            var req = objCreateUser.CreatePOSTRequestWithoutAuth(jData);
            GenericClass.sStepStartTime = DateTime.Now;
            var resp = objCreateUser.ExecuteAPIAndGetResponse(url, req);


            if (resp.IsSuccessful)
            {
                ObjCreateUser respContentDS = objCreateUser.GetResponseContent<ObjCreateUser>(resp);

                if (respContentDS.Id.ToString()!="0")
                {
                    string prettyJsonResp = objCreateUser.FormatJsonResponse(resp.Content);
                    //reporter.ReportStep("", "***************************************GET all Users*********************************", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Response URI ", "<b>" + resp.ResponseUri + "</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Status code: ", "<b>'" + resp.StatusCode + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Response Content: ", "<b>'" + prettyJsonResp + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Id:", "<b>'" + respContentDS.Id.ToString() + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Name :", "<b>'" + respContentDS.Name + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("Job :", "<b>'" + respContentDS.Job + "'</b>", "Pass", GenericClass.sStepStartTime);
                    reporter.ReportStep("CreatedDate :", "<b>'" + respContentDS.CreatedAt.ToString() + "'</b>", "Pass", GenericClass.sStepStartTime);
                    Console.WriteLine(prettyJsonResp);
                }
                else
                {
                    reporter.ReportStep("Response Content: ", "<b>'" + resp.Content + "'</b>", "Fail", GenericClass.sStepStartTime);
                    reporter.ReportStep("Response URI ", "<b>" + resp.ResponseUri + "</b>", "Fail", GenericClass.sStepStartTime);
                    reporter.ReportStep("Status code: ", "<b>'" + resp.StatusCode + "'</b>", "Fail", GenericClass.sStepStartTime);
                }
            }
            else
            {
                reporter.ReportStep("Response Content: ", "<b>'" + resp.Content + "'</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Response URI ", "<b>" + resp.ResponseUri + "</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Status code: ", "<b>'" + resp.StatusCode + "'</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Error exception ", "<b>'" + resp.Content + "'</b>", "Fail", GenericClass.sStepStartTime);
                reporter.ReportStep("Error message ", "<b>'" + resp.ErrorMessage + "'</b>", "Fail", GenericClass.sStepStartTime);
            }
        }
        #endregion



    }
}
