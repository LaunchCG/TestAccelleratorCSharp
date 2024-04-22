using Common.TestFramework.Core;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Tests.PageObjects;

namespace UI.Tests.PageActions
{
    internal class SDCheckoutOverviewTest
    {
        string reportStepDesc, reportStepVerify;
        Reports reports = new Reports();
        BaseClass baseClass = new BaseClass();

        SDCheckoutOverviewPage sDCheckoutOverviewPage = new SDCheckoutOverviewPage(GenericClass.driver);

        public void ValidatePaymentInfoShippingInfoTotalCost()
        {
            reportStepDesc = "Validate the Payment Info, Shipping Info and Price total of ordered items";


            bool blnFlag = false;
            Err.Description = "";
            GenericClass.sStepStartTime = DateTime.Now;

            try
            {
                string PaymentInfo = sDCheckoutOverviewPage.ElePaymentInfo.GetText();                
                string ShippingInfo = sDCheckoutOverviewPage.EleShippingInfo.GetText();
                string TotalCost = sDCheckoutOverviewPage.EleTotalCost.GetText();

                GenericClass.Wait(2);
                blnFlag = sDCheckoutOverviewPage.BtnFinish.WaitForElementLoad();

                reportStepVerify = "Payment info: <b>" + PaymentInfo + "</b>, Shipping Info: <b>" + ShippingInfo + "</b>, Total Cost: <b>" + TotalCost + "</b>";
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
            Console.WriteLine("Step 8:" + reportStepVerify + "");
        }

        public void ClickFinishButton()
        {
            reportStepDesc = "Click on Finish button";


            bool blnFlag = false;
            Err.Description = "";
            GenericClass.sStepStartTime = DateTime.Now;

            try
            {
                sDCheckoutOverviewPage.BtnFinish.ClickElement();
                GenericClass.Wait(2);

                IWebElement EleThankyou = GenericClass.driver.FindElement(By.XPath("//*[@id='checkout_complete_container']/h2"));
                string strThankyouforyourOrder = EleThankyou.GetText();

                if (strThankyouforyourOrder.Contains("Thank you for your order"))
                {
                    blnFlag = true;
                }
                else
                {
                    blnFlag = false;
                }

                reportStepVerify = "User has successfully completed the order and message displayed as: <b>" + strThankyouforyourOrder + "</b>";
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
            Console.WriteLine("Step 9:" + reportStepVerify + "");
        }
    }
}
