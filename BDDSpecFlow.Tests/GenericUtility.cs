using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDSpecFlow.TestsNew
{
     class GenericUtility
    {
        public static string browser = ConfigurationManager.AppSettings["Browser"].ToString();
        public static string url = ConfigurationManager.AppSettings["Url"].ToString();
        public static void KillObjectInstances(string ProcessName)
        {
            Process[] myAllProcesses = Process.GetProcessesByName(ProcessName);
            if (myAllProcesses.Count() != 0)
            {
                foreach (Process objProcess in myAllProcesses)
                {
                    try
                    {
                        objProcess.Kill();
                    }

                    catch { }
                }
            }
            myAllProcesses = null;
        }

        public static void WaitForHTMLPageToLoad(IWebDriver webDriver)
        {
            IWait<IWebDriver> wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30.00));
            wait.Until(driver1 => ((IJavaScriptExecutor)webDriver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public static void SimpleWaitOf5Sec(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public static void SimpleWaitOf2Sec(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        }
    }
}
