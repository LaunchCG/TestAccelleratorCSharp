using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace BDDSpecFlow.TestsNew
{
    [Binding]
    public class Hooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        private static ExtentReports extentReports;
        //private static ExtentHtmlReporter extentHtmlReporter;
        private static ExtentSparkReporter extentSparkReporter;
        private static FeatureContext featureContext;
        private static ScenarioContext scenarioContext;
        private static ExtentTest _feature;
        private static ExtentTest _scenario;

        [BeforeTestRun]
        public static void SetUp()
        {
            extentSparkReporter = new ExtentSparkReporter(@"D:\LaunchCG\Automation\Demo\GitProject\SeleniumCSharpAutomation\BDDSpecFlow.Tests\Reports\ExtentReport"+ "_" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '.') + ".html");
            extentReports = new ExtentReports();
            extentReports.AddSystemInfo("Browser", GenericUtility.browser);
            extentReports.AttachReporter(extentSparkReporter);
            GenericUtility.KillObjectInstances("chromedriver");
            GenericUtility.KillObjectInstances("IEDriverServer");
        }


        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            if (null != featureContext)
            {
                _feature = extentReports.CreateTest<Feature>(featureContext.FeatureInfo.Title, featureContext.FeatureInfo.Description);
            }
        }


        [BeforeScenario]
        public static void BeforeScenario(ScenarioContext scenarioContext)
        {
            Console.WriteLine("Before Scenario Hook");
            if (null != scenarioContext)
            {
                _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title, scenarioContext.ScenarioInfo.Description);
            }
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            Console.WriteLine("After Scenario Hook");
        }

        [AfterStep]
        public static void InsertReportingSteps(ScenarioContext scenarioContext)
        {
            // login --> Given , when, then
            //Add the node
            //ScenarioBlock scenarioBlock = scenarioContext.CurrentScenarioBlock;
            //switchCase

            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();

            if (scenarioContext.TestError == null)
            {
                switch (stepType)
                {
                    case "Given":
                        _scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                        break;
                    case "When":
                        _scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                        break;
                    case "Then":
                        _scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                        break;
                    default:
                        _scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
                        break;
                }
            }
            else if (scenarioContext.TestError != null)
            {
                switch (stepType)
                {
                    case "Given":
                        _scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.InnerException);
                        break;
                    case "When":
                        _scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.InnerException);
                        break;
                    case "Then":
                        _scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                        break;
                    default:
                        _scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                        break;
                }
            }
        }



        [AfterTestRun]
        public static void TearDown()
        {
            extentReports.Flush();
            BrowserManager.webDriver.Quit();
        }
    }
}
