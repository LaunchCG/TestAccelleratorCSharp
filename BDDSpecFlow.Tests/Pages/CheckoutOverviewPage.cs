using OpenQA.Selenium;
using System;

namespace BDDSpecFlow.TestsNew.Pages
{
    class CheckoutOverviewPage
    {
        //public CheckoutOverviewPage()
        //{
        //    PageFactory.InitElements(BrowserManager.webDriver, this);
        //}

        public CheckoutOverviewPage(IWebDriver webDriver)
        {
            BrowserManager.webDriver = webDriver;
        }

        public IWebElement ElePaymentInfo => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='checkout_summary_container']/div/div[2]/div[2]"));
        public IWebElement EleShippingInfo => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='checkout_summary_container']/div/div[2]/div[4]"));
        public IWebElement EleTotalCost => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='checkout_summary_container']/div/div[2]/div[8]"));
        public IWebElement BtnFinish => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='checkout_summary_container']/div/div[2]/div[9]/button[2]"));

        public void ValidatePaymentInfoShippingInfoTotalCost()
        {
            string PaymentInfo = ElePaymentInfo.Text;
            Console.WriteLine("PaymentInfo:"+PaymentInfo);
            string ShippingInfo = EleShippingInfo.Text;
            Console.WriteLine("ShippingInfo:" + ShippingInfo);
            string Totalcost = EleTotalCost.Text;
            Console.WriteLine("TotalCost:"+Totalcost);
        }

        public void ClickFinish() => BtnFinish.Click();
    }
}
