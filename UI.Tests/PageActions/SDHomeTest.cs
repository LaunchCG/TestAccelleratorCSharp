using Common.TestFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Tests.PageObjects;

namespace UI.Tests.PageActions
{
    internal class SDHomeTest
    {
        string reportStepDesc, reportStepVerify;
        Reports reports = new Reports();
        BaseClass baseClass = new BaseClass();

        SDHomePage sDHomePage = new SDHomePage(GenericClass.driver);
        SDCartPage sDCartPage = new SDCartPage(GenericClass.driver);

        public void AddItemsToCart()
        {
            reportStepDesc = "Add couple of items to the cart";
            

            bool blnFlag = false;
            Err.Description = "";
            GenericClass.sStepStartTime = DateTime.Now;

            try
            {
                sDHomePage.BtnAddToCartItem1.ClickElement();
                GenericClass.Wait(2);
                sDHomePage.BtnAddToCartItem2.ClickElement();
                GenericClass.Wait(2);

                string TotalItemsInCart = sDHomePage.EleTotalItemsInTheCart.Text;
                if (TotalItemsInCart == "2")
                {
                    blnFlag = true;
                }
                else
                {
                    blnFlag = false;   
                }


                reportStepVerify = "<b>2</b> Items are added to the cart";
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
            Console.WriteLine("Step 3:" + reportStepVerify + "");
        }

        public void ClickCartButtonToCheckout()
        {
            reportStepDesc = "Click on cart button to checkout the items";


            bool blnFlag = false;
            Err.Description = "";
            GenericClass.sStepStartTime = DateTime.Now;

            try
            {
                sDHomePage.EleCartIconCheckout.ClickElement();
                GenericClass.Wait(2);
                blnFlag = sDCartPage.BtnCheckout.WaitForElementLoad();

                reportStepVerify = "User navigated to Checkout screen";
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
            Console.WriteLine("Step 4:" + reportStepVerify + "");
        }
    }
}
