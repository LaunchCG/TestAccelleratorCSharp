using System;
using System.Linq;
using NUnit.Framework;


namespace  Common.TestFramework.Core
{

    public class BaseClass : ActionClass
    {
        //@ Initialize Variables
        public bool blnStepRC = false;
        int intIteration = 1;
        //public static int IterationForReport;

        public Reports reporter = new Reports();
        public GenericClass util = new GenericClass();
        public GenericClass g1 = new GenericClass();
        string strFinalStatus = "";

        [SetUp]
        public void setup()
        {
            GenericClass.dicDict["Folder"] = NUnit.Framework.TestContext.CurrentContext.Test.Name.Split('_')[0].Trim();
            GenericClass.dicDict["TestCaseName"] = NUnit.Framework.TestContext.CurrentContext.Test.Name.Substring(GenericClass.dicDict["Folder"].Length + 1, NUnit.Framework.TestContext.CurrentContext.Test.Name.Length - GenericClass.dicDict["Folder"].Length - 1).Split('(')[0];
            GenericClass.dicDict["TSName"] = NUnit.Framework.TestContext.CurrentContext.Test.ClassName.Split('.').Last();


            //Getting test case specific data
            g1.AddTestDataforApplication(intIteration);
            GenericClass.dicDict["TestCaseId"] = GenericClass.dicTestData["TestCaseId"];

            //Report file formatting            
            reporter.PrepDetailedReport(GenericClass.dicTestData["TestCaseId"] + "_" + util.RemoveSpecialCharactersFromString(GenericClass.dicDict["TestCaseName"]));
            reporter.DetailedReportHeader();
        }
        

        [TearDown]
        public void TearDown()
        {
            if (Reports.TestCaseScenarioChange != GenericClass.dicDict["Folder"].ToString())
            {
                Reports.TestCaseScenarioChange = GenericClass.dicDict["Folder"].ToString();
                Reports.IterationForReport = 0;
            }

            //Inserting Execution Details to Report File
            reporter.EndTestIteration(Reports.IterationForReport, out strFinalStatus);

            
            //@ Insert Logic for Summary Report for multiple Iteration
            if (strFinalStatus.ToLower() == "fail")
            {
                reporter.strSummaryFinalStatus = strFinalStatus;
            }

            if (reporter.SummaryAttachReportPath.Length != 0)
            {
                if (reporter.strSummaryFinalStatus.ToLower() != "fail")
                {
                    reporter.strSummaryFinalStatus = "PASS";
                }                
                Reports.SummaryReportDataEntry(GenericClass.dicDict["Folder"].ToString(), reporter.strSummaryFinalStatus.ToUpper(), Reports.TestCaseExecutionTime, reporter.SummaryAttachReportPath);
                reporter.strSummaryFinalStatus = "";
            }
            

            // Remove Dictionary key
            if (GenericClass.dicDict.ContainsKey("blnBrowserInvoke"))
            {
                //@ initialize the dictionary for broswer
                GenericClass.dicDict.Remove("blnBrowserInvoke");
            }

            //Shutting down Selenium Server/driver. Killing processes for browser and workbook
            g1.TeardownTest();

            //Clearing TestData dictionary for managing multiple iterations
            GenericClass.dicTestData.Clear();            

            //Re-setting values to base state for handling multiple iterations
            GenericClass.iStepCount = 0;
            GenericClass.iPassStepCount = 0;
            GenericClass.iFailStepCount = 0;
            g1.KillObjectInstances("EXCEL");
            reporter.EndTestReport();
        }
    }
}
