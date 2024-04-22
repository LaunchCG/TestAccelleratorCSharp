using Common.TestFramework.Core;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Tests.PageObjects
{
    internal class SDHomePage
    {
        public SDHomePage(IWebDriver webDriver)
        {
            GenericClass.driver = webDriver;
        }

        public IWebElement EleInventoryHeader => GenericClass.driver.FindElement(By.XPath("//*[@id='header_container']/div[2]/span"));
        public IWebElement BtnAddToCartItem1 => GenericClass.driver.FindElement(By.XPath("//*[@id='inventory_container']/div/div[3]/div[2]/div[2]/button"));
        public IWebElement BtnAddToCartItem2 => GenericClass.driver.FindElement(By.XPath("//*[@id='inventory_container']/div/div[4]/div[2]/div[2]/button"));
        public IWebElement ElePageHeader => GenericClass.driver.FindElement(By.XPath("//*[@id='menu_button_container']/div"));
        public IWebElement EleTotalItemsInTheCart => GenericClass.driver.FindElement(By.XPath("//*[@id='shopping_cart_container']/a/span"));
        public IWebElement EleCartIconCheckout => GenericClass.driver.FindElement(By.XPath("//*[@id='shopping_cart_container']/a"));
        public IWebElement Item1Desc => GenericClass.driver.FindElement(By.XPath("//*[@id='shopping_cart_container']/a"));
    }
}
