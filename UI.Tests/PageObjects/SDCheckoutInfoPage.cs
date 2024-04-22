using Common.TestFramework.Core;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Tests.PageObjects
{
    internal class SDCheckoutInfoPage
    {
        public SDCheckoutInfoPage(IWebDriver webDriver)
        {
            GenericClass.driver = webDriver;
        }


        public IWebElement TxtFirstName => GenericClass.driver.FindElement(By.XPath("//*[@id='first-name']"));
        public IWebElement TxtLastName => GenericClass.driver.FindElement(By.XPath("//*[@id='last-name']"));
        public IWebElement TxtPostalCode => GenericClass.driver.FindElement(By.XPath("//*[@id='postal-code']"));
        public IWebElement BtnContinue => GenericClass.driver.FindElement(By.XPath("//*[@id='checkout_info_container']/div/form/div[2]/input"));
        public IWebElement EleCheckoutInfoHeader => GenericClass.driver.FindElement(By.XPath("//*[@id='header_container']/div[2]/span"));
    }
}
