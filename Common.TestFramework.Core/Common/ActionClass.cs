using System;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Chrome;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.Win32;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Configuration;
using OpenQA.Selenium.Edge;

namespace Common.TestFramework.Core
{

    public class ActionClass
    {
        static string strPageNameInOR = null;
        static string strObjectProperty = null;
        static string strObjectNameInOR = null;
        public Dictionary<string, string> dictParamOut = new Dictionary<string, string>();

        public Elements Page(string strPageName)
        {
            strPageNameInOR = strPageName;
            return new Elements();
        }

        public class Elements
        {
            //Constructor
            public Elements()
            {
                Err.Description = null;
            }

            public void Frame(int frameIndex)
            {
                GenericClass.driver.SwitchTo().Frame(frameIndex);
            }

            /// <summary>
            /// Switch to Frame though framename
            /// </summary>
            /// <returns></returns>
            public void Frame(String frameName)
            {
                GenericClass.driver.SwitchTo().Frame(frameName);
            }

            /// <summary>
            /// Switch to main content or default content
            /// </summary>
            /// <returns></returns>
            public void DefaultContent()
            {
                GenericClass.driver.SwitchTo().DefaultContent();
            }

        }


        //This method fetchs the obsolute path for file from DataSheet folder in the project
        public string FilePathObsolute(string strFilenamePath)
        {
            var sIndexPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString().IndexOf(@"bin");
            var sCurrentDir = sIndexPath == -1 ? AppDomain.CurrentDomain.BaseDirectory : System.AppDomain.CurrentDomain.BaseDirectory.Substring(0, sIndexPath);

            //string strSearchFolderPath = string.Format("{0}{1}\\", sCurrentDir, ConfigurationManager.AppSettings["SearchTestDataPath"]);
            string strSearchFolderPath = string.Format("{0}{1}\\", sCurrentDir, ConfigurationManager.AppSettings["MasterTestDataPath"]); 
            strFilenamePath = strSearchFolderPath + strFilenamePath;

            return strFilenamePath;
        }

        public bool PerformActionOnAlert(string action)
        {
            bool blnFlag = true;
            Err.Description = "";
            //strOutText = "";

            try
            {
                // WebDriverWait wait = new WebDriverWait(TestGenericUtility.driver, TimeSpan.FromSeconds(60));
                //wait.Until(ExpectedConditions.AlertIsPresent());
                GenericClass.Wait(1);
                IAlert alert = GenericClass.driver.SwitchTo().Alert();

                switch (action.ToUpper())
                {
                    case "OK":
                        {
                            //strOutText = alert.Text;
                            if (dictParamOut.ContainsKey("alertText"))
                            {
                                dictParamOut["alertText"] = alert.Text;
                            }
                            else
                            {
                                dictParamOut.Add("alertText", alert.Text);
                            }

                            alert.Accept();
                            break;
                        }

                    case "CANCEL":
                        {
                            alert.Dismiss();
                            break;
                        }
                    case "GETTEXT":
                        {
                            if (dictParamOut.ContainsKey("alertText"))
                            {
                                dictParamOut["alertText"] = alert.Text;
                            }
                            else
                            {
                                dictParamOut.Add("alertText", alert.Text);
                            }
                            break;
                        }

                }
            }
            catch (Exception e)
            {
                Err.Description = e.Message;
                blnFlag = false;
            }

            return blnFlag;

        }

        
        //This Class contains functions for various actions
        public class Events
        {
            //Constructor for handling Webdriver
            IWebElement element = null;
            public Events()
            {
                if (Err.Description == null)
                {
                    if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    {
                        try
                        {
                            element = GenericClass.driver.FindElement(By.XPath(strObjectProperty));
                            if (element.Displayed == true || element.Enabled == true)
                                Err.Description = null;
                            else
                                Err.Description = "OBJECT '" + strObjectNameInOR + "' is not displayed on PAGE '" + strPageNameInOR + "'";
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            element = null;
                            Err.Description = "OBJECT '" + strObjectNameInOR + "' is not found on PAGE '" + strPageNameInOR + "'";
                        }
                    }
                }
            }



            public bool ClickAtCenter(int intTimeout = 120)
            {
                bool flag = false;
                try
                {
                    if (Err.Description != null)
                        return false;
                    if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    {
                        GenericClass g1 = new GenericClass();
                        g1.ExecuteJavascript("window.focus()");
                        if (GenericClass.dicConfig["strBrowserType"].ToLower().Contains("firefox"))
                        {
                            Browser.Maximize();
                            Browser.WindowFocus();
                            //Getting element location
                            int elementXoffset = element.Location.X;
                            int elementYoffset = element.Location.Y;
                            //Getting element width & heigth
                            int elementWidth = element.Size.Width;
                            int elementHeight = element.Size.Height;
                            //Getting offset for element's mid position
                            int elementMidXoffset = elementXoffset + (elementWidth / 2);
                            int elementMidYoffset = elementYoffset + (elementHeight / 2);
                            OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(GenericClass.driver);
                            action.MoveByOffset(elementMidXoffset, elementMidYoffset).ContextClick().Build().Perform();
                        }
                        else
                        {
                            OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(GenericClass.driver);
                            action.Click(element).Build().Perform();
                        }
                    }
                }
                catch (Exception e)
                {
                    flag = false;
                    Err.Description = e.Message;
                }
                return flag;
            }


            //----------------------------------------------------------------------------------------//
            // Function Name : MouseHover
            // Function Description : This function perform mouse hover operation on object passed to 'Element(string strObjectName)'
            // Input Variable : 
            // OutPut : true / false
            // example : Page("Google").Element("SearchButton").MouseHover()
            //---------------------------------------------------------------------------------------//
            public bool MouseHover()
            {
                bool flag = false;
                try
                {
                    if (Err.Description != null)
                        return false;
                    if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    {
                        OpenQA.Selenium.Interactions.Actions builder = new OpenQA.Selenium.Interactions.Actions(GenericClass.driver);
                        builder.MoveToElement(element).Build().Perform();
                    }
                }
                catch (Exception e)
                {
                    flag = false;
                    Err.Description = e.Message;
                }
                return flag;
            }            


            /// <summary>
            /// Switch to Frame though OR objects
            /// </summary>
            /// <returns></returns>

            public bool SwitchFrame()
            {
                bool flag = false;
                try
                {
                    GenericClass.driver.SwitchTo().Frame(element);
                    flag = true;
                }
                catch (Exception e)
                {
                    Err.Description = e.Message;
                }

                return flag;
            }



            //----------------------------------------------------------------------------------------//
            // Function Name : isVisible
            // Function Description : This function verifies the visibility of an object passed to 'Element(string strObjectName)'
            // Input Variable : 
            // OutPut : true / false
            // example : Page("Google").Element("lnkEspanol").isVisible()
            //---------------------------------------------------------------------------------------//
            public bool isVisible()
            {
                bool flag = false;
                try
                {
                    if (Err.Description != null)
                        return false;
                    if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                        flag = element.Displayed;
                }
                catch (Exception e)
                {
                    flag = false;
                    Err.Description = e.Message;
                }
                return flag;
            }

            //----------------------------------------------------------------------------------------//
            // Function Name : getValue
            // Function Description : This function gets the value of an input box object passed to 'Element(string strObjectName)'
            // Input Variable : 
            // OutPut : String text - for input box, on/off - for checkbox or radio
            // example : Page("Google").Element("SearchBox").getValue()
            //---------------------------------------------------------------------------------------//
            public String GetValue()
            {
                string strElementValue = "";
                try
                {
                    if (Err.Description != null)
                        throw new Exception(Err.Description);
                    if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                        strElementValue = element.GetAttribute("value");
                }
                catch (Exception e)
                {
                    strElementValue = "";
                    Err.Description = e.Message;
                }
                return strElementValue;
            }


        }
    }


    //This class deals with starting selenium server and Browser launching 
    public static class SystemUtil
    {
        static GenericClass objGeneric = new GenericClass();

        public static void Run()
        {
            //@ Application URL Handling for Reporting

            if (GenericClass.dicConfig.ContainsKey("strBrowserType"))
            {                
                Run(GenericClass.dicConfig["strBrowserType"], GenericClass.dicApplicationUrl["strApplicationURL"]);
            }
            else
                objGeneric.TeardownTest();
        }


        public static void Run(string strBrowser, string strURL)
        {
            GenericClass.strBrowser = strBrowser;
            GenericClass.applicationURL = strURL;

            try
            {
                // Handling Browser
                switch (GenericClass.strBrowser.ToString().ToLower())
                {
                    case "firefox":
                        GenericClass.strBrowser = "*firefox";
                        break;
                    case "ie":
                        GenericClass.strBrowser = "*iexplore";
                        break;
                    case "chrome":
                        GenericClass.strBrowser = "*googlechrome";
                        break;                     
                    case "opera":
                        GenericClass.strBrowser = "*opera";
                        break;
                    case "edge":
                        GenericClass.strBrowser = "*edge";
                        break;

                }
                //Check if the browser is installed in the system
                if (!isInstalled(GenericClass.strBrowser.ToString().ToLower()))
                {
                    Console.WriteLine(GenericClass.strBrowser.ToString() + " is not installed on your system. Please install the " + GenericClass.strBrowser.ToString() + " and retry.");
                    throw new Exception("Browser not installed.");
                }

                //Appending http
                if (!GenericClass.applicationURL.ToLower().Contains("http"))
                    GenericClass.applicationURL = "http://" + GenericClass.applicationURL;
                //If key 'SeleniumVariant' is not present, add it with value 'RemoteControl'
                if (!GenericClass.dicConfig.ContainsKey("SeleniumVariant"))
                    GenericClass.dicConfig.Add("SeleniumVariant", "RemoteControl");
                //If 'SeleniumVariant' is set to Webdriver, launch webdriver otherwise launch Remote Control
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {                 
                    LaunchWebDriver(strBrowser);
                    Browser.Maximize();
                    GenericClass.driver.Navigate().GoToUrl(GenericClass.applicationURL);
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                objGeneric.TeardownTest();
            }
        }

        private static bool isInstalled(string strBorwserType)
        {
            string registry_key = null;

            //Define registry name on the basis of OS version
            if (Environment.Is64BitOperatingSystem)
                registry_key = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\App Paths";
            else
                registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        try
                        {

                            if (subkey.Name.ToString().ToLower().Contains(strBorwserType) && strBorwserType != "ie")
                                return true;
                            else if (subkey.Name.ToString().ToLower().Contains("iexplore"))
                                return true;
                        }
                        catch (Exception ex)
                        { Console.WriteLine(ex.Message); }
                    }
                }
            }
            return false;
        }     


        //Changed from Private to Public for accessing in the main TC in the Regression suite
        public static void LaunchWebDriver(string strBrowser)
        {

            if (!GenericClass.dicConfig.ContainsKey("DownloadsPath"))
            {
                GenericClass.dicConfig.Add("DownloadsPath", (GenericClass.dicConfig["MasterTestDataPath"].Replace(GenericClass.dicConfig["MasterTestData"], "Downloads\\")));
            }   

            try
            {
                //strBrowser = System.Environment.GetEnvironmentVariable("BROWSER");
                switch (strBrowser.ToString().ToLower())
                {
                    //Passing respective driver path
                    case "firefox":
                        FirefoxProfile profile = new FirefoxProfile();
                        profile.SetPreference("browser.download.dir", GenericClass.dicConfig["DownloadsPath"]);
                        profile.SetPreference("browser.download.folderList", 1);
                        profile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");
                        break;
                    case "ie":
                        InternetExplorerOptions ieoptions = new InternetExplorerOptions();
                        objGeneric.KillObjectInstances("IEDriverServer");
                        ieoptions.EnsureCleanSession = true;
                        ieoptions.EnableNativeEvents = true;
                        //ieoptions.IntroduceInstabilityByIgnoringProtectedModeSettings = false;
                        //ieoptions.ForceCreateProcessApi = true;
                        //ieoptions.BrowserCommandLineArguments = "-private";
                        //ieoptions.EnablePersistentHover = false;
                        //ieoptions.RequireWindowFocus = true;
                        //ieoptions.IgnoreZoomLevel = true;
                        //ieoptions.ElementScrollBehavior = InternetExplorerElementScrollBehavior.Top;                        
                        //TestGenericUtility.driver = new InternetExplorerDriver(@TestGenericUtility.dicConfig["ServerPath"], ieoptions, TimeSpan.FromMinutes(3));
                        GenericClass.driver = new InternetExplorerDriver(GenericClass.dicConfig["ServerPath"], ieoptions);
                        //TestGenericUtility.driver = new InternetExplorerDriver(@TestGenericUtility.dicConfig["ServerPath"]);
                        break;
                    case "chrome":
                        //objGeneric.KillObjectInstances("chromedriver");
                        ChromeOptions chromeOptions = new ChromeOptions();
                        //chromeOptions.AddArgument("--disable-extensions");
                        //chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
                        //chromeOptions.AddArgument("--enable-extensions");
                        //chromeOptions.AddArgument("-no-sandbox");
                        //chromeOptions.AddArgument("--test-type");
                        chromeOptions.AddUserProfilePreference("download.default_directory", GenericClass.dicConfig["DownloadsPath"]);
                        chromeOptions.AddUserProfilePreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");
                        GenericClass.driver = new ChromeDriver(GenericClass.dicConfig["ServerPath"], chromeOptions, TimeSpan.FromMinutes(3));
                        break;                    

                    case "opera":
                        GenericClass.driver = new SafariDriver();
                        break;
                    case "edge":
                        //objGeneric.KillObjectInstances("Microsoft Web Driver (32 bit)");
                        objGeneric.KillObjectInstances("MicrosoftWebDriver");
                        EdgeOptions edgeOptions = new EdgeOptions();
                        //edgeOptions.UseChromium = true;
                        //edgeOptions.AcceptInsecureCertificates = true;                        
                        //edgeOptions.AddArgument("-no-sandbox");
                        //edgeOptions.AddArgument("--test-type");
                        GenericClass.driver = new EdgeDriver(GenericClass.dicConfig["ServerPath"], edgeOptions, TimeSpan.FromMinutes(3));
                        //TestGenericUtility.driver = new EdgeDriver();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                objGeneric.TeardownTest();
            }


            //}
            ////Console.WriteLine("Finished execution of LaunchWebDriver function");

        }

    }

    //This class performs various actions on Browsers
    public static class Browser
    {
        static GenericClass objGeneric = new GenericClass();

        //----------------------------------------------------------------------------------------//
        // Function Name : WaitForPageToLoad
        // Function Description : This function halts the execution commands until the browser is fully loaded or timer exceeds user timeout
        // Input Variable : Field name, for e.g. Category
        // OutPut : Cell value held by the column in that particular row
        // example : string strCategory = FetchDatafromFile(strFieldName)
        //---------------------------------------------------------------------------------------//
        public static bool WaitForPageToLoad(int intTimeOut = 120)
        {

            if (GenericClass.dicOutput.ContainsKey("WaitForPageToLoad"))
                GenericClass.dicOutput.Remove("WaitForPageToLoad");
            DateTime dtInitial = DateTime.Now;
            intTimeOut = intTimeOut * Convert.ToInt32(1);

            string strBrowserState = "";

            int intTime = 0;
            if (!GenericClass.dicConfig["strBrowserType"].ToLower().Contains("*safari"))
            {
                tagGotoHere:
                try
                {
                    if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    {
                        while (((IJavaScriptExecutor)GenericClass.driver).ExecuteScript("return navigator.onLine").ToString().ToLower() != "true" && intTime <= intTimeOut)
                        {
                            Thread.Sleep(1000);
                            intTime += 1;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Thread.Sleep(1000);
                    intTime += 1;
                    goto tagGotoHere;
                }
            }

            //Thread.Sleep(2000);
            intTime = 0;
            while (strBrowserState.ToLower() != "complete" && intTime <= intTimeOut)
            {
                Thread.Sleep(1000);
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    strBrowserState = ((IJavaScriptExecutor)GenericClass.driver).ExecuteScript("return window.document.readyState").ToString();

                intTime += 1;
            }

            string strBrowserNavigatorState = "";
            if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                strBrowserNavigatorState = ((IJavaScriptExecutor)GenericClass.driver).ExecuteScript("return navigator.onLine").ToString().ToLower();

            if (strBrowserState.ToLower() != "complete" || strBrowserNavigatorState != "true")
            {
                DateTime dtFinal = DateTime.Now;
                TimeSpan ts = dtFinal.Subtract(dtInitial);
                string strTotalWaitTime = "";
                strTotalWaitTime = Convert.ToInt32(ts.TotalSeconds).ToString();
                GenericClass.dicOutput.Add("WaitForPageToLoad", Convert.ToString(strTotalWaitTime));
                return false;
            }
            return true;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : Back
        // Function Description : This function performs Browser back operation
        // Input Variable : 
        // OutPut : true / false
        // example : Browser.Back
        //---------------------------------------------------------------------------------------//
        public static bool Back()
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    GenericClass.driver.Navigate().Back();

                Browser.WaitForPageToLoad(240);
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : Navigate
        // Function Description : This function navigates the opened browser window to the passed url
        // Input Variable : Page URL
        // OutPut : true / false
        // example : Browser.Navigate("http://www/google.com")
        //---------------------------------------------------------------------------------------//
        public static bool Navigate(string strURL)
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    GenericClass.driver.Navigate().GoToUrl(strURL);
                Browser.WaitForPageToLoad(240);
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : Refresh
        // Function Description : This function performs Page Refresh operation
        // Input Variable : 
        // OutPut : true / false
        // example : Browser.Refresh
        //---------------------------------------------------------------------------------------//
        public static bool Refresh()
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    GenericClass.driver.Navigate().Refresh();
                Browser.WaitForPageToLoad(240);
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : maximize
        // Function Description : This function maximizes the opened browser window
        // Input Variable : 
        // OutPut : true / false
        // example : Browser.Maximize()
        //---------------------------------------------------------------------------------------//
        public static bool Maximize()
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    GenericClass.driver.Manage().Window.Maximize();
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : OpenNewWindow
        // Function Description : This function opens new browser window and navigates the page to the passed url
        // Input Variable : Page URL
        // OutPut : true / false
        // example : Browser.OpenNewWindow("http://www/google.com")
        //---------------------------------------------------------------------------------------//
        public static bool OpenNewWindow(string strBaseURL = "")
        {
            bool flag = false;
            if (strBaseURL == "")
                strBaseURL = GenericClass.dicConfig["strBrowserPath"];
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    //Opening new window through Javascript
                    ((IJavaScriptExecutor)GenericClass.driver).ExecuteScript("window.open('" + strBaseURL + "','NewWindow')");
                    GenericClass.driver.SwitchTo().Window("NewWindow");
                    Browser.Maximize();
                }
                Browser.WaitForPageToLoad();
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : SelectWindowByObject
        // Function Description : This function selects the browser window on which passed object is found
        // Input Variable : Object XPath
        // OutPut : true / false
        // example : Browser.SelectWindowByObject("//[@id='login']")
        //---------------------------------------------------------------------------------------//
        public static bool SelectWindowByObject(string strObjectXpath)
        {
            bool flag = false;
            bool isObjectFound = false;
            try
            {
                string strObject = objGeneric.GetLocatorXPathProperty(strObjectXpath);
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    foreach (string strWindowHandle in GenericClass.driver.WindowHandles)
                    {
                        GenericClass.driver.SwitchTo().Window(strWindowHandle);
                        if (GenericClass.driver.FindElements(By.XPath(strObject)).Count > 0)
                        {
                            isObjectFound = true;
                            break;
                        }
                        else
                            continue;
                    }
                }
                if (!isObjectFound)
                    throw new Exception("No object found with xpath " + strObject + " in any of the opened window.");
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : SelectWindow
        // Function Description : This function selects the browser window based on page title or creation time
        // Input Variable : Page Title or Creation time
        // OutPut : true / false
        // example : Browser.SelectWindow("Google") or Browser.SelectWindow("1")
        //---------------------------------------------------------------------------------------//
        public static bool SelectWindow(string strWindowTitleOrCreationTime)
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    GenericClass.driver.SwitchTo().Window(strWindowTitleOrCreationTime);
                    flag = true;
                }

            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : SelectOriginalWindow
        // Function Description : This function selects the original browser window whoose creation time is 0
        // Input Variable : 
        // OutPut : true / false
        // example : Browser.SelectOriginalWindow()
        //---------------------------------------------------------------------------------------//
        public static bool SelectOriginalWindow()
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    GenericClass.driver.SwitchTo().Window(null);

                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : WindowFocus
        // Function Description : This function focuses the selected browser window
        // Input Variable : 
        // OutPut : true / false
        // example : Browser.WindowFocus()
        //---------------------------------------------------------------------------------------//
        public static bool WindowFocus()
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    ((IJavaScriptExecutor)GenericClass.driver).ExecuteScript("window.focus()");
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : GetURL
        // Function Description : This function gets the page URL and saves it in TestGenericUtility.dicOutput["url"]
        // Input Variable : 
        // OutPut : true / false
        // example : Browser.GetURL()
        //---------------------------------------------------------------------------------------//
        public static string GetURL()
        {
            string strURL = "";
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    strURL = GenericClass.driver.Url;
            }
            catch (Exception e)
            {
                strURL = "";
                Err.Description = e.Message;
            }
            return strURL;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : VerifyTitle
        // Function Description : This function compares the actual page title with the passed string
        // Input Variable : Page Title
        // OutPut : true / false
        // example : Browser.VerifyTitle("Google")
        //---------------------------------------------------------------------------------------//
        public static bool VerifyTitle(string strWindowTitle)
        {
            bool flag = false;
            try
            {
                string strBrowserTitle = "";
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    strBrowserTitle = GenericClass.driver.Title;
                if (strBrowserTitle.Contains(strWindowTitle))
                    flag = true;
                else
                    flag = false;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : Close
        // Function Description : This function closes the page window.
        // Input Variable : 
        // OutPut : true / false
        // example : Browser.Close()
        //---------------------------------------------------------------------------------------//
        public static bool Close()
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    GenericClass.driver.Quit();
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : GetPageSource
        // Function Description : This function gets the page source and saves it in TestGenericUtility.dicOutput["PageSource"]
        // Input Variable : 
        // OutPut : true / false
        // example : Browser.GetPageSource()
        //---------------------------------------------------------------------------------------//
        public static bool GetPageSource()
        {
            bool flag = false;
            try
            {
                if (!GenericClass.dicOutput.ContainsKey("PageSource"))
                    GenericClass.dicOutput.Add("PageSource", "");
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    GenericClass.dicOutput["PageSource"] = GenericClass.driver.PageSource;
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }

        public static bool SelectFrame(string strFrameName)
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    if (strFrameName == null)
                        GenericClass.driver.SwitchTo().DefaultContent();
                    else
                        GenericClass.driver.SwitchTo().Frame(strFrameName);
                }
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }

        public static void SimpleWaitOf120Sec(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);
        }

        public static void SimpleWaitOf60Sec(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
        }

        public static void SimpleWaitOf10Sec(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public static void SimpleWaitOf5Sec(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public static void SimpleWaitOf1Sec(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        }

        public static void WaitForHTMLPageToLoadComplete(IWebDriver driver)
        {
            IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30.00));
            wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

    }


    public static class Err
    {
        public static string Description = null;
    }


    //Newly added code 

    #region New Class WebElementExtension Implementation - May 16th 2018
    public static class WebElementExtension
    {
        //----------------------------------------------------------------------------------------//
        // Function Name : WaitForElementLoad
        // Function Description : This function waits for a particular element in a page until timeout is reached. 120 seconds is default
        // Input Variable : 
        // OutPut : true / false
        // example : portalLoginpage.txtUserName.WaitForElementLoad(60);
        //---------------------------------------------------------------------------------------//

        //public static bool WaitForElementLoad(this IWebElement element, int iTimeout = 120)
        public static bool WaitForElementLoad(this IWebElement element)
        {
            bool flag = false;
            try
            {
                
                for (int i = 0; i <= 5; i++)
                {
                    if (element.Displayed || element.Location.X > 0 && element.Location.Y > 0)
                    {
                        flag = true;
                        break;
                    }
                    else
                    {                        
                        GenericClass.Wait(2);
                    }
                        
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                flag = false;
            }
            return flag;

        }

        //public static bool WaitForElementLoad1secs(this IWebElement element, int iTimeout = 1)
        public static bool WaitForElementLoad1secs(this IWebElement element)
        {
            bool flag = false;
            try
            {
                for (int i = 0; i <= 5; i++)
                {
                    if (element.Displayed || element.Location.X > 0 && element.Location.Y > 0)
                    {
                        flag = true;
                        break;
                    }
                    else
                    {
                        //System.Threading.Thread.Sleep(Convert.ToInt32(TestGenericUtility.dicConfig["intWait"]) * 1000);
                        GenericClass.Wait(2);
                    }
                        
                }
                //if (!flag)
                //{
                //    Err.Description = "OBJECT '" + element.Selected + ". Waited for " + iTimeout.ToString() + " seconds.";
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                flag = false;
            }
            return flag;
        }

        //public static bool WaitForElementLoad5secs(this IWebElement element, int iTimeout = 5)
        public static bool WaitForElementLoad5secs(this IWebElement element)
        {
            bool flag = false;
            try
            {
                for (int i = 0; i <= 10; i++)
                {
                    if (element.Displayed || element.Location.X > 0 && element.Location.Y > 0)
                    {
                        flag = true;
                        break;
                    }
                    else
                    {
                        //System.Threading.Thread.Sleep(Convert.ToInt32(TestGenericUtility.dicConfig["intWait"]) * 1000);
                        //System.Threading.Thread.Sleep(2000);
                        GenericClass.Wait(2);
                        //Thread.Sleep(2000);
                    }

                }
                //if (!flag)
                //{
                //    Err.Description = "OBJECT '" + element.Selected + ". Waited for " + iTimeout.ToString() + " seconds.";
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                flag = false;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : Click
        // Function Description : This function perform click operation on object passed to 'Element(string strObjectName)'
        // Input Variable : Optional (Timeout)
        // OutPut : true / false
        // example : Page("Google").Element("SearchButton").Click()
        //---------------------------------------------------------------------------------------//
        public static bool ClickElement(this IWebElement element, int intTimeout = 120)
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    try
                    {
                        if (GenericClass.dicConfig["strBrowserType"].ToLower() == "ie")
                        {
                            if (element.Location.X > 0 || element.Location.Y > 0)
                                ((IJavaScriptExecutor)GenericClass.driver).ExecuteScript("arguments[0].click();", element);
                            else
                                throw new OpenQA.Selenium.ElementNotVisibleException();
                        }
                        else
                            element.Click();
                    }
                    catch (OpenQA.Selenium.ElementNotVisibleException ex)
                    {
                        if (element.Location.X > 0 || element.Location.Y > 0)
                            ((IJavaScriptExecutor)GenericClass.driver).ExecuteScript("arguments[0].click();", element);
                        else
                            throw ex;
                    }
                }
                else
                {
                    element.Click();//TestGenericUtility.selenium.Click(strObjectProperty);
                }
                Browser.WaitForPageToLoad(intTimeout);
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : Click
        // Function Description : This function perform click operation on object passed to 'Element(string strObjectName)'
        // Input Variable : Optional (Timeout)
        // OutPut : true / false
        // example : Page("Google").Element("SearchButton").Click()
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// click an object through IJavaScriptExecutor
        /// </summary>
        /// <returns></returns>
        public static bool JSClickObject(this IWebElement element, int intTimeout = 120)
        {
            bool flag = false;
            try
            {
                //if (Err.Description != null)
                //    return false;
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    try
                    {
                        ((IJavaScriptExecutor)GenericClass.driver).ExecuteScript("arguments[0].click();", element);
                    }
                    catch (OpenQA.Selenium.ElementNotVisibleException ex)
                    {
                        //throw ex;
                        Err.Description = ex.Message;
                    }
                }
                else
                    element.Click();//TestGenericUtility.selenium.Click(strObjectProperty);

                Browser.WaitForPageToLoad(intTimeout);
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }

        public static bool ClickAtCenterCheck(this IWebElement element, int intTimeout = 120)
        {
            bool flag = false;
            try
            {
                if (Err.Description != null)
                    return false;
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    GenericClass g1 = new GenericClass();
                    g1.ExecuteJavascript("window.focus()");
                    if (GenericClass.dicConfig["strBrowserType"].ToLower().Contains("firefox"))
                    {
                        Browser.Maximize();
                        Browser.WindowFocus();
                        //Getting element location
                        int elementXoffset = element.Location.X;
                        int elementYoffset = element.Location.Y;
                        //Getting element width & heigth
                        int elementWidth = element.Size.Width;
                        int elementHeight = element.Size.Height;
                        //Getting offset for element's mid position
                        int elementMidXoffset = elementXoffset + (elementWidth / 2);
                        int elementMidYoffset = elementYoffset + (elementHeight / 2);
                        OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(GenericClass.driver);
                        action.MoveByOffset(elementMidXoffset, elementMidYoffset).ContextClick().Build().Perform();
                    }
                    else
                    {
                        OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(GenericClass.driver);
                        action.Click(element).Build().Perform();
                    }
                }
                else
                    element.Click();//TestGenericUtility.selenium.Click(strObjectProperty);
                Browser.WaitForPageToLoad(intTimeout);
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }

        public static bool ExistValidate(this IWebElement element)
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    if (element.Displayed || (element.Location.X > 0 && element.Location.Y > 0))
                    {
                        flag = true;
                    }
                    else
                    {
                        throw new Exception("OBJECT - " + element.TagName + " is not visible on PAGE");
                    }
                }
                else
                {
                    if (element.Location.X > 0 && element.Location.Y > 0)
                    {
                        if (element.Displayed)
                            flag = true;
                        else
                            throw new Exception("OBJECT - " + element.TagName + " is not visible on PAGE");
                    }
                    else
                        throw new Exception("OBJECT - " + element.TagName + " is not found on PAGE ");
                }
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }



        //----------------------------------------------------------------------------------------//
        // Function Name : getRoProperty
        // Function Description : This function gets the run time property 
        // Input Variable : ""
        // OutPut : String 
        // example : Page("Google").Element("SearchBox").getRoProperty("class")
        //---------------------------------------------------------------------------------------//
        public static string getRunTimeProperty(this IWebElement element, string strAttributeNameOrTagnameOrLocation)
        {
            string strRoObjectProperty = "";
            try
            {
                switch (strAttributeNameOrTagnameOrLocation.ToLower())
                {
                    case "text":
                        {
                            strRoObjectProperty = element.Text;
                            break;
                        }
                    case "tagname":
                        {
                            strRoObjectProperty = element.TagName;
                            break;
                        }

                    case "location":
                        {
                            strRoObjectProperty = element.Location.X.ToString() + "," + element.Location.Y.ToString();
                            break;
                        }
                    default:
                        strRoObjectProperty = element.GetAttribute(strAttributeNameOrTagnameOrLocation);
                        return strRoObjectProperty;
                }

            }
            catch (Exception e)
            {
                Err.Description = e.Message;
            }

            return strRoObjectProperty;
        }

        /// <summary>
        /// Switch to Frame though OR objects
        /// </summary>
        /// <returns></returns>

        public static bool SwitchFrameWindow(this IWebElement element)
        {
            bool flag = false;
            try
            {
                GenericClass.driver.SwitchTo().Frame(element);
                flag = true;
            }
            catch (Exception e)
            {
                Err.Description = e.Message;
            }

            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : Select
        // Function Description : This function types into input box object passed to 'Element(string strObjectName)'
        // Input Variable : String List Value
        // OutPut : true / false
        // example : Page("Google").Element("ListCountry").Select("test")
        //---------------------------------------------------------------------------------------//
        public static bool SelectDropdown(this IWebElement element, string strListValue)
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    try
                    {
                        SelectElement select = new SelectElement(element);
                        select.SelectByText(strListValue);
                        flag = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        if (GenericClass.dicConfig["strBrowserType"].ToLower() == "ie")
                        {
                            if (element.Location.X > 0 || element.Location.Y > 0)
                            {
                                ((IJavaScriptExecutor)GenericClass.driver).ExecuteScript("arguments[0].click();", element);
                                Thread.Sleep(300);
                                element.FindElement(By.XPath("//*[text()='" + strListValue + "']")).Click();
                            }
                            else
                                throw new OpenQA.Selenium.ElementNotVisibleException();
                        }
                        else
                        {
                            element.Click();
                            Thread.Sleep(300);
                            element.FindElement(By.XPath("//*[text()='" + strListValue + "']")).Click();

                        }

                    }
                }

            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }


        //----------------------------------------------------------------------------------------//
        // Function Name : Type
        // Function Description : This function types into input box object passed to 'Element(string strObjectName)'
        // Input Variable : String Keyword
        // OutPut : true / false
        // example : Page("Google").Element("SearchBox").Type("test")
        //---------------------------------------------------------------------------------------//
        public static bool TypeData(this IWebElement element, string strKeyword)
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    //Uncomment this code for execution on safari
                    try
                    {
                        if (GenericClass.dicConfig["strBrowserType"].ToLower() == "ie")
                        {
                            if (element.Location.X > 0 || element.Location.Y > 0)
                            {
                                //((IJavaScriptExecutor)TestGenericUtility.driver).ExecuteScript("arguments[0].value='" + strKeyword + "'", element);
                                try
                                {
                                    element.Clear();
                                }
                                catch (Exception e) { Console.WriteLine(e.Message); };
                                element.SendKeys(strKeyword);
                            }
                            else
                                throw new OpenQA.Selenium.ElementNotVisibleException();

                        }
                        else
                        {
                            try
                            {
                                element.Clear();
                            }
                            catch (Exception e) { Console.WriteLine(e.Message); };

                            element.SendKeys(strKeyword);
                        }
                    }
                    catch (StaleElementReferenceException ex)
                    {
                        Console.WriteLine(ex.Message);
                        element.Click();
                        //InputSimulator.SimulateTextEntry(strKeyword);
                        //InputSimulator
                    }
                    catch (OpenQA.Selenium.InvalidElementStateException ex)
                    {
                        if (element.Location.X > 0 || element.Location.Y > 0)
                            ((IJavaScriptExecutor)GenericClass.driver).ExecuteScript("arguments[0].value='" + strKeyword + "'", element);
                        else
                            throw ex;
                    }
                }

            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }

        public static bool isElementSelected(this IWebElement element)
        {
            bool flag = false;
            try
            {
                //if (Err.Description != null)
                //    return false;
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    flag = element.Selected;

            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }

        /*''@##########################################################################################################################
           ''@Function ID: 
           ''@Function Name: PerformActionOnAlert
           ''@Objective: This functions perform action on alert
           ''@---------------------------------------------------------------------------------------------------------------------------
           ''@Param Name: strOutText (Output)
           ''@Param Desc: Output parameter returns alert text
           ''@Param Name: action          
           ''@Param Desc: perform action like Click "OK", "Cancel" and "GetText"
           ''@---------------------------------------------------------------------------------------------------------------------------
           ''@Return Desc: 
           ''@     Success - True
           ''@     Failure - False
           ''@---------------------------------------------------------------------------------------------------------------------------
           ''@Example: blnStatus= PerformActionOnAlert("Ok");
           ''@---------------------------------------------------------------------------------------------------------------------------
           ''@Created by[Date]: Kamlesh Kumar Yadav [June 16th, 2015]
           ''@---------------------------------------------------------------------------------------------------------------------------
           ''@Reviewed by[Date]: 
           ''@---------------------------------------------------------------------------------------------------------------------------
           ''@History Notes: 
           ''@---------------------------------------------------------------------------------------------------------------------------
           ''@########################################################################################################################### */

        //----------------------------------------------------------------------------------------//
        // Function Name : isSelected
        // Function Description : This function verifies for selected radio or list object passed to 'Element(string strObjectName)'
        // Input Variable : 
        // OutPut : true / false
        // example : Page("Google").Element("chkbox1").isSelected()
        //---------------------------------------------------------------------------------------//
        public static bool isSelectedItem(this IWebElement element)
        {
            bool flag = false;
            try
            {
                if (Err.Description != null)
                    return false;
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    flag = element.Selected;
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }

        public static String GetText(this IWebElement element)
        {
            string strElementText = "";
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    strElementText = element.Text;
            }
            catch (Exception e)
            {
                strElementText = "";
                Err.Description = e.Message;
            }
            return strElementText;


        }

        //----------------------------------------------------------------------------------------//
        // Function Name : MouseHover
        // Function Description : This function perform mouse hover operation on object passed to 'Element(string strObjectName)'
        // Input Variable : 
        // OutPut : true / false
        // example : Page("Google").Element("SearchButton").MouseHover()
        //---------------------------------------------------------------------------------------//
        public static bool MouseHover(this IWebElement elementToHover)
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    OpenQA.Selenium.Interactions.Actions builder = new OpenQA.Selenium.Interactions.Actions(GenericClass.driver);
                    builder.MoveToElement(elementToHover).Build().Perform();
                    flag = true;
                }
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }

        public static bool MouseHoverAndClick(this IWebElement elementToHover, IWebElement elementToClick)
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    OpenQA.Selenium.Interactions.Actions builder = new OpenQA.Selenium.Interactions.Actions(GenericClass.driver);
                    builder.MoveToElement(elementToHover).Click(elementToClick).Build().Perform();
                    flag = true;
                }
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }

        public static bool ClickAfterMouseHover(this IWebElement elementToClick)
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    OpenQA.Selenium.Interactions.Actions builder = new OpenQA.Selenium.Interactions.Actions(GenericClass.driver);
                    //builder.MoveToElement(elementToHover).Click(elementToClick).Build().Perform();
                    builder.Click(elementToClick).Build().Perform();
                    flag = true;
                }
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }

        public static bool DragAndDropElements(this IWebElement elementDragFrom, IWebElement elementDragTo)
        {
            bool flag = false;
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    OpenQA.Selenium.Interactions.Actions builder = new OpenQA.Selenium.Interactions.Actions(GenericClass.driver);
                    //builder.MoveToElement(elementToHover).Click(elementToClick).Build().Perform();
                    builder.ClickAndHold(elementDragFrom).MoveToElement(elementDragTo).Release(elementDragTo).Build().Perform();
                    flag = true;
                }
            }
            catch (Exception e)
            {
                flag = false;
                Err.Description = e.Message;
            }
            return flag;
        }
    }
    #endregion
}
