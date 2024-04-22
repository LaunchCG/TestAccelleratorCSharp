using Common.TestFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Tests.PageObjects;

namespace UI.Tests.PageActions
{
    internal class SDCartTest
    {
        string reportStepDesc, reportStepVerify;
        Reports reports = new Reports();
        BaseClass baseClass = new BaseClass();

        SDCartPage sDCartPage = new SDCartPage(GenericClass.driver);
        SDCheckoutInfoPage sDCheckoutInfoPage = new SDCheckoutInfoPage(GenericClass.driver);
        public void ClickCheckoutButton()
        {
            reportStepDesc = "Verify the items added and click on checkout button";


            bool blnFlag = false;
            Err.Description = "";
            GenericClass.sStepStartTime = DateTime.Now;

            try
            {
                string Item1Desc = sDCartPage.EleItem1Desc.GetText();
                string Item2Desc = sDCartPage.EleItem2Desc.GetText();

                string Item1Price = sDCartPage.EleItem1Price.GetText();
                string Item2Price = sDCartPage.EleItem2Price.GetText();

                sDCartPage.BtnCheckout.ClickElement();
                GenericClass.Wait(2);
                blnFlag = sDCheckoutInfoPage.EleCheckoutInfoHeader.WaitForElementLoad();

                reportStepVerify = "Item: <b>" + Item1Desc + "</b> with price: <b>" + Item1Price + "</b> & Item2 :<b>" + Item2Desc + "</b> with price: <b>" + Item2Price + "</b> are checked out";
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
            Console.WriteLine("Step 5:" + reportStepVerify + "");
        }
    }
}
