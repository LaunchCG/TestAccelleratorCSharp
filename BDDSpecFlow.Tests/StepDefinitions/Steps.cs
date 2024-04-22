using BDDSpecFlow.TestsNew.Pages;
using BDDSpecFlow.TestsNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using System.Threading;
using TechTalk.SpecFlow.Assist;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace BDDSpecFlow.Tests.StepDefinitions
{
    [Binding]
    public class Steps
    {

        LoginPage loginPage = new LoginPage(BrowserManager.webDriver);        
        LandingPage landingPage = new LandingPage(BrowserManager.webDriver);
        CartPage cartPage = new CartPage(BrowserManager.webDriver);
        CheckoutInfoPage checkoutInfoPage = new CheckoutInfoPage(BrowserManager.webDriver);
        CheckoutOverviewPage checkoutOverviewPage = new CheckoutOverviewPage(BrowserManager.webDriver);
        FinishPage finishPage = new FinishPage(BrowserManager.webDriver);

        [Given(@"I open the browser and enter url")]
        public void GivenIOpenTheBrowserAndEnterUrl()
        {
            //string browser = ConfigurationManager.AppSettings["Browser"].ToString();
            BrowserManager.LaunchWebdriver(GenericUtility.browser);
            //string url = ConfigurationManager.AppSettings["Url"].ToString();
            BrowserManager.webDriver.Navigate().GoToUrl(GenericUtility.url);
            BrowserManager.webDriver.Manage().Window.Maximize();
            Thread.Sleep(2000);
            //GenericUtility.WaitForHTMLPageToLoad(BrowserManager.webDriver);
        }

        [Given(@"I enter username and password")]
        public void GivenIEnterUsernameAndPassword(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            loginPage.EnterUsernamePwd((string)data.Username, (string)data.Password);
        }

        [Given(@"I click on Login button")]
        public void GivenIClickOnLoginButton()
        {
            loginPage.ClickLoginButton();
            GenericUtility.SimpleWaitOf2Sec(BrowserManager.webDriver);
            GenericUtility.WaitForHTMLPageToLoad(BrowserManager.webDriver);
            Thread.Sleep(3000);
        }

        [Given(@"I verify the title of homepage")]
        public void GivenIVerifyTheTitleOfHomepage()
        {
            landingPage.ValidateLandingPageHeader();
        }

        [When(@"I select Item(.*) and add to cart")]
        public void WhenISelectItemAndAddToCart(int Item)
        {
            if (Item == 1)
            {
                landingPage.AddToCartItem1();
            }

            Thread.Sleep(2000);

            if (Item == 2)
            {
                landingPage.AddToCartItem2();
            }
            Thread.Sleep(2000);
        }

        [When(@"I verify the total items in the cart")]
        public void WhenIVerifyTheTotalItemsInTheCart()
        {
            landingPage.ValidateTotalItemsInCart();
        }

        [When(@"I click on Cart icon")]
        public void WhenIClickOnCartIcon()
        {
            landingPage.ClickCartIcon();
            GenericUtility.SimpleWaitOf5Sec(BrowserManager.webDriver);
            GenericUtility.WaitForHTMLPageToLoad(BrowserManager.webDriver);
            Thread.Sleep(3000);
        }

        [When(@"I verify the items before checking out")]
        public void WhenIVerifyTheItemsBeforeCheckingOut()
        {
            cartPage.VerifyItemDesc();
        }

        [When(@"I click on Checkout button")]
        public void WhenIClickOnCheckoutButton()
        {
            cartPage.ClickCheckoutButton();
            GenericUtility.SimpleWaitOf5Sec(BrowserManager.webDriver);
            GenericUtility.WaitForHTMLPageToLoad(BrowserManager.webDriver);
        }

        [When(@"i verify the title in the checkout screen")]
        public void WhenIVerifyTheTitleInTheCheckoutScreen()
        {
            checkoutInfoPage.ValidateCheckoutInfoHeader();
        }

        [When(@"I enter user information")]
        public void WhenIEnterUserInformation(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            checkoutInfoPage.EnterUserInfo((string)data.Firstname, (string)data.Lastname, (int)data.PostalCode);
        }

        [When(@"I click on Continue button")]
        public void WhenIClickOnContinueButton()
        {
            checkoutInfoPage.ClickContinue();
            GenericUtility.WaitForHTMLPageToLoad(BrowserManager.webDriver);
            Thread.Sleep(3000);
        }

        [When(@"I verify the Shipping Payment info and Total cost")]
        public void WhenIVerifyTheShippingPaymentInfoAndTotalCost()
        {
            checkoutOverviewPage.ValidatePaymentInfoShippingInfoTotalCost();
        }

        [When(@"I Click on Finish button")]
        public void WhenIClickOnFinishButton()
        {
            checkoutOverviewPage.ClickFinish();
            GenericUtility.WaitForHTMLPageToLoad(BrowserManager.webDriver);
            Thread.Sleep(3000);
        }

        [When(@"I verify the order placed")]
        public void WhenIVerifyTheOrderPlaced()
        {
            /*WebDriverWait wait = new WebDriverWait(BrowserManager.webDriver, System.TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='checkout_complete_container']/h2")));*/
            finishPage.ValidateSuccessOrderCreation();
        }

        [Then(@"I logout from the app")]
        public void ThenILogoutFromTheApp()
        {
            finishPage.Logout();
        }

        [Given(@"I enter invalid ""([^""]*)"" and ""([^""]*)""")]
        public void GivenIEnterInvalidAnd(string username, string password)
        {
            loginPage.EnterUsernamePwd(username, password);
        }

    }
}
