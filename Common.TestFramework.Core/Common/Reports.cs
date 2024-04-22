using System;
using Microsoft.VisualBasic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Reflection;
using OpenQA.Selenium;

namespace  Common.TestFramework.Core
{
    public class Reports : GenericClass
    {
        public static StreamWriter oReportSummaryWriter;
        public static int iSummaryStepCount;
        public static int iPassTotalCount;
        public static int iFailTotalCount;
        public static int iNoRunTotalCount;
        public static TimeSpan iExceutionTime;
        public static int iSummaryPassStepCount;
        public static int iSummaryFailStepCount;
        public string strSummaryFinalStatus = "";
        public static string strBVer;
        public static TimeSpan TestCaseExecutionTime;
        public string SummaryAttachReportPath;
        public string DetailedReport = string.Empty;
        public string sTestDur;
        public static string TestCaseScenarioChange = string.Empty;
        public static int IterationForReport = 0;
        public static string BuildNumber;
        public static string Instance;
        public static string APIMethodType;
        GenericClass util = new GenericClass();


        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        public Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());


        }

        public static Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up 
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;

        }

        //----------------------------------------------------------------------------------------//
        // Function Name : ReportStep
        // Function Description : This function perform reporting for a step.
        // Input type : string
        // Input Variable : sExpectedStat
        // Input Description : Expected result for the step
        // Input type : string
        // Input Variable : sActualStat
        // Input Description : Actual result for the step
        // OutPut : void
        // example : ReportStep("Login to pyramid core" , "Successfully logged in"
        //---------------------------------------------------------------------------------------//

        public void ReportStep(string sStepDescription, string sActualResult, string sStepStatus, DateTime sStepStartTime)
        {
            if (sStepStatus.ToLower().Contains("pass") || sStepStatus.ToLower().Contains("true") || sStepStatus.ToLower().Contains("done"))
                ReportStep(sStepDescription, sActualResult, "", "pass", sStepStartTime);
            else if (sStepStatus.ToLower().Contains("warning"))
                ReportStep(sStepDescription, "", sActualResult, "warning", sStepStartTime);
            else
                ReportStep(sStepDescription, "", sActualResult, "fail", sStepStartTime);
        }

        //----------------------------------------------------------------------------------------//
        // Function Name : ReportStep
        // Function Description : This function perform reporting for a step.
        // Input type : string
        // Input Variable : sExpectedStat
        // Input Description : Expected result for the step
        // Input type : string
        // Input Variable : sActualPassStat
        // Input Description : Pass statement for the step
        // Input type : string
        // Input Variable : sActualFailStat
        // Input Description : Fail statement for the step
        // OutPut : void
        // example : ReportStep("Login to pyramid core" , "successfully logged in" , "Unable to log in")
        //---------------------------------------------------------------------------------------//

        public void ReportStep(string sStepDescription, string sPassedResult, string sFailedResult, string sStepStatus, DateTime sStepStartTime)
        {
            TimeSpan sStepEndTime = DateTime.Now - sStepStartTime;
            string strHour = sStepEndTime.Hours.ToString(), strMin = sStepEndTime.Minutes.ToString(), strSec = sStepEndTime.Seconds.ToString(), strMilliSecs = sStepEndTime.Milliseconds.ToString();
            if (sStepEndTime.Hours <= 9) strHour = "0" + strHour;
            if (sStepEndTime.Minutes <= 9) strMin = "0" + strMin;
            if (sStepEndTime.Seconds <= 9) strSec = "0" + strSec;
            string sTestStepDur = strHour + ":" + strMin + ":" + strSec + ":" + strMilliSecs;

            if (sStepStatus.ToLower().Contains("pass") || sStepStatus.ToLower().Contains("true") || sStepStatus.ToLower().Contains("done"))
                GenericClass.dicReporting["StepStatus"] = "pass";
            else if (sStepStatus.ToLower().Contains("warning"))
                GenericClass.dicReporting["StepStatus"] = "warning";
            else
                GenericClass.dicReporting["StepStatus"] = "fail";

            if (!Directory.Exists(dicConfig["ReportPath"] + GenericClass.dicDict["Folder"] + "\\"))
            {
                Directory.CreateDirectory(dicConfig["ReportPath"] + GenericClass.dicDict["Folder"] + "\\");

            }


            //string sSscreenshotFile;
            //int intPass = 0, intFail = 1;
            /* if (!dicConfig.ContainsKey("ScreenShot"))
             {
                 dicConfig.Add("ScreenShot", "No");
             } */
            oReportWriter.WriteLine("<tr>");

            oReportWriter.WriteLine("<TD class='tdborder_1'  align='center'>" + ++iStepCount + "</TD>");
            oReportWriter.WriteLine("<td class ='tdborder_1'>" + sStepDescription + "</td>");

            if (dicReporting["StepStatus"].ToLower() != "pass" && dicReporting["StepStatus"].ToLower() != "done")
            {
                if (sFailedResult.ToLower().Contains("permission denied") && sFailedResult.ToLower().Contains("location.href"))
                    dicReporting["StepStatus"] = "Pass";
            }

            if (dicReporting["StepStatus"].ToLower() == "pass" || dicReporting["StepStatus"].ToLower() == "done")
            {
                //SaveScreenShot();
                //oReportWriter.WriteLine("<td class ='tdborder_1'>" + sPassedResult + "</td>");
                //Newly added tdborder_1_passedcontent - 12Aug2020
                oReportWriter.WriteLine("<td class ='tdborder_1_PassedContent'>" + sPassedResult + "</td>");
                //oReportWriter.WriteLine(" <td  class ='tdborder_1_Pass' width ='20%' align='center'>" + dicReporting["StepStatus"].ToUpper() + "</td>");
                oReportWriter.WriteLine(" <td  class ='tdborder_1_Pass' align='center'>" + dicReporting["StepStatus"].ToUpper() + "</td>");
                //Added newly for step timing
                oReportWriter.WriteLine("<td class ='tdborder_1'>" + sTestStepDur + "</td>");
                iPassStepCount += 1;
            }
            else if (dicReporting["StepStatus"].ToLower() == "warning")
            {
                SaveScreenShot();
                oReportWriter.WriteLine("<td class ='tdborder_1'>" + sFailedResult + "</td>");
                //oReportWriter.WriteLine(" <td  class ='tdborder_1_Warning' width ='20%' align='center'>" + dicReporting["StepStatus"].ToUpper() + "</td>");
                oReportWriter.WriteLine(" <td  class ='tdborder_1_Warning' align='center'>" + dicReporting["StepStatus"].ToUpper() + "</td>");
                //Added newly for step timing
                oReportWriter.WriteLine("<td class ='tdborder_1'>" + sTestStepDur + "</td>");
                iPassStepCount += 1;
            }
            else
            {
                string sSscreenshotFile = GenericClass.dicDict["TestCaseId"] + "_" + util.RemoveSpecialCharactersFromString(GenericClass.dicDict["TestCaseName"]) + "_" + DateAndTime.Now.ToString().Replace("/", "_").Replace(" ", "_").Replace(":", "_");
                SaveScreenShot(sSscreenshotFile);

                if (GenericClass.driver != null)
                    oReportWriter.WriteLine("<td class ='tdborder_1'>" + sFailedResult + ".<br> For Screenshot: <a href=\"" + GenericClass.dicConfig["ScreenShotPath"].Replace("LastRun", GenericClass.dicDict["Folder"] + "\\" + GenericClass.dicDict["TestCaseId"]) + sSscreenshotFile + ".png" + "\" style=\"color: #0000FF\" target=\"_blank\">" + "Click here</a></td>");
                else
                    oReportWriter.WriteLine("<td class ='tdborder_1'>" + sFailedResult + "</td>");

                oReportWriter.WriteLine(" <td  class ='tdborder_1_Fail' align='center'>" + dicReporting["StepStatus"].ToUpper() + "</td>");
                //Added newly for step timing
                oReportWriter.WriteLine("<td class ='tdborder_1'>" + sTestStepDur + "</td>");
                iFailStepCount += 1;
            }
            oReportWriter.WriteLine("</tr>");
        }




        //----------------------------------------------------------------------------------------//
        // Function Name : PrepDetailedReport
        // Function Description : This function is used generate template for report
        // Input type : string
        // Input Variable : sTestCaseName
        // Input Description : Name of the testcase
        // OutPut : void
        // example : PreReport("CheckHolidayList")
        //---------------------------------------------------------------------------------------//

        public void PrepDetailedReport(string sTestCaseName)
        {
            var sReportFileName = GenericClass.dicConfig["ReportPath"].ToString() + "LastRun\\" + sTestCaseName + "_" +
            DateAndTime.Day(DateAndTime.Now) + "_" + DateAndTime.Month(DateAndTime.Now) + "_" +
            DateAndTime.Year(DateAndTime.Now) + "_" + DateAndTime.Hour(DateAndTime.Now) + "H_" +
            DateAndTime.Minute(DateAndTime.Now) + "M_" + DateAndTime.Second(DateAndTime.Now) + "S.html";

            oReportWriter = new StreamWriter(sReportFileName);

            oReportWriter.WriteLine("<html>");
            oReportWriter.WriteLine("<style>");

            oReportWriter.WriteLine(".subheading { BORDER-RIGHT:#000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-WEIGHT: bold;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #000000;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri,helvetica, sans-serif;HEIGHT: 20px;BACKGROUND-COLOR: #FAC090;Color: #000000}");

            oReportWriter.WriteLine(".subheading1{BORDER-RIGHT: #000000 1px solid;BACKGROUND-COLOR: #FFFFE0;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-WEIGHT: bold;FONT-SIZE: 13pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #000000;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri,helvetica, sans-serif;HEIGHT: 10px;}");

            oReportWriter.WriteLine(".subheading2{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 2px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 2px;FONT-WEIGHT: bold;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #000000;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri,helvetica, sans-serif;HEIGHT: 10px;BACKGROUND-COLOR: #F5F5F5;Color: #000000}");

            oReportWriter.WriteLine(".tdborder_1{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #000000;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri, helvetica,  sans-serif;HEIGHT: 20px}");
            //Newly added for PAssed content - Word wrap
            oReportWriter.WriteLine(".tdborder_1_PassedContent{WORD-BREAK:BREAK-ALL;BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #000000;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri, helvetica,  sans-serif;HEIGHT: 20px}");

            oReportWriter.WriteLine(".tdborder_1_Pass{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #00ff00;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri,  helvetica, sans-serif;HEIGHT: 20px}");

            oReportWriter.WriteLine(".SnapShotLink_style{PADDING-RIGHT: 4px;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;COLOR: #0000EE;PADDING-TOP: 0px;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px}");

            oReportWriter.WriteLine(".tdborder_1_Fail{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid; COLOR: #ff0000;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px}");

            oReportWriter.WriteLine(".tdborder_1_Done{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid; COLOR: #ffcc00;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri,  helvetica, sans-serif;HEIGHT: 20px}");

            oReportWriter.WriteLine(".tdborder_1_Skipped{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px  solid;COLOR: #00ccff;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px}");

            oReportWriter.WriteLine(".tdborder_1_Warning{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #660066;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px}");

            oReportWriter.WriteLine(".heading {FONT-WEIGHT: bold; FONT-SIZE: 17px; COLOR: #005484;FONT-FAMILY: Calibri, Verdana, Tahoma, Calibri;}");

            oReportWriter.WriteLine(".style1 { border: 1px solid #8eb3d8;padding: 0px 4px;FONT-WEIGHT: bold;FONT-SIZE: 12pt;COLOR: #000000;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px;width: 180px;}");

            oReportWriter.WriteLine(".style3 { border: 1px solid #8eb3d8;padding: 0px 4px;FONT-WEIGHT: bold;FONT-SIZE: 12pt;COLOR: #000000;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px;width: 2px;}");

            oReportWriter.WriteLine("</style>");


            oReportWriter.WriteLine("<head><title>" + GenericClass.dicConfig["ProjectName"] + " Test Result</title></head>");

            oReportWriter.WriteLine("<body vlink=\"FF00FF\">");

            oReportWriter.WriteLine("<table cellSpacing='0' cellPadding='0' border='0' align='center' style='width:96%;" +
                                    " margin-left:20px;'><td class='subheading1' colspan=5 align=center><p style='font-size:1.8em'>" +
                                    "<body link='#00ff00' vlink=\"FF00FF\">" + " Demo Project " + GenericClass.dicConfig["ProjectName"] + " Automation Detailed Report </body></td><tr></tr></table>");


            dicReporting["ReportPath"] = sReportFileName;

        }

        //----------------------------------------------------------------------------------------//
        // Function Name : DetailedReportHeader
        // Function Description : This function is used to generate haeder for new script
        // OutPut : void
        // example : DetailedReportHeader()
        //---------------------------------------------------------------------------------------//

        public void DetailedReportHeader()
        {
            sStartDate = DateTime.Now;
            iPassStepCount = 0;
            iFailStepCount = 0;
            oReportWriter.WriteLine("<table cellSpacing='0' cellPadding='0' border='0' align='center' style='width:96%; margin-" +
                                    "left:20px;'>");

            oReportWriter.WriteLine("<TR>" +
                                    " <TD class='subheading2' width ='10%' align='center' >Test Case Id</TD>" +
                                    " <TD class='subheading2' align='center' >Test Case Name</TD>" +
                                    " <TD class='subheading2' align='center'>Environment</TD>");

            if (GenericClass.dicConfig["strBrowserType"].ToLower() != "none")
            {
                oReportWriter.WriteLine(" <TD class='subheading2'align='center'>Browser</TD>");
            }



            oReportWriter.WriteLine("<TR>" +
                        " <TD class='tdborder_1'  vAlign=center  align=middle >" + GenericClass.dicDict["TestCaseId"] + "</TD>" +
                        " <TD class='tdborder_1'  vAlign=center  align=middle >" + dicDict["TestCaseName"] + "</TD>" +
                        " <TD class='tdborder_1'  vAlign=center  align=middle >" + GenericClass.dicConfig["strEnvironment"] + "</TD>");

            if (GenericClass.dicConfig["strBrowserType"].ToLower() != "none")
            {
                if (!dicConfig["ProjectName"].Equals("WebServices"))
                {
                    strBVer = GetBrowserVersion();
                }
                else
                {
                    strBVer = "NA";
                }
                //string strBVer = GetBrowserVersion();
                ////Console.WriteLine(strBVer);
                oReportWriter.WriteLine(" <TD class='tdborder_1'  vAlign=center  align=middle >" + GenericClass.dicConfig["strBrowserType"] + "<br> Version( " + strBVer + " )</TD>");
            }

            oReportWriter.WriteLine("</table>");

            oReportWriter.WriteLine("<table cellSpacing='0' cellPadding='0' border='0' align='center' style='width:96%;" +
                                    " margin-left:20px;'>");

            oReportWriter.WriteLine("<tr></tr>");

            oReportWriter.WriteLine("<tr>");
            oReportWriter.WriteLine("<td class='subheading2' align='center'>Steps</td>");
            oReportWriter.WriteLine("<td class='subheading2' align='center'>Description</td>");
            oReportWriter.WriteLine("<td class='subheading2' align='center'>Actual Result</td>");
            oReportWriter.WriteLine("<td class='subheading2' align='center'>Step Status</td>");
            oReportWriter.WriteLine("<td class='subheading2' align='center'>Timing</td>");
            oReportWriter.WriteLine("</tr>");
        }

        //----------------------------------------------------------------------------------------//
        // Function Name : EndTestIteration
        // Function Description : This function is used to end a test iterations
        // OutPut type : void
        // Output : null
        // example : EndTestIteration()
        //---------------------------------------------------------------------------------------//

        public void EndTestIteration(int IterNumber, out string strFinalStatus)
        {
            string htmlreportLink = "";
            SummaryAttachReportPath = "";
            strFinalStatus = "";

            //if (DDiff.Minutes == 0)
            DDiff = DateTime.Now - sStartDate;

            string strHour = DDiff.Hours.ToString(), strMin = DDiff.Minutes.ToString(), strSec = DDiff.Seconds.ToString(), strMilliSecs = DDiff.Milliseconds.ToString();
            if (DDiff.Hours <= 9) strHour = "0" + strHour;
            if (DDiff.Minutes <= 9) strMin = "0" + strMin;
            if (DDiff.Seconds <= 9) strSec = "0" + strSec;

            /*if (GenericClass.strNodeURL != "")
            {
                GenericClass objGeneric = new GenericClass();
                objGeneric.UpdateNodeStatus(GenericClass.strNodeURL, "idle");
            }*/

            sTestDur = strHour + ":" + strMin + ":" + strSec;

            oReportWriter.WriteLine("</table>");

            oReportWriter.WriteLine("<table cellSpacing='0' cellPadding='0' border='0' align='center' style='width:96%; " +
                                    " margin-left:20px;'>");

            oReportWriter.WriteLine("<TR></TR>");

            oReportWriter.WriteLine("<TR>");
            oReportWriter.WriteLine("<TD class='subheading2' align='center'>Test Step-Pass</TD>");
            oReportWriter.WriteLine("<TD class='subheading2' align='center'>Test Step-Fail</TD>");
            oReportWriter.WriteLine("<TD class='subheading2' align='center'>Execution date & time</TD>");
            oReportWriter.WriteLine("<TD class='subheading2' align='center'>Execution Machine name</TD>");
            oReportWriter.WriteLine("<TD class='subheading2' align='center'>Test run duration</TD>");
            oReportWriter.WriteLine("<TD class='subheading2' align='center'>Iteration</TD>");
            oReportWriter.WriteLine("<TD class='subheading2' align='center'>Build Version</TD>");
            oReportWriter.WriteLine("</TR>");

            oReportWriter.WriteLine("<TR>");
            oReportWriter.WriteLine("<TD class='tdborder_1'  align='center' >" + iPassStepCount + "</TD>");
            oReportWriter.WriteLine("<TD class='tdborder_1'  align='center'>" + iFailStepCount + "</TD>");
            oReportWriter.WriteLine("<TD class='tdborder_1'  align='center'>" + sStartDate + "</TD>");
            oReportWriter.WriteLine("<TD class='tdborder_1'  align='center'>" + Environment.MachineName.ToString() + "</TD>");

            TestCaseExecutionTime = TestCaseExecutionTime + TimeSpan.Parse(sTestDur);

            oReportWriter.WriteLine("<TD class='tdborder_1' align='center' >" + sTestDur + "</TD>");
            oReportWriter.WriteLine("<TD class='tdborder_1' align='center' >" + ++IterNumber + "</TD>");
            oReportWriter.WriteLine("<TD class='tdborder_1'  align='center'>" + BuildNumber + "</TD>");
            oReportWriter.WriteLine("</TR>");
            oReportWriter.WriteLine("<tr></tr><tr></tr><tr></tr><tr></tr><tr></tr><tr></tr>");

            oReportWriter.WriteLine("</table>");
            //Finishing Report
            oReportWriter.WriteLine("</table></body></html>");
            oReportWriter.Flush();


            if (!(iFailStepCount == 0 && iPassStepCount != 0))
            {
                strFinalStatus = "Fail";
            }
            else
            {
                strFinalStatus = "Pass";
            }

            //Check if the iteration has passed or not and if not, make final status as fail
            if (!(iFailStepCount == 0 && iPassStepCount != 0))
            {
                strFinalStatus = "Fail";
            }

            //ResultFinalStatTFS = strFinalStatus;

            // Code for generating Html Link In TestSuite
            htmlreportLink = GenericClass.dicReporting["ReportPath"];
            //htmlreportLink = htmlreportLink.Substring(0,htmlreportLink.LastIndexOf("\\")).Replace("LastRun", TestGenericUtility.dicDict["Folder"]);
            htmlreportLink = htmlreportLink.Replace("LastRun", GenericClass.dicDict["Folder"] + "\\" + GenericClass.dicDict["TestCaseId"]);

            SummaryAttachReportPath = htmlreportLink;

            KillObjectInstances("Excel");

            /* Microsoft.Win32.RegistryKey key;
             key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("strStatus");
             key.SetValue("strStatus", iFailStepCount);
             key.Close();*/
        }



        //----------------------------------------------------------------------------------------//
        // Function Name : EndTestReport
        // Function Description : This function is used to end a test report
        // OutPut type : void
        // Output : null
        // example : EndTestReport()
        //---------------------------------------------------------------------------------------//

        public void EndTestReport()
        {
            oReportWriter.Close();

            //Creating directory for which the test script belongs under 'Reports'
            if (!System.IO.Directory.Exists(GenericClass.dicConfig["ReportPath"] + GenericClass.dicDict["Folder"] + "\\" + GenericClass.dicDict["TestCaseId"]))
                System.IO.Directory.CreateDirectory(GenericClass.dicConfig["ReportPath"] + GenericClass.dicDict["Folder"] + "\\" + GenericClass.dicDict["TestCaseId"]);


            //Copying the file from Last Run to specific directory

            foreach (string strFile in System.IO.Directory.GetFiles(GenericClass.dicConfig["ReportPath"] + "LastRun"))
            {
                try
                {
                    if (strFile.Contains(GenericClass.dicDict["TestCaseId"]))
                    {
                        DetailedReport = GenericClass.dicConfig["ReportPath"] + GenericClass.dicDict["Folder"] + "\\" + GenericClass.dicDict["TestCaseId"] + "\\" + strFile.Split('\\').Last();
                        File.Move(strFile, DetailedReport);

                    }
                }
                catch (Exception e)
                { Console.WriteLine(e.Message); }
            }

            //TestGenericUtility.dicDetailedReport.Add(TestGenericUtility.dicDict["TestCaseName"], DetailedReport);
            Console.WriteLine("Completed TC : " + GenericClass.dicDict["TestCaseName"] + "," + " Iteration :" + IterationForReport + "," + " Time taken : " + sTestDur);

        }

        public static string GetBrowserVersion(string browsername = "")
        {
            string strBrowserVersion = "";
            try
            {
                switch (dicConfig["strBrowserType"].ToLower())
                {

                    case "firefox":
                        strBrowserVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Mozilla\Mozilla Firefox").GetValue("CurrentVersion").ToString();
                        break;
                    case "ie":
                        strBrowserVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer").GetValue("svcVersion").ToString();
                        break;
                    case "chrome":
                        strBrowserVersion = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Google\Chrome\BLBeacon").GetValue("Version").ToString();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                try
                {
                    switch (dicConfig["strBrowserType"].ToLower())
                    {
                        case "firefox":
                            strBrowserVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\Mozilla\Mozilla Firefox").GetValue("CurrentVersion").ToString();
                            break;
                        case "ie":
                            strBrowserVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer").GetValue("Version").ToString();
                            break;
                        case "chrome":
                            strBrowserVersion = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Google\Chrome\BLBeacon").GetValue("Version").ToString();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return strBrowserVersion;

        }

        public string GetBrowserVersion()
        {
            string strBrowserVersion = "";
            try
            {
                switch (dicConfig["strBrowserType"].ToLower())
                {

                    case "firefox":
                        strBrowserVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Mozilla\Mozilla Firefox").GetValue("CurrentVersion").ToString();
                        break;
                    case "ie":
                        strBrowserVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer").GetValue("svcVersion").ToString();
                        break;
                    case "chrome":
                        strBrowserVersion = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Google\Chrome\BLBeacon").GetValue("Version").ToString();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                try
                {
                    switch (dicConfig["strBrowserType"].ToLower())
                    {

                        case "firefox":
                            strBrowserVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\Mozilla\Mozilla Firefox").GetValue("CurrentVersion").ToString();
                            break;
                        case "ie":
                            strBrowserVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer").GetValue("Version").ToString();
                            break;
                        case "chrome":
                            strBrowserVersion = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Google\Chrome\BLBeacon").GetValue("Version").ToString();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return strBrowserVersion;

        }

        public void SaveScreenShot(string sSscreenshotFile = "")
        {

            if (GenericClass.driver == null)
                return;
            try
            {
                if (sSscreenshotFile == "")
                    sSscreenshotFile = GenericClass.dicDict["TestCaseId"] + "_" + util.RemoveSpecialCharactersFromString(GenericClass.dicDict["TestCaseName"]) + "_" + DateAndTime.Now.ToString().Replace("/", "_").Replace(" ", "_").Replace(":", "_");
                if (GenericClass.dicConfig["SeleniumVariant"].ToLower().Contains("webdriver"))
                {
                    //OpenQA.Selenium.Screenshot screenshot = null;
                    Screenshot sshot = ((ITakesScreenshot)GenericClass.driver).GetScreenshot();
                    sshot.SaveAsFile(dicConfig["ScreenShotPath"] + sSscreenshotFile + ".png");
                }

            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
        }


        public static void PrepSummaryReport(string sTestSuiteName, string strEnvName)
        {
            //iStepCount = 0;
            iSummaryStepCount = 0;
            //string FullTestCasePath = null;


            //string FullTestCasePath = (dicConfig["ReportPath"]).ToString();
            string FullTestCasePath = (dicConfig["ReportPath"]).ToString();


            if (!Directory.Exists(FullTestCasePath))
            {
                Directory.CreateDirectory(FullTestCasePath);
            }

            var SummaryReportFileName = FullTestCasePath.ToString() + sTestSuiteName + "_" + strEnvName + "_" + "SummaryReport" + "_" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '.') + ".html";
            //var SummaryReportFileName = FullTestCasePath.ToString() + sTestSuiteName + "_" + strEnvName + "_" + dicDictNew["TestSuiteName"]+ "_"+ "SummaryReport" + "_" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '.') + ".html";

            oReportSummaryWriter = new StreamWriter(SummaryReportFileName);

            oReportSummaryWriter.WriteLine("<html>");
            oReportSummaryWriter.WriteLine("<style>");

            oReportSummaryWriter.WriteLine(".subheading { BORDER-RIGHT:#000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-WEIGHT: bold;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #000000;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri,helvetica, sans-serif;HEIGHT: 20px;BACKGROUND-COLOR: #FAC090;Color: #000000}");//(new - Wheat:#F5DEB3) (old - Blue:#FAC090)

            oReportSummaryWriter.WriteLine(".subheading1{BORDER-RIGHT: #000000 1px solid;BACKGROUND-COLOR: #FFFFE0;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-WEIGHT: bold;FONT-SIZE: 13pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #000000;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri,helvetica, sans-serif;HEIGHT: 10px;}");//(new - LightSkyblue:#87CEFA) (old - #CCC0DA)

            oReportSummaryWriter.WriteLine(".subheading2{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 2px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 2px;FONT-WEIGHT: bold;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #000000;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri,helvetica, sans-serif;HEIGHT: 10px;BACKGROUND-COLOR: #F5F5F5;Color: #000000}");//(new - LightSkyblue:#87CEFA) (old - #C2DC9A)

            oReportSummaryWriter.WriteLine(".tdborder_1{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #000000;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri, helvetica,  sans-serif;HEIGHT: 20px}");

            oReportSummaryWriter.WriteLine(".tdborder_1_Pass{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #00ff00;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri,  helvetica, sans-serif;HEIGHT: 20px}");

            oReportSummaryWriter.WriteLine(".SnapShotLink_style{PADDING-RIGHT: 4px;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;COLOR: #0000EE;PADDING-TOP: 0px;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px}");

            oReportSummaryWriter.WriteLine(".tdborder_1_Fail{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid; COLOR: #ff0000;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px}");

            oReportSummaryWriter.WriteLine(".tdborder_1_Done{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid; COLOR: #ffcc00;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri,  helvetica, sans-serif;HEIGHT: 20px}");

            oReportSummaryWriter.WriteLine(".tdborder_1_Skipped{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px  solid;COLOR: #00ccff;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px}");

            oReportSummaryWriter.WriteLine(".tdborder_1_Warning{BORDER-RIGHT: #000000 1px solid;PADDING-RIGHT: 4px;BORDER-TOP: #000000 1px solid;PADDING-LEFT: 4px;FONT-SIZE: 12pt;PADDING-BOTTOM: 0px;BORDER-LEFT: #000000 1px solid;COLOR: #660066;PADDING-TOP: 0px;BORDER-BOTTOM: #000000 1px solid;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px}");

            oReportSummaryWriter.WriteLine(".heading {FONT-WEIGHT: bold; FONT-SIZE: 17px; COLOR: #005484;FONT-FAMILY: Calibri, Verdana, Tahoma, Calibri;}");

            oReportSummaryWriter.WriteLine(".style1 { border: 1px solid #8eb3d8;padding: 0px 4px;FONT-WEIGHT: bold;FONT-SIZE: 12pt;COLOR: #000000;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px;width: 180px;}");

            oReportSummaryWriter.WriteLine(".style3 { border: 1px solid #8eb3d8;padding: 0px 4px;FONT-WEIGHT: bold;FONT-SIZE: 12pt;COLOR: #000000;FONT-FAMILY: Calibri, helvetica, sans-serif;HEIGHT: 20px;width: 2px;}");

            oReportSummaryWriter.WriteLine("</style>");


            oReportSummaryWriter.WriteLine("<head><title>" + GenericClass.dicConfig["ProjectName"] + " Test Result</title></head>");

            oReportSummaryWriter.WriteLine("<body vlink=\"FF00FF\">");

            oReportSummaryWriter.WriteLine("<table cellSpacing='0' cellPadding='0' border='0' align='center' style='width:96%;" +
                                    " margin-left:20px;'><td class='subheading1' colspan=5 align=center><p style='font-size:1.8em'>" +
                                    "<body link='#00ff00' vlink=\"FF00FF\">" + "Demo Project " + GenericClass.dicConfig["ProjectName"] + " Automation Summary Report </body></td><tr></tr></table>");
            dicReporting["SummaryReportPath"] = SummaryReportFileName;

        }

        public static void SummaryReportHeader()
        {
            //sStartDate = DateTime.Now;
            iSummaryPassStepCount = 0;
            iSummaryFailStepCount = 0;
            iPassTotalCount = 0;
            iFailTotalCount = 0;
            iNoRunTotalCount = 0;
            iExceutionTime = TimeSpan.Parse("00:00:00");
            oReportSummaryWriter.WriteLine("<table cellSpacing='0' cellPadding='0' border='0' align='center' style='width:96%; margin-" +
                                    "left:20px;'>");

            oReportSummaryWriter.WriteLine("<TR>" +

                                    " <TD class='subheading2' width ='10%' align='center' >Test Suite Name</TD>" +
                                    " <TD class='subheading2' width ='45%' align='center'>Environment</TD>");

            if (dicConfig["strBrowserType"].ToLower() != "none")
            {
                oReportSummaryWriter.WriteLine(" <TD class='subheading2' width ='45%' align='center'>Browser</TD>");
            }

            string build = BuildNumber;
            oReportSummaryWriter.WriteLine("<TR>" +
                        " <TD class='tdborder_1'  vAlign=center  align=middle >" + GenericClass.dicConfig["TestProjectName"] + "</TD>" +
                        " <TD class='tdborder_1'  vAlign=center  align=middle >" + GenericClass.dicConfig["strEnvironment"] + "</TD>");

            if (dicConfig["strBrowserType"].ToLower() != "none")
            {
                if (!dicConfig["ProjectName"].Equals("WebServices"))
                {
                    strBVer = GetBrowserVersion("");
                }
                else
                {
                    strBVer = "NA";
                }

                oReportSummaryWriter.WriteLine(" <TD class='tdborder_1'  vAlign=center  align=middle >" + GenericClass.dicConfig["strBrowserType"] + "<br> Version( " + strBVer + " )</TD>");
            }

            oReportSummaryWriter.WriteLine("</table>");

            oReportSummaryWriter.WriteLine("<table cellSpacing='0' cellPadding='0' border='0' align='center' style='width:96%;" +
                                    " margin-left:20px;'>");

            oReportSummaryWriter.WriteLine("<tr></tr>");

            oReportSummaryWriter.WriteLine("<tr>" +
                                    " <td class='subheading2' width ='5%' align='center'>TC#</td>" +
                                    " <td class='subheading2'  width ='45%' align='center'>Test Case Scenarios</td>" +
                                    " <td class='subheading2'  width ='40%' align='center'>Test Case Status</td>" +
                                    " <td class='subheading2' width ='10%' align='center'>Execution Time</td>" +
                                    " </tr>");
        }

        public static void SummaryReportDataEntry(string TestCaseScenario, string TestCaseStatus, TimeSpan ExecutionTime, string strAttachReportPath)
        {
            oReportSummaryWriter.WriteLine("<tr>");

            //TC#
            oReportSummaryWriter.WriteLine("<TD class='tdborder_1'  align='center'>" + ++iSummaryStepCount + "</TD>");

            //Test Case Scenarios
            if (!dicConfig["ProjectName"].Equals("WebServices"))
            {
                oReportSummaryWriter.WriteLine("<td class ='tdborder_1'><a href=" + strAttachReportPath + " style='color:blue' target=\"_blank\">" + TestCaseScenario + " - " + "Iteration " + ++IterationForReport + " - " + Instance + "</a></td>");
            }
            else
            {
                oReportSummaryWriter.WriteLine("<td class ='tdborder_1'><a href=" + strAttachReportPath + " style='color:blue' target=\"_blank\">" + TestCaseScenario + " - " + "Iteration " + ++IterationForReport + " - " + APIMethodType + "</a></td>");
            }

            //Test Case Status
            if (TestCaseStatus.ToUpper() == "FAIL")
            {

                oReportSummaryWriter.WriteLine("<td class ='tdborder_1' style='color:red'>" + TestCaseStatus.ToUpper() + "</td>");
                ++iFailTotalCount;
            }
            else
            {
                oReportSummaryWriter.WriteLine("<td class ='tdborder_1' style='color:green'>" + TestCaseStatus.ToUpper() + "</td>");
                ++iPassTotalCount;
            }


            iExceutionTime = iExceutionTime + ExecutionTime;
            //Execution Time
            oReportSummaryWriter.WriteLine("<td class ='tdborder_1 width ='40%' align='center'>" + ExecutionTime + "</td>");
            iSummaryPassStepCount += 1;

            oReportSummaryWriter.WriteLine("</tr>");

            TestCaseExecutionTime = TimeSpan.Parse("00:00:00");
        }


        public static void EndSummaryTestReport()
        {
            oReportSummaryWriter.WriteLine("</table>");

            oReportSummaryWriter.WriteLine("<table cellSpacing='0' cellPadding='0' border='0' align='center' style='width:96%; " +
                                    " margin-left:20px;'>");

            oReportSummaryWriter.WriteLine("<TR></TR>");

            oReportSummaryWriter.WriteLine("<TR>");
            oReportSummaryWriter.WriteLine("<TD class='subheading2' align='center'>Test Cases-Passed</TD>");
            oReportSummaryWriter.WriteLine("<TD class='subheading2' align='center'>Test Cases-Failed</TD>");
            oReportSummaryWriter.WriteLine("<TD class='subheading2' align='center'>Test Cases-Not Run</TD>");
            oReportSummaryWriter.WriteLine("<TD class='subheading2' align='center'>Totals</TD>");
            oReportSummaryWriter.WriteLine("<TD class='subheading2' align='center'>Test Suite Duration</TD>");
            oReportSummaryWriter.WriteLine("<TD class='subheading2' align='center'>Build Version</TD>");

            oReportSummaryWriter.WriteLine("</TR>");

            oReportSummaryWriter.WriteLine("<TR>");
            oReportSummaryWriter.WriteLine("<TD class='tdborder_1'  align='center' >" + iPassTotalCount + "</TD>");
            oReportSummaryWriter.WriteLine("<TD class='tdborder_1'  align='center'>" + iFailTotalCount + "</TD>");
            oReportSummaryWriter.WriteLine("<TD class='tdborder_1'  align='center'>" + iNoRunTotalCount + "</TD>");
            oReportSummaryWriter.WriteLine("<TD class='tdborder_1'  align='center'>" + (iPassTotalCount + iFailTotalCount) + "</TD>");
            oReportSummaryWriter.WriteLine("<TD class='tdborder_1' align='center' >" + Convert.ToString(iExceutionTime) + "</TD>");
            oReportSummaryWriter.WriteLine("<TD class='tdborder_1' align='center' >" + BuildNumber + "</TD>");

            oReportSummaryWriter.WriteLine("</TR>");
            oReportSummaryWriter.WriteLine("<tr></tr><tr></tr><tr></tr><tr></tr><tr></tr><tr></tr>");

            oReportSummaryWriter.WriteLine("</table>");
            //Finishing Report
            oReportSummaryWriter.WriteLine("</table></body></html>");
            oReportSummaryWriter.Flush();
            oReportSummaryWriter.Close();
        }
    }
}
