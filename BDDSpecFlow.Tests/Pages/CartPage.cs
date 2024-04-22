using  Common.TestFramework.Core;
using OpenQA.Selenium;
using System;

namespace BDDSpecFlow.TestsNew.Pages
{
    class CartPage
    {

        public CartPage(IWebDriver webDriver)
        {
            BrowserManager.webDriver = webDriver;
        }

        public IWebElement BtnCheckout => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='cart_contents_container']/div/div[2]/button[2]"));
        public IWebElement EleItem1Desc => BrowserManager.webDriver.FindElement(By.XPath("//*[@class='cart_list']/div[3]/div/a/div"));
        public IWebElement EleItem2Desc => BrowserManager.webDriver.FindElement(By.XPath("//*[@class='cart_list']/div[4]/div/a/div"));

        public void ClickCheckoutButton() => BtnCheckout.Click();

        public void VerifyItemDesc()
        {
            string Item1Desc = EleItem1Desc.Text;
            string Item2Desc = EleItem2Desc.Text;

            if(Item1Desc != "" && Item2Desc!="")
            {
                Console.WriteLine("PASS:Item 1 Desc: "+ Item1Desc +"");
                Console.WriteLine("PASS:Item 2 Desc: "+ Item2Desc +"");
            }
            else
            {
                Console.WriteLine("FAIL:Items not displayed");
            }
        }
    }
}
