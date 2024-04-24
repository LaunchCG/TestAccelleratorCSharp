# Framework Features:
 - Multi browser support
 - Multi environment support
 - Data driven UI code with custom HTML reports
 - BDD Specflow code with Extent report
 - Restful services (API's) testing with Restsharp
 - Multi iteration with different data sets
 - Code for DB Validation
 - Email feature 

# Steps to set-up:
**Step 1:**
Install the following tools:
 - as
 - asdf
 - asdf
 - asdf

**Step 2:**
 - Open the solution file in MS VS and restore all packages which are already installed
 - Clean solution once
 - Rebuild solution now
 - Now you are in a position to run the scripts if there are no errors while building the solution  
     - If there are errors, we need to fix those based on the error we see in the o/p window

**Step 3:**
 - Open Test Explorer window and enable Traits view – this will list all scripts under each category
 - Open any script and take a look at how the script is designed

**Step 4:**
 - On the Solution explorer, you can see different projects
 - Other projects are:  
    1. BDDSpecFlow.Tests
         - This project contains use cases related to BDD 
    2. Common.TestFramework.core
         - This has common class files required for the UI and webservices projects
    3. UI.Tests
         - This project is designed based on Page Object Model (PoM)
         - All TC’s are under RegressionTestSuite class file
    4. WebServices.Tests
         - This project is for API and Webservices testing
         - All TC’s are under RegressionTestSuite class file

**Step 5:**
 - Under UI and Webservice projects, there is a Datasheet folder
 - Look for Config.xls, open the file and select the environment and corresponding data sheet pertaining to the environment selected above
 - Also select the Test project name, Test suite file name and browser you want to select  
![alt text](image.png)

**Step 6:**
 - Ensure test data sheets are separate for each environment
     - TestDataALPHA.xls
     - TestDataBETA.xls
     - TestDataDEV.xls
     - TestDataPROD.xls

**Step 7:**
 - TC naming convention
     - Sample TC name
         - PurchaseItems_UI_SD_App_PurchaseItems
     - Scenario name
         - PurchaseItems
     - TC Detail name
         - UI_SD_App_PurchaseItems
 - Scenario name and TC detail name should be connected using a ‘_’ (underscore) symbol
 - Datasheet should contain the Scenario name as the tab name like below

**Step 8:**
 - Select any TC from the Test explorer and run it
 - If you want to run in suite, select any category from test explorer and execute it

**Step 9:**
 - Reports can be found in this location
 - *\UI.Tests\Reports
 - *\WebServices.Tests\Reports
 - Report has Summary report (html) and Detailed report (html)


# Execution Flow
 - RegressionSuite.cs
     - AddTestDataforApplicationNew
 - ConfigClass.cs
     - [Onetimesetup]
         - TestFixturesetup()
             - getAppConfigData(1)
             - getExecutionData()
             - PrepSummaryReport
             - SummaryReportHeader
 - BaseClass.cs
     - [SetUp]
         - Setup()
             - AddTestDataforApplication(intIteration()
             - PrepDetailedReport()
             - DetailedReportHeader()
 - RegressionSuite.cs
     - <'Test Case e.g. PurchaseItems_UI_SD_App_PurchaseItems'>
	
 - BaseClass.cs
     - [TearDown]
         - TearDown()
             - EndTestIteration()
 - ConfigClass.cs
     - [OnetimeTearDown]
         - TestFixtureTearDown()
             - EndSummaryTestReport
