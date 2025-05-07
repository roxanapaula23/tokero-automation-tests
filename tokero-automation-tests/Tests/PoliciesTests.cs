using System.Text.RegularExpressions;
using Microsoft.Playwright;
using NUnit.Framework;
using tokero_automation_tests.tokero_automation_tests.Pages;
using tokero_automation_tests.tokero_automation_tests.Utils;

namespace tokero_automation_tests.tokero_automation_tests.Tests;

[TestFixture]
public class PoliciesTests
{
    private BrowserFactory _browserFactory;
    private IBrowser _browser;
    private IBrowserContext _context;

    [SetUp]
    public async Task SetUp()
    {
        _browserFactory = new BrowserFactory();
        _browser = await _browserFactory.GetBrowserAsync();
        _context = await _browser.NewContextAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _browserFactory.DisposeAsync();
    }

    [Test]
    public async Task ValidatePoliciesTitles()
    {
        var page = await _context.NewPageAsync();
        await page.GotoAsync("https://tokero.dev/en/");

        var footerPage = new FooterPage(page);
        await footerPage.AcceptCookiesIfVisibleAsync();

        await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
        await page.WaitForTimeoutAsync(1000);
        await footerPage.NavigateToPoliciesPageAsync();

        var policiesPage = new PoliciesPage(page);
        var policyTitles = await policiesPage.GetPolicyTitlesAsync();

        var expectedPolicyTitles = new List<string>
        {
            "Terms and Conditions",
            "Privacy",
            "Fees",
            "Cookies",
            "KYC",
            "Referrals",
            "Request answering/processing times",
            "Minimums and options",
            "GDPR",
            "Countries list for AML risk assessment"
        };

        Assert.That(policyTitles, Is.Not.Empty, "No policy titles were found.");

        foreach (var expectedTitle in expectedPolicyTitles)
        {
            Assert.That(policyTitles.Contains(expectedTitle),
                $"Expected policy title '{expectedTitle}' was not found in the actual list.");
        }
    }

    [Test]
    public async Task ValidatePoliciesLinks()
    {
        var page = await _context.NewPageAsync();
        await page.GotoAsync("https://tokero.dev/en/");

        var footerPage = new FooterPage(page);
        await footerPage.AcceptCookiesIfVisibleAsync();

        await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
        await page.WaitForTimeoutAsync(1000);

        await footerPage.NavigateToPoliciesPageAsync();

        var policyPage = new PoliciesPage(page);
        var policyLinks = await policyPage.GetPolicyLinksAsync();

        Assert.That(policyLinks, Is.Not.Empty, "No policy links were found.");

        var policyLinkPattern = @"^https:\/\/tokero\.dev\/en\/[a-z0-9\-\/]+$";

        foreach (var link in policyLinks)
        {
            Assert.That(Regex.IsMatch(link, policyLinkPattern),
                $"Policy link '{link}' does not match the expected pattern.");

            var response =
                await page.GotoAsync(link, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
            Assert.That(response?.Status, Is.EqualTo(200),
                $"Page '{link}' did not return status 200. Actual: {response?.Status}");
        }
    }
}