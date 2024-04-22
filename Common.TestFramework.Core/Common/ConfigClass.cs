using System;
using NUnit.Framework;
using System.Xml;
using System.IO;
using NUnit.Framework.Internal;
using System.Data.OleDb;

namespace  Common.TestFramework.Core
{

    [SetUpFixture]
    public class ConfigClass : BaseClass
    {
        bool isCodeGenerated = true;

        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            //@ Initialize Global data 
            g1.GetAppConfigData(1);
            g1.GetExecutionData();

            Reports.PrepSummaryReport(GenericClass.dicConfig["TestProjectName"], GenericClass.dicConfig["strEnvironment"]);
            Reports.SummaryReportHeader();
        }

        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
            //@ Create HTML Summary Report
            Reports.EndSummaryTestReport();

            //@ Tear Down Code
            if (!isCodeGenerated)
            {
                try
                {
                    GenericClass.oReportWriter.Close();
                }
                catch (Exception excp)
                {
                    Console.WriteLine(excp.Message);
                }

                File.Delete(GenericClass.dicReporting["ReportPath"].Replace("LastRun", GenericClass.dicDict["Folder"] + "\\" + GenericClass.dicDict["TestCaseId"]));
                GenericClass.dicMaster.Clear();
                GenericClass.dicOutput.Clear();
                GenericClass.dicCurrentIteartion.Clear();
                GenericClass.dicQuery.Clear();
                GenericClass.dicDict.Clear();
                Reports.Instance = string.Empty;
                Reports.BuildNumber = string.Empty;
                if (GenericClass.dicConfig.ContainsKey("QueryString"))
                    GenericClass.dicConfig.Remove("QueryString");
            }

            //Added code for sending email summary
            if (GenericClass.dicConfig["SendSummaryMail"].ToLower().Contains("yes"))
            {
                //GenericClass.SendSummaryStatusMail((GenericClass.dicReporting["SummaryReportPath"]), (Reports.iPassTotalCount + Reports.iFailTotalCount), Reports.iPassTotalCount, Reports.iFailTotalCount, Reports.iNoRunTotalCount);
                GenericClass.SaveToEmailQueue1((Reports.iPassTotalCount + Reports.iFailTotalCount), Reports.iPassTotalCount, Reports.iFailTotalCount, Reports.iNoRunTotalCount);
            }

            GenericClass.dicMaster.Clear();
            GenericClass.dicOutput.Clear();
            GenericClass.dicCurrentIteartion.Clear();
            GenericClass.dicQuery.Clear();
            GenericClass.dicDict.Clear();
            if (GenericClass.dicConfig.ContainsKey("QueryString"))
            {
                GenericClass.dicConfig.Remove("QueryString");
            }
            GenericClass.oReportWriter.Close();
        }

    }
}
