using Common.TestFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Tests.PageObjects;

namespace UI.Tests.PageActions
{
    internal class SDLoginTest
    {
        string reportStepDesc, reportStepVerify;
        Reports reports = new Reports();
        BaseClass baseClass = new BaseClass();

        SDLoginPage sDLoginPage = new SDLoginPage(GenericClass.driver);
        SDHomePage sDHomePage = new SDHomePage(GenericClass.driver);

        public bool LaunchApplicationURL(string strURL)
        {
            reportStepDesc = "Launch Sauce Demo application with URL: '<b>" + strURL + "</b>' <br>and verify if login page is successfully displayed";
            reportStepVerify = "Sauce Demo application is launched successfully";

            bool blnFlag = false;
            Err.Description = "";
            GenericClass.sStepStartTime = DateTime.Now;

            try
            {
                SystemUtil.Run(GenericClass.dicConfig["strBrowserType"], strURL);

                Browser.WaitForPageToLoad();
                Browser.SimpleWaitOf120Sec(GenericClass.driver);

                //SDLoginPage sDLoginPage = new SDLoginPage(GenericClass.driver);
                blnFlag = sDLoginPage.BtnLogin.WaitForElementLoad();
            }
            catch (Exception e)
            {
                Err.Description = e.Message;
            }

            if (blnFlag)
            {
                reports.ReportStep(reportStepDesc, reportStepVerify, "PASS", GenericClass.sStepStartTime);
            }
            else
            {
                reports.ReportStep(reportStepDesc, Err.Description, "FAIL", GenericClass.sStepStartTime);
                baseClass.TearDown();
            }
            Console.WriteLine("Step 1:"+reportStepVerify+"");
            return blnFlag;
            
        }

        public bool LoginToApplication(string UserName, string Password)
        {            

            reportStepDesc = "Login to Sauce Demo application by entering the username and password";
            reportStepVerify = "Login to Sauce Demo app with username <b>'" + UserName + "'</b> was successful. Landing screen is displayed";

            GenericClass.sStepStartTime = DateTime.Now;

            bool blnFlag = false;
            Err.Description = "";

            try
            {
                sDLoginPage.TxtUsername.Clear();
                sDLoginPage.TxtUsername.SendKeys(UserName);

                GenericClass.Wait(2);
                sDLoginPage.TxtPassword.Clear();
                sDLoginPage.TxtPassword.SendKeys(Password);
                GenericClass.Wait(2);

                sDLoginPage.BtnLogin.Click();
                GenericClass.Wait(3);

                blnFlag = sDHomePage.EleInventoryHeader.WaitForElementLoad();

            }
            catch (Exception e)
            {
                Err.Description = e.Message;
            }

            if (blnFlag)
            {
                reports.ReportStep(reportStepDesc, reportStepVerify, "PASS", GenericClass.sStepStartTime);
            }
            else
            {
                reports.ReportStep(reportStepDesc, Err.Description, "FAIL", GenericClass.sStepStartTime);
                baseClass.TearDown();
            }
            Console.WriteLine("Step 2:" + reportStepVerify + "");
            return blnFlag;
        }
    }
}
