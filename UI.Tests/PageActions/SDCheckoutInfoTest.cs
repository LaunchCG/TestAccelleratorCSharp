using Common.TestFramework.Core;
using OpenQA.Selenium.DevTools.V121.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UI.Tests.PageObjects;

namespace UI.Tests.PageActions
{
    internal class SDCheckoutInfoTest
    {
        string reportStepDesc, reportStepVerify;
        Reports reports = new Reports();
        BaseClass baseClass = new BaseClass();

        SDCheckoutInfoPage sDCheckoutInfoPage = new SDCheckoutInfoPage(GenericClass.driver);
        SDCheckoutOverviewPage sDCheckoutOverviewPage = new SDCheckoutOverviewPage(GenericClass.driver);

        public void EnterUserInfo(string fName, string lName, string postalCode)
        {

            reportStepDesc = "Enter the user information";            

            GenericClass.sStepStartTime = DateTime.Now;

            bool blnFlag = false;
            Err.Description = "";

            try
            {
                sDCheckoutInfoPage.TxtFirstName.Clear();
                sDCheckoutInfoPage.TxtFirstName.SendKeys(fName);
                Thread.Sleep(1000);
                sDCheckoutInfoPage.TxtLastName.Clear();
                sDCheckoutInfoPage.TxtLastName.SendKeys(lName);
                Thread.Sleep(1000);
                sDCheckoutInfoPage.TxtPostalCode.Clear();
                sDCheckoutInfoPage.TxtPostalCode.SendKeys(postalCode.ToString());

                blnFlag = sDCheckoutInfoPage.BtnContinue.WaitForElementLoad();

                reportStepVerify = "User entered Firstname as: <b>" + fName + "</b>, Lastname as: <b>" + lName + "</b> and Postalcode as: <b>" + postalCode + "</b>";

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
            Console.WriteLine("Step 6:" + reportStepVerify + "");
        }

        public void ClickContinueButton()
        {
            reportStepDesc = "Click on continue button in the Checkout information screen";


            bool blnFlag = false;
            Err.Description = "";
            GenericClass.sStepStartTime = DateTime.Now;

            try
            {
                sDCheckoutInfoPage.BtnContinue.ClickElement();
                GenericClass.Wait(2);
                blnFlag = sDCheckoutOverviewPage.EleCheckoutOverviewHeader.WaitForElementLoad();

                reportStepVerify = "User navigated to Checkout overview/Finish screen";
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
            Console.WriteLine("Step 7:" + reportStepVerify + "");
        }
    }
}
