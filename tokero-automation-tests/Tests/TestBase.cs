// Utils/TestBase.cs
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.Playwright;
using NUnit.Framework;
using tokero_automation_tests.tokero_automation_tests.Utils;

namespace tokero_automation_tests.tokero_automation_tests.Utils
{
    public abstract class TestBase
    {
        protected BrowserFactory BrowserFactory;
        protected IBrowser Browser;
        protected IBrowserContext Context;
        protected static ExtentReports Extent;
        protected ExtentTest Test;

        [OneTimeSetUp]
        public void InitializeReport()
        {
            if (Extent != null) return;

            var reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestReports", "ExtentReport.html");
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath)!);

            var sparkReporter = new ExtentSparkReporter(reportPath);
            Extent = new ExtentReports();
            Extent.AttachReporter(sparkReporter);
        }

        [SetUp]
        public async Task SetUpTestAsync()
        {
            BrowserFactory = new BrowserFactory();
            Browser = await BrowserFactory.GetBrowserAsync();
            Context = await Browser.NewContextAsync();
            Test = Extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public async Task TearDownTestAsync()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;

            if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
                Test.Fail($"Test failed: {message}");
            else if (status == NUnit.Framework.Interfaces.TestStatus.Passed)
                Test.Pass("Test passed.");

            await BrowserFactory.DisposeAsync();
        }

        [OneTimeTearDown]
        public void FlushReport()
        {
            Extent.Flush();
        }
    }
}