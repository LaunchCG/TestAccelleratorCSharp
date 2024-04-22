using Common.TestFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Tests.PageObjects;

namespace UI.Tests.PageActions
{
    internal class SDLogoutTest
    {
        string reportStepDesc, reportStepVerify;
        Reports reports = new Reports();
        BaseClass baseClass = new BaseClass();

        SDLogoutPage sDLogoutPage = new SDLogoutPage(GenericClass.driver);
        SDLoginPage sDLoginPage = new SDLoginPage(GenericClass.driver);

        public void Logout()
        {
            reportStepDesc = "Logout from the app";


            bool blnFlag = false;
            Err.Description = "";
            GenericClass.sStepStartTime = DateTime.Now;

            try
            {
                sDLogoutPage.EleHamburgerIcon.ClickElement();
                sDLogoutPage.EleLogout.ClickElement();
                GenericClass.Wait(2);
                blnFlag = sDLoginPage.BtnLogin.WaitForElementLoad();

                reportStepVerify = "User successfully logged out from the application";
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
            Console.WriteLine("Step 10:" + reportStepVerify + "");
        }
    }
}
