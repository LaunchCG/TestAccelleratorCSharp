using Common.TestFramework.Core;
using UI.Tests.PageActions;
using Microsoft.VisualBasic.Devices;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace UI.Tests.TestSuite
{
    [TestFixture]
    class UIRegressionTestSuite : ConfigClass
    {
        string reportStepDesc;
        string reportStepVerify;
        ActionClass actionClass = new ActionClass();
        BaseClass baseClass = new BaseClass();
        GenericClass gClass = new GenericClass();
        DBValidationTest dbValTest = new DBValidationTest();


        #region PurchaseItems_UI_SD_App_PurchaseItems

        public static IEnumerable<Dictionary<string, string>> ExcelDATA_UI_SD_App_PurchaseItems()
        {
            var testCaseDataList = new GenericClass().AddTestDataforApplicationNew("PurchaseItems_UI_SD_App_PurchaseItems");
            return testCaseDataList;
        }

        [Test, TestCaseSource("ExcelDATA_UI_SD_App_PurchaseItems"), Category("UISmoke")]
        public void PurchaseItems_UI_SD_App_PurchaseItems(Dictionary<string, string> TestCaseData)
        {
            string strUrl = GenericClass.dicApplicationUrl["SauceDemoUrl"];
            string strUsername = GenericClass.dicApplicationUrl["SauceDemoUsername"];
            string strPassword = GenericClass.dicApplicationUrl["SauceDemoPassword"];

            string strFName = TestCaseData["UserFirstName"];
            string strLName = TestCaseData["UserLastName"];
            string strPostalCode = TestCaseData["UserPostalCode"];


            string strEnvironment = TestCaseData["Env"];
            Reports.Instance = strEnvironment;


            //Login
            SDLoginTest loginTest = new SDLoginTest();
            loginTest.LaunchApplicationURL(strUrl);
            loginTest.LoginToApplication(strUsername, strPassword);

            //Home Page
            SDHomeTest homeTest = new SDHomeTest();
            homeTest.AddItemsToCart();
            homeTest.ClickCartButtonToCheckout();

            //Cart Page
            SDCartTest cartTest = new SDCartTest();
            cartTest.ClickCheckoutButton();

            //Checkout Info
            SDCheckoutInfoTest checkoutInfoTest = new SDCheckoutInfoTest();
            checkoutInfoTest.EnterUserInfo(strFName, strLName, strPostalCode);
            checkoutInfoTest.ClickContinueButton();

            //Checkout Overview
            SDCheckoutOverviewTest checkoutOverviewTest = new SDCheckoutOverviewTest();
            checkoutOverviewTest.ValidatePaymentInfoShippingInfoTotalCost();
            checkoutOverviewTest.ClickFinishButton();

            //Logout
            SDLogoutTest logoutTest = new SDLogoutTest();
            logoutTest.Logout();

            //DB Validation Tests
            dbValTest.VerifyOrderIdDetails("200124");
            dbValTest.VerifyOrderIdDetails("200107");

            dbValTest.VerifyEmpDetails("5");
            dbValTest.VerifyEmpDetails("12");

        }
        #endregion


    }
}
