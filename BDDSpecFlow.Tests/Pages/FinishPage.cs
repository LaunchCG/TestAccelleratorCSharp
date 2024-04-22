using OpenQA.Selenium;
using System.Threading;

namespace BDDSpecFlow.TestsNew.Pages
{
    class FinishPage
    {
        public FinishPage(IWebDriver webDriver)
        {
            BrowserManager.webDriver = webDriver;
        }

        public IWebElement EleThankyouforyourOrder => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='checkout_complete_container']/h2"));
        public IWebElement EleHamburgerIcon => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='menu_button_container']/div/div/div/button"));
        public IWebElement EleLogout => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='logout_sidebar_link']"));
        

        public bool ValidateSuccessOrderCreation() => EleThankyouforyourOrder.Displayed;

        public void Logout()
        {
            EleHamburgerIcon.Click();
            
            EleLogout.Click();
        }
    }
}
