using OpenQA.Selenium;
using System;

namespace BDDSpecFlow.TestsNew.Pages
{
    class LandingPage
    {

        public LandingPage(IWebDriver webDriver)
        {
            BrowserManager.webDriver = webDriver;
        }

        public IWebElement BtnAddToCartItem1 => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='inventory_container']/div/div[1]/div[2]/div[2]/button"));
        public IWebElement BtnAddToCartItem2 => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='inventory_container']/div/div[2]/div[2]/div[2]/button"));
        public IWebElement ElePageHeader => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='menu_button_container']/div"));
        public IWebElement EleTotalItemsInTheCart => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='shopping_cart_container']/a/span"));
        public IWebElement EleCartIconCheckout => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='shopping_cart_container']/a"));

        public bool ValidateLandingPageHeader()
        {
            return ElePageHeader.Displayed;
        }

        public void AddToCartItem1() => BtnAddToCartItem1.Click();
        public void AddToCartItem2() => BtnAddToCartItem2.Click();

        public void ValidateTotalItemsInCart()
        {
            string TotalItemsInCart = EleTotalItemsInTheCart.Text;
            if(TotalItemsInCart =="2")
                Console.WriteLine("PASS:Total # of items in the cart is {0}",TotalItemsInCart.ToString());
            else
                Console.WriteLine("FAIL:No items in the cart");            
        }
        public void ClickCartIcon() => EleCartIconCheckout.Click();

    }
}
