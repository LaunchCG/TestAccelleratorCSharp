using Common.TestFramework.Core;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Tests.PageObjects
{
    internal class SDCheckoutOverviewPage
    {
        public SDCheckoutOverviewPage(IWebDriver webDriver)
        {
            GenericClass.driver = webDriver;
        }

        public IWebElement EleCheckoutOverviewHeader => GenericClass.driver.FindElement(By.XPath("//*[@id='header_container']/div[2]/span"));
        public IWebElement ElePaymentInfo => GenericClass.driver.FindElement(By.XPath("//*[@id='checkout_summary_container']/div/div[2]/div[2]"));
        public IWebElement EleShippingInfo => GenericClass.driver.FindElement(By.XPath("//*[@id='checkout_summary_container']/div/div[2]/div[4]"));
        public IWebElement EleTotalCost => GenericClass.driver.FindElement(By.XPath("//*[@id='checkout_summary_container']/div/div[2]/div[8]"));
        public IWebElement BtnFinish => GenericClass.driver.FindElement(By.XPath("//*[@id='checkout_summary_container']/div/div[2]/div[9]/button[2]"));
    }
}
