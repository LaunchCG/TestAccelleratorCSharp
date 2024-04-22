using  Common.TestFramework.Core;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace  UI.Tests.PageActions
{
    public class DBValidationTest
    {
        string reportStepDesc;
        string reportStepVerify;
        Reports reporter = new Reports();
        DBValidation dbVal = new DBValidation();

        public bool VerifyOrderIdDetails(string ordId)
        {
            bool blnFlag = false;
            reportStepDesc = "Verify the order id details from the <b>'[TrainingDB].[dbo].[ORDERS]'</b> table";

            var result = dbVal.GetTestDataDataSet("select * from [TrainingDB].[dbo].[ORDERS] where [ORD_NUM] = '" + ordId + "'", GenericClass.dicApplicationUrl["TrianingDBConnection"]);
            string strOrdNumber = result.Tables[0].Rows[0]["ORD_NUM"].ToString();
            string strOrdAmt = result.Tables[0].Rows[0]["ORD_AMOUNT"].ToString();
            string strAdvanceAmt = result.Tables[0].Rows[0]["ADVANCE_AMOUNT"].ToString();
            string strOrdDate = result.Tables[0].Rows[0]["ORD_DATE"].ToString();
            string strCustCode = result.Tables[0].Rows[0]["CUST_CODE"].ToString();
            string strAgentCode = result.Tables[0].Rows[0]["AGENT_CODE"].ToString();
            string strOrdDesc = result.Tables[0].Rows[0]["ORD_DESCRIPTION"].ToString();

            if (strOrdNumber.Equals(ordId))
            {
                blnFlag = true;
                reportStepVerify = "Order id:  <b>" + strOrdNumber + "</b> successfully found and the details are Order Amount: = <b>" + strOrdAmt + "</b>, Advance Amount: = <b>" + strAdvanceAmt + "</b>, Order Date: = <b>" + strOrdDate + "</b>, Customer Code: = <b>" + strCustCode + "</b>, Agent Code: = <b>" + strAgentCode + "</b>, Order Description: = <b>" + strOrdDesc + "</b>";
            }
            else
            {
                Err.Description = "Order ID not found";
            }

            if (blnFlag)
            {
                reporter.ReportStep(reportStepDesc, reportStepVerify, "Pass", GenericClass.sStepStartTime);
            }
            else
            {
                reporter.ReportStep(reportStepDesc, Err.Description, "Fail", GenericClass.sStepStartTime);
            }
            Console.WriteLine(reportStepVerify);
            return blnFlag;            
        }
        public bool VerifyEmpDetails(string empId)
        {
            bool blnFlag = false;
            reportStepDesc = "Verify the Employee details from the <b>[TestDB].[dbo].[emp]</b> table";

            var result = dbVal.GetTestDataDataSet("select * from [TestDB].[dbo].[emp] where [emp_id] = '" + empId + "'", GenericClass.dicApplicationUrl["TestDBConnection"]);
            string strEmpid = result.Tables[0].Rows[0]["emp_id"].ToString();
            string strEmpFName = result.Tables[0].Rows[0]["f_name"].ToString();
            string strEmpLName = result.Tables[0].Rows[0]["l_name"].ToString();

            if (strEmpid.Equals(empId))
            {
                blnFlag = true;
                reportStepVerify = "Emp id: <b>" + strEmpid + "</b> successfully retrieved and the details are FirstName: = <b>" + strEmpFName + "</b>, Last Name: = <b>" + strEmpLName + "</b>";
            }
            else
            {
                Err.Description = "Employee not found";
            }

            if (blnFlag)
            {
                reporter.ReportStep(reportStepDesc, reportStepVerify, "Pass", GenericClass.sStepStartTime);
            }
            else
            {
                reporter.ReportStep(reportStepDesc, Err.Description, "Fail", GenericClass.sStepStartTime);
            }
            Console.WriteLine(reportStepVerify);
            return blnFlag;
        }

    }
}
