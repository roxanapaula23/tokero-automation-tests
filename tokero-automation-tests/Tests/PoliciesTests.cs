using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using NUnit.Framework;
using tokero_automation_tests.tokero_automation_tests.Pages;
using tokero_automation_tests.tokero_automation_tests.Utils;

namespace tokero_automation_tests.tokero_automation_tests.Tests;

[TestFixture]
public class PoliciesTests : TestBase
{
    [TestCase("en")]
    [TestCase("it")]
    [TestCase("fr")]
    [TestCase("de")]
    public async Task ValidatePoliciesTitles(string lang)
    {
        try
        {
            var page = await Context.NewPageAsync();
            await page.GotoAsync("https://tokero.dev/en/");

            var footerPage = new FooterPage(page);
            await footerPage.AcceptCookiesIfVisibleAsync();

            var homePage = new HomePage(page);
            await homePage.SwitchLanguageAsync(lang);

            await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
            await page.WaitForTimeoutAsync(1000);
            await footerPage.NavigateToPoliciesPageAsync();

            var policiesPage = new PoliciesPage(page);
            var policyTitles = await policiesPage.GetPolicyTitlesAsync();

            var rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
            var filePath = Path.Combine(rootDirectory!, "tokero-automation-tests", "TestData", $"policy-titles-{lang}.txt");
            var expectedPolicyTitles = (await File.ReadAllLinesAsync(filePath))
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            Assert.That(policyTitles, Is.Not.Empty, "No policy titles were found.");

            foreach (var expectedTitle in expectedPolicyTitles)
            {
                Assert.That(policyTitles.Contains(expectedTitle),
                    $"Expected policy title '{expectedTitle}' was not found in the actual list.");
            }

            Test.Pass($"ValidatePoliciesTitles ({lang}) passed successfully.");
        }
        catch (Exception ex)
        {
            Test.Fail($"ValidatePoliciesTitles ({lang}) failed: {ex.Message}");
            throw;
        }
    }

    [TestCase("en")]
    [TestCase("it")]
    [TestCase("fr")]
    [TestCase("de")]
    public async Task ValidatePoliciesLinks(string lang)
    {
        try
        {
            var page = await Context.NewPageAsync();
            await page.GotoAsync("https://tokero.dev/en/");

            var footerPage = new FooterPage(page);
            await footerPage.AcceptCookiesIfVisibleAsync();

            var homePage = new HomePage(page);
            await homePage.SwitchLanguageAsync(lang);

            await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
            await page.WaitForTimeoutAsync(1000);

            await footerPage.NavigateToPoliciesPageAsync();

            var policyPage = new PoliciesPage(page);
            var policyLinks = await policyPage.GetPolicyLinksAsync();

            Assert.That(policyLinks, Is.Not.Empty, $"No policy links found for language '{lang}'.");

            var policyLinkPattern = $@"^https:\/\/tokero\.dev\/{lang}\/[a-z0-9\-\/]+$";

            foreach (var link in policyLinks)
            {
                Assert.That(Regex.IsMatch(link, policyLinkPattern),
                    $"Policy link '{link}' does not match the expected pattern for language '{lang}'.");

                var response = await page.GotoAsync(link, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });

                Assert.That(response?.Status, Is.EqualTo(200),
                    $"Page '{link}' did not return status 200. Actual: {response?.Status}");
            }

            Test.Pass($"ValidatePoliciesLinks ({lang}) passed successfully.");
        }
        catch (Exception ex)
        {
            Test.Fail($"ValidatePoliciesLinks ({lang}) failed: {ex.Message}");
            throw;
        }
    }

    [Test]
    [Category("Performance")]
    public async Task EachPolicyPageShouldLoadUnder2Seconds()
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

            var policyPage = new PoliciesPage(page);
            var policyLinks = await policyPage.GetPolicyLinksAsync();

            Assert.That(policyLinks, Is.Not.Empty, "No policy links found on the policies page.");

            foreach (var link in policyLinks)
            {
                var stopwatch = Stopwatch.StartNew();

                var response = await page.GotoAsync(link, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });

                stopwatch.Stop();
                var loadTime = stopwatch.ElapsedMilliseconds;

                Assert.That(response?.Status, Is.EqualTo(200),
                    $"Page '{link}' did not return status 200. Actual: {response?.Status}");

                Assert.That(loadTime, Is.LessThan(2000),
                    $"Page '{link}' took too long to load: {loadTime} ms (limit: 2000 ms)");
            }

            Test.Pass("EachPolicyPageShouldLoadUnder2Seconds passed successfully.");
        }
        catch (Exception ex)
        {
            Test.Fail($"EachPolicyPageShouldLoadUnder2Seconds failed: {ex.Message}");
            throw;
        }
    }
}
