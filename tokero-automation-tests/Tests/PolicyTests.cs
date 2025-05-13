using AventStack.ExtentReports.Model;
using NUnit.Framework;
using tokero_automation_tests.tokero_automation_tests.Pages;

namespace tokero_automation_tests.tokero_automation_tests.Tests;

[TestFixture]
public class PolicyTests : TestBase
{
    [Test]
    public async Task ValidatePoliciesHeaderTitles()
    {
        try
        {
            var page = await Context.NewPageAsync();
            await page.GotoAsync("https://tokero.dev/en/");

            var footerPage = new FooterPage(page);
            await footerPage.AcceptCookiesIfVisibleAsync();

            await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
            await page.WaitForTimeoutAsync(1000);
            await footerPage.NavigateToPoliciesPageAsync();

            var policiesPage = new PoliciesPage(page);
            var policyLinks = await policiesPage.GetPolicyLinksAsync();

            var expectedHeaderTitles = new List<string>
            {
                "Terms of Service",
                "Privacy Policy",
                "Fees and timings",
                "Cookies Policy",
                "KYC and AML policy",
                "TOKERO Affiliate Heroes Program",
                "Request answering/processing times",
                "Minimums and options",
                "RIGHTS OF DATA SUBJECTS WITH REGARD TO THE PROCESSING OF PERSONAL DATA UNDER EU REGULATION 2016/679",
                "AML Country Status"
            };

            var policyLinksList = policyLinks.ToList();

            for (var i = 0; i < policyLinksList.Count; i++)
            {
                var link = policyLinksList[i];
                var response = await page.GotoAsync(link);
                Assert.That(response?.Status, Is.EqualTo(200), $"Page {link} did not return status 200.");

                var policyPage = new PolicyPage(page);
                var htmlContent = await policyPage.GetPolicyPageHtmlContentAsync();

                var expectedHeaderTitle = expectedHeaderTitles[i];
                Assert.That(htmlContent, Does.Contain(expectedHeaderTitle),
                    $"Expected header title '{expectedHeaderTitle}' not found in the page HTML.");
            }

            Test.Pass("ValidatePoliciesHeaderTitles passed successfully.");
        }
        catch (Exception ex)
        {
            Test.Fail($"ValidatePoliciesHeaderTitles failed: {ex.Message}");
            throw;
        }
    }
}
