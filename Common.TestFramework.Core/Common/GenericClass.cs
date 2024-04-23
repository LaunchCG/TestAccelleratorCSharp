using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Data;
using System.Xml;
using System.Linq;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Collections;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Common.TestFramework.Core
{


    public class GenericClass
    {
        private static DataSet _dataSet;

        public static IWebDriver driver;
        public static string strBrowser = "";
        public static string applicationURL = "";
        public static string strObject = "";
        public static string ExcelTimeStamp = "";


        public static int pass = 0;
        public static int fail = 0;


        public static StringBuilder verificationErrors;
        public static TimeSpan DDiff;
        public static Dictionary<string, string> dicTestData = new Dictionary<string, string>();
        public static Dictionary<string, string> dicDBValue = new Dictionary<string, string>();
        public static Dictionary<string, string> dicKBTestData = new Dictionary<string, string>();
        public static Dictionary<string, string> dicDict = new Dictionary<string, string>();
        public static Dictionary<string, string> dicDictNew = new Dictionary<string, string>();
        public static Dictionary<string, string> dicOutput = new Dictionary<string, string>();
        public static Dictionary<string, string> dicMaster = new Dictionary<string, string>();
        public static Dictionary<string, string> dicQuery = new Dictionary<string, string>();
        public static Dictionary<string, string> dicInput = new Dictionary<string, string>();
        public static Dictionary<string, string> dicApplicationUrl = new Dictionary<string, string>();
        public static Dictionary<string, int> dicCurrentIteartion = new Dictionary<string, int>();
        public static Dictionary<string, int> dicCoordinate = new Dictionary<string, int>();
        public static Dictionary<string, string> dicTestScriptResult = new Dictionary<string, string>();
        public static string[] sObject;
        public static Excel.Application myExcel;
        public static Dictionary<string, string> dicReporting = new Dictionary<string, string>();
        public static Dictionary<string, string> dicConfig = new Dictionary<string, string>();
        //dicConfigNew
        public static Dictionary<string, string> dicConfigNew = new Dictionary<string, string>();
        //public static Dictionary<string, string> dicDetailedReport = new Dictionary<string, string>();
        public static StreamWriter oReportWriter;
        public static int iStepCount;
        public static DateTime sStartDate;
        public static DateTime sStepStartTime;
        public static int iPassStepCount;
        public static int iFailStepCount;
        public static int iScriptCount = 1;
        public static string sCurrentDir;
        public static string sCurrentExePath;

        public static string strWindowText;
        public static IntPtr intpWindowHdl;
        public static int index;
        public static int index1;
        public static int IntIteration;
        public string strMasterTestDataFile;
        public static Dictionary<int, Dictionary<string, string>> TestProviderObject = new Dictionary<int, Dictionary<string, string>>();



        public class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [DllImport("user32.dll")]
            public static extern IntPtr SetForegroundWindow(IntPtr hWnd);
            public delegate bool EnumWindowProc(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern bool EnumChildWindows(IntPtr hwnd, EnumWindowProc func);
            [DllImport("user32.dll")]
            public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        }

        public string ExecuteJavascript(string strJavascript)
        {

            string strJavascriptReturnValue = "";
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                    strJavascriptReturnValue = ((IJavaScriptExecutor)GenericClass.driver).ExecuteScript("return " + strJavascript).ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                strJavascriptReturnValue = null;
            }
            return strJavascriptReturnValue;
        }

        public string GetLocatorXPathProperty(string strObjectDesc)
        {

            string strAttr = strObjectDesc.Substring(0, 4);
            string strAttrValue = strObjectDesc.Substring(4);
            string strXPathProperty = "";

            switch (strAttr)
            {
                case "idef":
                    strXPathProperty = "@id='" + strAttrValue + "'";
                    break;
                case "txtp":
                    strXPathProperty = "//*[contains(text(),'" + strAttrValue + "')" + "]";
                    break;
                case "txtf":
                    strXPathProperty = "//*[text()='" + strAttrValue + "']";
                    break;

                case "imgs":
                    strXPathProperty = "contains(@src,'" + strAttrValue + "')";
                    break;
                case "idep":
                    strXPathProperty = "contains(@id,'" + strAttrValue + "')";
                    break;
                case "clsf":
                    strXPathProperty = "@class='" + strAttrValue + "'";
                    break;
                case "clsp":
                    strXPathProperty = "contains(@class,'" + strAttrValue + "')";
                    break;
                case "altf":
                    strXPathProperty = "@alt='" + strAttrValue + "'";
                    break;
                case "altp":
                    strXPathProperty = "contains(@alt,'" + strAttrValue + "')";
                    break;
                case "href":
                    strXPathProperty = "@href='" + strAttrValue + "'";
                    break;
                case "hrep":
                    strXPathProperty = "contains(@href,'" + strAttrValue + "')";
                    break;
                case "valf":
                    strXPathProperty = "@value='" + strAttrValue + "'";
                    break;
                case "valp":
                    strXPathProperty = "contains(@value,'" + strAttrValue + "')";
                    break;
                case "namf":
                    strXPathProperty = "@name='" + strAttrValue + "'";
                    break;
                case "namp":
                    strXPathProperty = "contains(@name,'" + strAttrValue + "')";
                    break;
                case "srcf":
                    strXPathProperty = "@src='" + strAttrValue + "'";
                    break;
                case "srcp":
                    strXPathProperty = "contains(@src,'" + strAttrValue + "')";
                    break;
                case "typf":
                    strXPathProperty = "@type='" + strAttrValue + "'";
                    break;
                case "typp":
                    strXPathProperty = "contains(@type,'" + strAttrValue + "')";
                    break;
                case "oncf":
                    strXPathProperty = "@onclick='" + strAttrValue + "'";
                    break;
                case "oncp":
                    strXPathProperty = "contains(@onclick,'" + strAttrValue + "')";
                    break;
                case "lnkf":
                    strXPathProperty = "link=" + strAttrValue;
                    break;
                case "lftf":
                    strXPathProperty = "//div[contains(@class,'left')]//*[contains(text(),'" + strAttrValue + "')]";
                    break;
                case "lftd":
                    strXPathProperty = "//div[@id='id_dept_list']//*[contains(text(),'" + strAttrValue + "')]";
                    break;
                case "cssf":
                    strXPathProperty = "css=" + strAttrValue;
                    break;
                default:
                    //strXPathProperty = strAttrValue;
                    return strObjectDesc;
            }

            if (strAttr == "cssf" || strAttr == "lnkf" || strAttr == "txtf" || strAttr == "lftf" || strAttr == "lftd")
                return strXPathProperty;
            else
                return "//*[" + strXPathProperty + "]";
        }






        //----------------------------------------------------------------------------------------//
        // Function Name : TeardownTest
        // Function Description : This function is used to close selenium server
        // Input Variable : none
        // OutPut : void
        // example : TeardownTest()
        //---------------------------------------------------------------------------------------//
        public void TeardownTest()
        {
            try
            {
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    if (GenericClass.driver != null)
                        GenericClass.driver.Quit();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //do nothing
            }
            finally
            {
                switch (dicConfig["strBrowserType"].ToLower())
                {
                    case "firefox":
                        KillObjectInstances("FIREFOX");
                        break;
                    case "iexplore":
                        KillObjectInstances("IEXPLORE");
                        KillObjectInstances("IEXPLORE.EXE");
                        KillObjectInstances("IEXPLORE.EXE *32");
                        break;
                    case "googlechrome":
                        KillObjectInstances("CHROME");
                        break;

                }
                KillObjectInstances("EXCEL");
                GenericClass.Wait(5);
            }
        }




        //----------------------------------------------------------------------------------------//
        // Function Name : GetExecutionData
        // Function Description : This function is used to get execution data files and convert them to there respective xml's
        // Input Variable : none
        // OutPut : void
        // example : GetExecutionData()
        //---------------------------------------------------------------------------------------//

        public void GetExecutionData()
        {
            OleDbConnection oleDbCon = null;
            OleDbDataAdapter dataAdapter = null;
            DataSet dataSet = null;
            try
            {
                //MessageBox.Show(dicConfig["frameworkconfig"]);
                //Getting data from Config.xls
                oleDbCon = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dicConfig["frameworkconfig"] + ";Extended Properties=Excel 12.0");
                oleDbCon.Open();
                dataSet = new DataSet();
                dataAdapter = new OleDbDataAdapter("SELECT * FROM [Config$]", oleDbCon);
                dataAdapter.Fill(dataSet);
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    if (dicConfig.ContainsKey(dataSet.Tables[0].Rows[i].ItemArray[0].ToString()))
                    {
                        //Handling case for Local copy of MasterTestData. If value is not blank, pick the local copy
                        if (dataSet.Tables[0].Rows[i].ItemArray[0].ToString().Contains("MasterTestData"))
                        {
                            if (dataSet.Tables[0].Rows[i].ItemArray[1].ToString() != "")
                                if (dataSet.Tables[0].Rows[i].ItemArray[1].ToString().Contains("\\"))
                                    dicConfig["MasterTestDataPath"] = dataSet.Tables[0].Rows[i].ItemArray[1].ToString();
                                else
                                    dicConfig["MasterTestDataPath"] = dicConfig["MasterTestDataPath"] + dataSet.Tables[0].Rows[i].ItemArray[1].ToString();
                        }
                        else
                            dicApplicationUrl[dataSet.Tables[0].Rows[i].ItemArray[0].ToString()] = dataSet.Tables[0].Rows[i].ItemArray[1].ToString();
                    }
                    else
                        dicConfig.Add(dataSet.Tables[0].Rows[i].ItemArray[0].ToString(), dataSet.Tables[0].Rows[i].ItemArray[1].ToString());
                }

                if (!dicConfig.ContainsKey("TestSuiteFileName"))
                    dicConfig.Add("TestSuiteFileName", "TestSuite");

                if (!dicConfig["MasterTestDataPath"].Contains(".xls"))  
                    dicConfig["MasterTestDataPath"] = dicConfig["MasterTestDataPath"] + "MasterTestData.xls";

                dataSet.Clear();
                dataAdapter.Dispose();
                oleDbCon.Close();


                //Code to configure a remote path for saving reports
                if (dicConfig.ContainsKey("PathToSaveReport"))
                {
                    if (dicConfig["PathToSaveReport"].Length != 0)
                        dicConfig["ReportPath"] = dicConfig["PathToSaveReport"];
                }

                string[] strMTD = dicConfig["MasterTestDataPath"].Split('\\');

                if (!dicConfig.ContainsKey("MasterTestData"))
                    dicConfig.Add("MasterTestData", strMTD[strMTD.Length - 1]);

                //strMasterTestDataFile = strMTD[strMTD.Length-1];

                //Getting data from 'ApplicationDetails' screen of Datasheet
                oleDbCon = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dicConfig["MasterTestDataPath"] + ";Extended Properties=Excel 12.0");
                oleDbCon.Open();
                dataSet = new DataSet();
                dataAdapter = new OleDbDataAdapter("SELECT * FROM [ApplicationDetails$]", oleDbCon);
                dataAdapter.Fill(dataSet);
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    if (dicApplicationUrl.ContainsKey(dataSet.Tables[0].Rows[i].ItemArray[0].ToString()))
                        dicApplicationUrl[dataSet.Tables[0].Rows[i].ItemArray[0].ToString()] = dataSet.Tables[0].Rows[i].ItemArray[1].ToString();
                    else
                        dicApplicationUrl.Add(dataSet.Tables[0].Rows[i].ItemArray[0].ToString(), dataSet.Tables[0].Rows[i].ItemArray[1].ToString());
                }
                dataSet.Clear();
                dataAdapter.Dispose();

                oleDbCon.Close();

            }
            catch (Exception e)
            {

                Console.WriteLine("Exception occurred: " + e.ToString());
                try
                {
                    dataSet.Clear();
                    dataAdapter.Dispose();
                    oleDbCon.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occurred: " + ex.Message);
                }
            }

            //ReadTestSuiteandChangetoXML(dicConfig["TestSuitePath"], "TestSuite", "TestSuite", "select * from [strSheetName] where Run='Y'");

            if (Directory.Exists(dicConfig["ReportPath"] + "LastRun\\"))
            {
                //try
                //{
                //    foreach (string file in Directory.GetFiles(dicConfig["ReportPath"] + "LastRun\\", "*.html").Where(item => item.EndsWith(".html")))
                //        File.Delete(file);
                //    foreach (string file in Directory.GetFiles(dicConfig["ReportPath"] + "LastRun\\", "*.png").Where(item => item.EndsWith(".png")))
                //        File.Delete(file);
                //}
                //catch (Exception e)
                //{

                //}
            }
            else
                Directory.CreateDirectory(dicConfig["ReportPath"] + "LastRun\\");


            //if already app config data is loaded then dont load it again
            if (GenericClass.dicConfig.ContainsKey("ScreenShotPath"))
            {
                return;
            }

            dicConfig.Add("ScreenShotPath", dicConfig["ReportPath"] + "LastRun\\");



        }

        //----------------------------------------------------------------------------------------//
        // Function Name : ExcelDBConnection
        // Function Description : This function is used to create Db connection with an excel file
        // Input Variable : sExcelPath
        // Input Description : Path of the excel
        // Input type : string
        // Input Variable : sExcelName
        // Input Description : Name of the excel file
        // Input type : string
        // OutPut : returns an open connection to a data source
        // example : ExcelDBConnection("c:\seleniumproject\TestData" , "MasterTestData")
        //---------------------------------------------------------------------------------------//

        public OleDbConnection ExcelDBConnection(string sExcelPath, string sExcelName)
        {
            OleDbConnection ole1 = null;

            try
            {
                ole1 =
                new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sExcelPath + sExcelName +
                                    ";Extended Properties=Excel 12.0");
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return ole1;

        }


        public void KillObjectInstances(string ProcessName)
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


        public void GetAppConfigData(int iLen)
        {
            GenericClass.dicReporting.Add("StepStatus", "fail");
            KillObjectInstances("EXCEL");
            KillObjectInstances("javaw");
            sCurrentDir = "";
            if (iLen == 0 || iLen == 1)
            {
                var sIndexPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString().IndexOf(@"bin");
                
                sCurrentDir = sIndexPath == -1
                    ? AppDomain.CurrentDomain.BaseDirectory
                    : System.AppDomain.CurrentDomain.BaseDirectory.Substring(0, sIndexPath);
                
                sCurrentExePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\";
            }
            else
            {
                string sCommandLine = Environment.CommandLine;
                sCommandLine = sCommandLine.Substring(0, sCommandLine.IndexOf("RunManager.exe"));
                sCurrentDir = sCommandLine.Substring(1, sCommandLine.IndexOf("RunManager") - 1);
                sCurrentExePath = sCommandLine;

            }

            string[] arrAllKeys = System.Configuration.ConfigurationManager.AppSettings.AllKeys;

            //if already app config data is loaded then dont load it again
            if (GenericClass.dicConfig.ContainsKey("frameworkconfig"))
            {
                return;
            }

            foreach (string item in arrAllKeys)
            {
                if (item == "frameworkconfig" || item == "ExcelWorkBookName" || item == "QCIntegration" || item == "LibFiles" || item == "ProjectName")
                    dicConfig.Add(item, System.Configuration.ConfigurationManager.AppSettings[item]);
                else
                {
                    dicConfig.Add(item, sCurrentDir +
                        (string.IsNullOrEmpty(ConfigurationManager.AppSettings[item])
                        ? string.Empty
                        : ConfigurationManager.AppSettings[item] + "\\"));
                    //dicConfig.Add(item, sCurrentDir + System.Configuration.ConfigurationManager.AppSettings[item] + "\\");
                }
            }

            if (!File.Exists(dicConfig["frameworkconfig"]))
                dicConfig["frameworkconfig"] = dicConfig["MasterTestDataPath"] + "Config.xls";

            dicConfig.Add("ServerPath", System.AppDomain.CurrentDomain.BaseDirectory);

        }
        public void AddTestDataforApplication(int Iteration, string AcessMethod = "")
        {
            Iteration = 1;
            OleDbConnection oleDbCon;
            OleDbDataAdapter da;
            DataSet ds;
            //oleDbCon = ExcelDBConnection(dicConfig["MasterTestDataPath"], "MasterTestData.xls");
            //Console.WriteLine("Master Test Data " + dicConfig["MasterTestData"] + " And Master Test Data Path " + dicConfig["MasterTestDataPath"]);
            oleDbCon = ExcelDBConnection(dicConfig["MasterTestDataPath"].Replace(dicConfig["MasterTestData"], ""), dicConfig["MasterTestData"]);
            oleDbCon.Open();


            //da = new OleDbDataAdapter("SELECT count(*)  FROM [" + dicDict["Folder"] + "$] WHERE TESTCASENAME='" + dicDict["TestCaseName"] + "'", oleDbCon);

            da = new OleDbDataAdapter("SELECT * FROM [" + dicDict["Folder"] + "$] WHERE TESTCASENAME='" + dicDict["TestCaseName"] + "'", oleDbCon);
            //da = new OleDbDataAdapter("SELECT * FROM [Checkout$] WHERE TESTCASENAME='SnapDeal_CkeckOut_ValidProduct_Pass'", oleDbCon);
            ds = new DataSet();


            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 0)
                return;

            //Test data getting read from the excel sheet for 2nd time. Placed into dicTestData

            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                if (dicTestData.ContainsKey(ds.Tables[0].Columns[i].ColumnName.ToString()))
                    dicTestData[ds.Tables[0].Columns[i].ColumnName.ToString()] = ds.Tables[0].Rows[Iteration - 1].ItemArray[i].ToString();
                else
                    dicTestData.Add(ds.Tables[0].Columns[i].ColumnName.ToString(), ds.Tables[0].Rows[Iteration - 1].ItemArray[i].ToString());

            }


            ds.Clear();
            da.Dispose();
            oleDbCon.Dispose();
            oleDbCon.Close();

        }

        public List<Dictionary<string, string>> AddTestDataforApplicationNew(string testCaseName)
        {
            var sIndexPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString().IndexOf(@"bin");
            var sCurrentDir = sIndexPath == -1 ? AppDomain.CurrentDomain.BaseDirectory : System.AppDomain.CurrentDomain.BaseDirectory.Substring(0, sIndexPath);
            dicConfigNew.Add("DataSheetPath", string.Format("{0}{1}\\", sCurrentDir, ConfigurationManager.AppSettings["MasterTestDataPath"]));
            dicConfigNew["ConfigExcelFile"] = dicConfigNew["DataSheetPath"] + "Config.xls";

            OleDbConnection oleDbCon1 = null;
            OleDbDataAdapter dataAdapter = null;
            DataSet dataSet = null;

            //Getting data from Config.xls
            oleDbCon1 = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dicConfigNew["ConfigExcelFile"] + ";Extended Properties=Excel 12.0");
            oleDbCon1.Open();
            dataSet = new DataSet();
            dataAdapter = new OleDbDataAdapter("SELECT * FROM [Config$]", oleDbCon1);
            dataAdapter.Fill(dataSet);

            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                if (dataSet.Tables[0].Rows[i].ItemArray[0].ToString().Contains("MasterTestData"))
                {
                    dicConfigNew["MasterTestDataPath"] = dataSet.Tables[0].Rows[i].ItemArray[1].ToString();
                    dicConfigNew["MasterTestDataPath"] = dicConfigNew["DataSheetPath"] + dicConfigNew["MasterTestDataPath"];
                    break;
                }

            }

            dataSet.Clear();
            dataAdapter.Dispose();
            oleDbCon1.Close();

            FormatTestCaseName(testCaseName);

            return GetTestDataforTestCase();
        }


        private void FormatTestCaseName(string testCaseName)
        {
            string[] strValue = testCaseName.Split('_');
            dicDictNew["TestSuiteName"] = testCaseName.Split('_')[0].Trim();
            dicDictNew["TCName"] = testCaseName.Remove(0, strValue[0].Count() + 1);
        }

        public List<Dictionary<string, string>> GetTestDataforTestCase()
        {
            if (_dataSet == null)
            {
                _dataSet = LoadTestDataToDataset();
            }


            var dataRows = (from data in _dataSet.Tables[GenericClass.dicDictNew["TestSuiteName"] + "$"].AsEnumerable()
                            where data.Field<string>("TestcaseName") == GenericClass.dicDictNew["TCName"]
                            select data);

            var testData = new List<Dictionary<string, string>>();

            foreach (var dataRow in dataRows)
            {
                var dataDictionary = dataRow.Table.Columns
                    .Cast<DataColumn>()
                    .ToDictionary(c => c.ColumnName, c => dataRow[c].ToString());

                testData.Add(dataDictionary);
            }
            GenericClass.dicConfigNew.Clear();
            return testData;
        }

        private static DataSet LoadTestDataToDataset()
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dicConfigNew["MasterTestDataPath"] +
                                  ";Extended Properties=Excel 12.0";

            var TestDataSet = new DataSet();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                DataTable schemaTable = connection.GetOleDbSchemaTable(
                    OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                foreach (DataRow schemaRow in schemaTable.Rows)
                {
                    string sheet = schemaRow["TABLE_NAME"].ToString();

                    if (!sheet.EndsWith("_"))
                    {
                        try
                        {
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet + "]", connection);
                            cmd.CommandType = CommandType.Text;

                            DataTable dataTable = new DataTable(sheet);
                            TestDataSet.Tables.Add(dataTable);
                            new OleDbDataAdapter(cmd).Fill(dataTable);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message + string.Format("Sheet:{0}.File:F{1}", sheet, dicConfigNew["MasterTestDataPath"]), ex);
                        }
                    }
                }
            }

            return TestDataSet;
        }

        public static void Wait(int intTimeSeconds = 1)
        {
            try
            {
                Thread.Sleep(intTimeSeconds * 1000);
            }
            catch (Exception e) { Console.WriteLine(e.Message); };
        }

        public static bool DefaultContent()
        {
            bool flag = false;

            try
            {
                GenericClass.driver.SwitchTo().DefaultContent();
                flag = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return flag;
        }

        public static void SendSummaryStatusMail(String SummaryAttachmentPath, int totalExcecuted, int totalPassed, int totalFailed, int totalNoRun)
        {

            //@ Data fetched from config file to send Email
            string smtpPlatform = dicConfig["smtpServer"];
            string fromEmailAddress = dicConfig["fromEmailAddress"];
            string fromEmailPassword = dicConfig["fromEmailPassword"];
            //string toEmailAddress = dicConfig["toEmailAddress"];
            string toEmailAddress = "sanbalagan@launchcg.com";
            string smtpClient = dicConfig["smtpClient"];
            int smtpPort = Convert.ToInt32(dicConfig["smtpPort"]);
            //string mailSubject = dicConfig["mailSubject"];
            string mailSubject = dicConfig["strEnvironment"] + " - " + dicConfig["ProjectName"] + " " + "Automation Summary Report" + " (" + dicConfig["TestProjectName"] + ")";
            //string mailBody = string.Empty;

            string mailBody = "<font face=\"Calibri\" size=\"2\">Dear all,<br><br>Please find below the Automation Execution Summary.<br><br>" +
                    "<font face=\"Calibri\"" +
                            "<table border=2>" + //<table border=\"2\">
                            "<th border=\"2\" colspan=\"4\" bgcolor=\"#d3d3d3\" font=\"Calibri\" size=\"2\">Automation Execution Status</th>" +
                            " <tr> " +
                                "<th align=\"Center\" font=\"Calibri\" size=\"2\">Test Suite</th> " +
                                "<th align=\"Center\" font=\"Calibri\" size=\"2\">Passed</th>  " +
                                "<th align=\"Center\" font=\"Calibri\" size=\"2\">Failed</th> " +
                                "<th align=\"Center\" font=\"Calibri\" size=\"2\">Total</th> " +
                            "</tr> " +
                        "<tr>" +
                            //"<td align=\"Center\" font=\"Calibri\">" + dicReporting["tempFolder"] + "</td> " +
                            //"<td align=\"Center\" font=\"Calibri\">" + TestGenericUtility.dicDict["TSName"] + "</td> " +
                            "<td align=\"Center\" font=\"Calibri\" size=\"2\">" + GenericClass.dicConfig["TestProjectName"] + "</td> " +
                            "<td align=\"Center\" font=\"Calibri\" size=\"2\">" + totalPassed + "</td> " +
                            "<td align=\"Center\" font=\"Calibri\" size=\"2\">" + totalFailed + "</td>" +
                            "<td align=\"Center\" font=\"Calibri\" size=\"2\">" + totalExcecuted + "</td>" +
                            "</tr>" +
                        "<tr>" +
                            "<td align=\"Center\" font=\"Calibri\" size=\"2\">" + "Total" + "</td> " +
                            "<td align=\"Center\" font=\"Calibri\" size=\"2\">" + totalPassed + "</td> " +
                            "<td align=\"Center\" font=\"Calibri\" size=\"2\">" + totalFailed + "</td>" +
                            "<td align=\"Center\" font=\"Calibri\" size=\"2\">" + totalExcecuted + "</td>" +
                            "</tr>" +
                        "</table><font>" +
                        "<br><br>Thanks,<br>RequestWeb Automation Team<br><br>Note:- This is auto generated message and please do not respond.</font>";
            //}

            mailBody = "<HTML><BODY><TABLE BORDER=.1>" + mailBody + "</TABLE></BODY></HTML>";

            try
            {
                if (smtpPlatform.ToLower().Contains("gmail"))
                {
                    //Mail Message
                    MailMessage mM = new MailMessage();
                    //Mail Address
                    mM.From = new MailAddress(fromEmailAddress);
                    //receiver email id
                    mM.To.Add(toEmailAddress);

                    //subject of the email
                    mM.Subject = mailSubject;
                    //deciding for the attachment
                    mM.Attachments.Add(new Attachment(SummaryAttachmentPath));
                    //mM.Attachments.Add(new Attachment(DetailedReport));
                    //add the body of the email
                    mM.Body = mailBody;
                    mM.IsBodyHtml = true;
                    //SMTP client
                    SmtpClient sC = new SmtpClient(smtpClient);                    
                    sC.UseDefaultCredentials = false;
                    //port number for Gmail mail
                    sC.Port = smtpPort;
                    //credentials to login in to Gmail account
                    sC.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
                    //enabled SSL
                    sC.EnableSsl = true;
                    //Send an email
                    sC.Send(mM);
                    Console.WriteLine("Summary Mail Sent Successfully");

                }//end of try block
                else if (smtpPlatform.ToLower().Contains("outlook"))
                {
                    //Console.WriteLine("Outlook");
                    // mail server
                    SmtpClient smtp = new SmtpClient(smtpClient, smtpPort); //specify the mail server
                    // credentials
                    if (!String.IsNullOrEmpty(fromEmailAddress))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential(fromEmailAddress, fromEmailPassword);
                    }

                    //Console.WriteLine("toEmailAddress :-" + toEmailAddress);
                    string[] toEmails = toEmailAddress.Split(',');
                    MailAddress to = new MailAddress(toEmails[0]);
                    MailAddress from = new MailAddress(fromEmailAddress);
                    //Console.WriteLine("fromEmailAddress :-" + fromEmailAddress);

                    MailMessage message = new MailMessage(from, to);
                    for (int i = 1; i <= toEmails.Length - 1; i++)
                    {
                        message.To.Add(new MailAddress(toEmails[i]));
                    }



                    message.Subject = mailSubject;
                    message.Body = mailBody;
                    //MemoryStream stream = new MemoryStream(new byte[64000]);
                    message.IsBodyHtml = true;
                    Attachment attachment = new Attachment(SummaryAttachmentPath);
                    //Attachment attachment1 = new Attachment(DetailedReport);
                    message.Attachments.Add(attachment);
                    //message.Attachments.Add(attachment1);

                    try
                    {
                        smtp.Send(message);
                        Console.WriteLine("Summary Mail Sent Successfully");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception caught in CreateTestMessage3(): {0}", ex.ToString());
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Sent Summary Mail :- " + ex.Message.ToString());
            }
        }

        
        public static bool PerformActionOnAlert(string action, out string strOutText)
        {
            bool blnFlag = true;
            strOutText = "";

            try
            {
                IAlert alert = GenericClass.driver.SwitchTo().Alert();

                switch (action.ToUpper())
                {
                    case "OK":
                        {
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

                            strOutText = alert.Text;
                            break;
                        }

                }
            }
            catch (Exception e)
            {
                //Err.Description = e.Message;
                Console.WriteLine(e.Message);
                blnFlag = false;
            }

            return blnFlag;

        }

        #region Utility functions

        public static class SortTools
        {
            public static bool IsSorted(int[] arr)
            {
                for (int i = 1; i < arr.Length; i++)
                {
                    if (arr[i - 1] > arr[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            public static bool IsSorted(string[] arr)
            {
                for (int i = 1; i < arr.Length; i++)
                {
                    if (arr[i - 1].CompareTo(arr[i]) > 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            public static bool IsSortedDescending(int[] arr)
            {
                for (int i = arr.Length - 2; i >= 0; i--)
                {
                    if (arr[i] < arr[i + 1])
                    {
                        return false;
                    }
                }
                return true;
            }
            public static bool IsSortedDescending(string[] arr)
            {
                for (int i = arr.Length - 2; i >= 0; i--)
                {
                    if (arr[i].CompareTo(arr[i + 1]) < 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public void KillProcess(string ProcessName)
        {
            Process[] processesByName = Process.GetProcessesByName(ProcessName);
            if (processesByName.Count<Process>() != 0)
            {
                Process[] array = processesByName;
                for (int i = 0; i < array.Length; i++)
                {
                    Process process = array[i];
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
            }
        }
        public string CDateToString(string date, string Format)
        {
            string result = "";
            try
            {
                result = DateAndTime.DateValue(date).ToString(Format);
            }
            catch (Exception)
            {
            }
            return result;
        }
        public bool PressKeyboardKey(string keyName)
        {
            bool result;
            try
            {
                SendKeys.SendWait(keyName);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        public string ExtractNumerics(string strData)
        {
            string text = "";
            for (int i = 0; i < strData.Length; i++)
            {
                try
                {
                    text += Convert.ToInt32(strData.Substring(i, 1)).ToString();
                }
                catch (Exception)
                {
                }
            }
            return text;
        }
        public string RandomString(int stringLenght)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < stringLenght; i++)
            {
                char value = Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * random.NextDouble() + 65.0)));
                stringBuilder.Append(value);
            }
            return stringBuilder.ToString();
        }
        public bool VerifySorting(object arr, string sortascdsc)
        {
            string text = arr.GetType().ToString();
            if (text.ToLower().Contains("int"))
            {
                int[] arr2 = arr as int[];
                string a;
                if ((a = sortascdsc.ToLower().Trim()) != null)
                {
                    if (a == "asc")
                    {
                        return GenericClass.SortTools.IsSorted(arr2);
                    }
                    if (a == "dsc")
                    {
                        return GenericClass.SortTools.IsSortedDescending(arr2);
                    }
                }
            }
            else
            {
                string[] arr3 = arr as string[];
                string a2;
                if ((a2 = sortascdsc.ToLower().Trim()) != null)
                {
                    if (a2 == "asc")
                    {
                        return GenericClass.SortTools.IsSorted(arr3);
                    }
                    if (a2 == "dsc")
                    {
                        return GenericClass.SortTools.IsSortedDescending(arr3);
                    }
                }
            }
            return false;
        }
        public bool CompareString(string stringvalue1, string stringvalue2, string ignorecase = "")
        {
            int num = 1;
            if (ignorecase == "")
            {
                num = Strings.StrComp(stringvalue1, stringvalue2, CompareMethod.Binary);
            }
            if (ignorecase.ToUpper().Equals("IGNORECASE"))
            {
                num = Strings.StrComp(stringvalue1, stringvalue2, CompareMethod.Text);
            }
            return num == 0;
        }
        public string RemoveSpecialCharactersFromString(string stringvalue)
        {
            string text = "";
            try
            {
                Regex regex = new Regex("(?:[^a-z0-9% ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
                text = regex.Replace(stringvalue, string.Empty);
                text = Strings.Replace(Strings.Trim(text), " ", "", 1, -1, CompareMethod.Binary);
                text = Strings.Replace(Strings.Trim(text), ")", "", 1, -1, CompareMethod.Binary);
                text = Strings.Replace(Strings.Trim(text), "™", "", 1, -1, CompareMethod.Binary);
            }
            catch (Exception)
            {
            }
            return text;
        }
        public string ReplaceSpecialCharacterAndSpaces(string strText, string strChrToReplaceWith)
        {
            string text = "";
            try
            {
                Regex regex = new Regex("(?:[^a-z0-9% ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
                text = regex.Replace(strText, strChrToReplaceWith);
                text = text.Replace(" ", "_");
            }
            catch (Exception)
            {
            }
            return text;
        }
        public bool DeleteFilesFromDirectory(string FileNameWithPath, string FileOrFolder = "")
        {
            string a;
            if ((a = FileOrFolder.ToLower().Trim()) != null)
            {
                if (a == "" || a == "file")
                {
                    if (File.Exists(FileNameWithPath))
                    {
                        try
                        {
                            File.Delete(FileNameWithPath);
                            bool result = true;
                            return result;
                        }
                        catch (Exception)
                        {
                            bool result = false;
                            return result;
                        }
                    }
                    return false;
                }
                if (!(a == "folder"))
                {
                    return false;
                }
                if (Directory.Exists(FileNameWithPath))
                {
                    try
                    {
                        Directory.Delete(FileNameWithPath, true);
                        bool result = true;
                        return result;
                    }
                    catch (Exception)
                    {
                        bool result = false;
                        return result;
                    }
                }
                return false;
            }
            return false;
        }
        public string[] SortArray(string[] arr, string sortascdsc)
        {
            arr.Count<string>();
            Array.Sort<string>(arr, StringComparer.InvariantCulture);
            string a;
            if ((a = sortascdsc.ToLower().Trim()) != null)
            {
                if (a == "asc")
                {
                    return arr;
                }
                if (a == "dsc")
                {
                    Array.Reverse(arr);
                    return arr;
                }
            }
            return arr;
        }
        public int[] SortArray(int[] arr, string sortascdsc)
        {
            arr.Count<int>();
            Array.Sort(arr, StringComparer.InvariantCulture);
            string a;
            if ((a = sortascdsc.ToLower().Trim()) != null)
            {
                if (a == "asc")
                {
                    return arr;
                }
                if (a == "dsc")
                {
                    Array.Reverse(arr);
                    return arr;
                }
            }
            return arr;
        }
        public object[] MergeArray(object[] first, object[] second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            Type type = first.GetType();
            Type type2 = second.GetType();
            if (type != type2)
            {
                throw new InvalidOperationException("type mismatch");
            }
            Hashtable table = new Hashtable();
            ArrayList arrayList = new ArrayList();
            GenericClass.NewMethod(first, table, arrayList);
            GenericClass.NewMethod(second, table, arrayList);
            return (object[])arrayList.ToArray(type.GetElementType());
        }
        private static void NewMethod(object[] array, Hashtable table, ArrayList items)
        {
            for (int i = 0; i < array.Length; i++)
            {
                object obj = array[i];
                if (!table.Contains(obj))
                {
                    table.Add(obj, 1);
                    items.Add(obj);
                }
            }
        }

        /*deleteCookies method deleted as its not referred*/
        public string getscreenresolution()
        {
            string result = "";
            try
            {
                int height = Screen.PrimaryScreen.Bounds.Height;
                int width = Screen.PrimaryScreen.Bounds.Width;
                result = width + "x" + height;
            }
            catch (Exception)
            {
            }
            return result;
        }

        #endregion

    }
}
