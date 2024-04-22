using Common.TestFramework.Core;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Tests.PageObjects
{
    internal class SDLoginPage
    {
        public SDLoginPage(IWebDriver webDriver)
        {
            GenericClass.driver = webDriver;
        }

        public IWebElement TxtUsername => GenericClass.driver.FindElement(By.XPath("//*[@id='user-name']"));
        public IWebElement TxtPassword => GenericClass.driver.FindElement(By.XPath("//*[@id='password']"));
        public IWebElement BtnLogin => GenericClass.driver.FindElement(By.XPath("//*[@id='login-button']"));
    }
}
