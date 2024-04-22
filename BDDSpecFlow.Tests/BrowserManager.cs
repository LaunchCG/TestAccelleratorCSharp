using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Edge;

namespace BDDSpecFlow.TestsNew
{
    public class BrowserManager
    {
        public static IWebDriver webDriver;
        public static void LaunchWebdriver(string strBrowser)
        {
            //string browser = ConfigurationManager.AppSettings["Browser"].ToString();

            try
            {
                switch (strBrowser.ToString().ToLower())
                {
                    case "chrome":
                        ChromeOptions chromeOptions = new ChromeOptions();
                        webDriver = new ChromeDriver(chromeOptions);
                        break;
                    case "firefox":
                        webDriver = new FirefoxDriver();
                        break;
                    case "edge":
                        webDriver = new EdgeDriver();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
    }
}
