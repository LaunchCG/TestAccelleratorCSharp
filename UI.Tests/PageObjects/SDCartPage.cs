using Common.TestFramework.Core;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Tests.PageObjects
{
    internal class SDCartPage
    {
        public SDCartPage(IWebDriver webDriver)
        {
            GenericClass.driver = webDriver;
        }

        public IWebElement BtnCheckout => GenericClass.driver.FindElement(By.XPath("//*[@id='cart_contents_container']/div/div[2]/button[2]"));
        public IWebElement EleItem1Desc => GenericClass.driver.FindElement(By.XPath("//*[@class='cart_list']/div[3]/div/a/div"));
        public IWebElement EleItem2Desc => GenericClass.driver.FindElement(By.XPath("//*[@class='cart_list']/div[4]/div/a/div"));        
        public IWebElement EleItem1Price => GenericClass.driver.FindElement(By.XPath("//*[@class='cart_list']/div[3]/div[2]/div[2]/div"));
        public IWebElement EleItem2Price => GenericClass.driver.FindElement(By.XPath("//*[@class='cart_list']/div[4]/div[2]/div[2]/div"));

    }
}
