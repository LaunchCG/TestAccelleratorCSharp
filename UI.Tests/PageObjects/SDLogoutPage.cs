using Common.TestFramework.Core;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Tests.PageObjects
{
    internal class SDLogoutPage
    {
        public SDLogoutPage(IWebDriver webDriver)
        {
            GenericClass.driver = webDriver;
        }

        public IWebElement EleThankyouforyourOrder => GenericClass.driver.FindElement(By.XPath("//*[@id='checkout_complete_container']/h2"));
        public IWebElement EleHamburgerIcon => GenericClass.driver.FindElement(By.XPath("//*[@id='menu_button_container']/div/div/div/button"));
        public IWebElement EleLogout => GenericClass.driver.FindElement(By.XPath("//*[@id='logout_sidebar_link']"));
    }
}
