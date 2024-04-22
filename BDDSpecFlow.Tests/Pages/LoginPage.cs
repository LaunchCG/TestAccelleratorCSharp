using OpenQA.Selenium;

namespace BDDSpecFlow.TestsNew.Pages
{
    class LoginPage
    {

        public LoginPage(IWebDriver webDriver)
        {
            BrowserManager.webDriver = webDriver;
        }

        public IWebElement TxtUsername => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='user-name']"));
        public IWebElement TxtPassword => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='password']"));
        public IWebElement BtnLogin => BrowserManager.webDriver.FindElement(By.XPath("//*[@id='login-button']"));


        public void ClickLoginButton() => BtnLogin.Click();

        public void EnterUsernamePwd(string UN, string Pwd)
        {
            TxtUsername.Clear();
            TxtUsername.SendKeys(UN);
            TxtPassword.Clear();
            TxtPassword.SendKeys(Pwd);
        }

    }
}
