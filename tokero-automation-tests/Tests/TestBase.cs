using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.Playwright;
using NUnit.Framework;
using tokero_automation_tests.tokero_automation_tests.Utils;

namespace tokero_automation_tests.tokero_automation_tests.Tests
{
    public abstract class TestBase
    {
        private BrowserFactory? _browserFactory;
        private IBrowser? _browser;
        private static ExtentReports? _extent;
        
        protected ExtentTest Test;
        protected IBrowserContext Context;

        [OneTimeSetUp]
        public void InitializeReport()
        {
            if (_extent != null) return;

            var reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestReports", "ExtentReport.html");
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath)!);

            var sparkReporter = new ExtentSparkReporter(reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(sparkReporter);
        }

        [SetUp]
        public async Task SetUpTestAsync()
        {
            _browserFactory = new BrowserFactory();
            _browser = await _browserFactory.GetBrowserAsync();
            Context = await _browser.NewContextAsync();
            Test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
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

            await _browserFactory.DisposeAsync();
        }

        [OneTimeTearDown]
        public void FlushReport()
        {
            _extent.Flush();
        }
    }
}