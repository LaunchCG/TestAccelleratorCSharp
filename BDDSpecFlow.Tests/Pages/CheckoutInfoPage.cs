using OpenQA.Selenium;
using System;
using System.Threading;

namespace BDDSpecFlow.TestsNew.Pages
{
    class CheckoutInfoPage
    {
 
        public CheckoutInfoPage(IWebDriver webDriver)
        {
            BrowserManager.webDriver = webDriver;
        }

        public IWebElement TxtFirstName => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='first-name']"));
        public IWebElement TxtLastName => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='last-name']"));
        public IWebElement TxtPostalCode => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='postal-code']"));

        public IWebElement BtnContinue => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='checkout_info_container']/div/form/div[2]/input"));

        public IWebElement EleCheckoutInfoHeader => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='header_container']/div[2]/span"));

        public void EnterUserInfo(string FName, string LName, int Postalcode)
        {
            TxtFirstName.Clear();
            TxtFirstName.SendKeys(FName);
            Thread.Sleep(1000);
            TxtLastName.Clear();
            TxtLastName.SendKeys(LName);
            Thread.Sleep(1000);
            TxtPostalCode.Clear();
            TxtPostalCode.SendKeys(Postalcode.ToString());
        }

        public void ClickContinue() => BtnContinue.Click();

        public bool ValidateCheckoutInfoHeader()
        {
            bool blnFlag = false;
            string ChkoutInfoHeader = EleCheckoutInfoHeader.Text;
            if(ChkoutInfoHeader.Contains("Checkout: Your Information"))
                Console.WriteLine("PASS");
            else
                Console.WriteLine("FAIL");
            return blnFlag;
        }

    }



}
