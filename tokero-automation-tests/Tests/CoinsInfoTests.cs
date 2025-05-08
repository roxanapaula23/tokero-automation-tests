using System.Collections.Concurrent;
using System.Diagnostics;
using Allure.NUnit;
using Microsoft.Playwright;
using NUnit.Framework;
using tokero_automation_tests.tokero_automation_tests.Pages;
using tokero_automation_tests.tokero_automation_tests.Utils;

namespace tokero_automation_tests.tokero_automation_tests.Tests;

[AllureNUnit]
public class CoinsInfoTests
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
    public async Task TestCardanoAdaPages()
    {
        var page = await _context.NewPageAsync();
        await page.GotoAsync("https://tokero.dev/en/");

        var footerPage = new FooterPage(page);
        await footerPage.AcceptCookiesIfVisibleAsync();

        var coinsInfoPage = new CoinsInfoPage(page);
        await coinsInfoPage.NavigateToCoinsInfoPageAsync();
        await coinsInfoPage.ClickLayer1ButtonAsync();
        await coinsInfoPage.ClickCardanoAdaLinkAsync();

        var currentUrl = await coinsInfoPage.GetCurrentUrlAsync();
        TestContext.WriteLine($"Navigated to URL: {currentUrl}");
        Assert.That(currentUrl, Does.Contain("/en/info/cardano-ada/"),
            "Expected to be on the Cardano ADA info page.");

        await coinsInfoPage.ClickCardanoExternalLinkAsync();

        var (isVisible, href) = await coinsInfoPage.GetCardanoExternalLinkDetailsAsync();
        Assert.That(isVisible, Is.True, "Cardano external link should be visible.");
        Assert.That(href, Is.EqualTo("https://cardano.org/"), "The Cardano link href should match the expected URL.");
    }

    [Test]
    [TestCase(3)]
    [Category("Load")]
    public async Task LoadTimeCardanoAdaForMultipleTotalUser(int totalUsers)
    {
        const int latencyLimitMs = 10000;
        var failures = new ConcurrentBag<string>();

        var tasks = Enumerable.Range(0, totalUsers).Select(async userId =>
        {
            try
            {
                var page = await _context.NewPageAsync();

                var stopwatch = Stopwatch.StartNew();
                var response = await page.GotoAsync("https://tokero.dev/en/info/cardano-ada/", new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });
                stopwatch.Stop();

                var elapsed = stopwatch.ElapsedMilliseconds;

                if (response?.Status != 200)
                    failures.Add($"User {userId}: HTTP status {response?.Status}");

                if (!page.Url.Contains("/en/info/cardano-ada/"))
                    failures.Add($"User {userId}: Incorrect URL - {page.Url}");

                if (elapsed > latencyLimitMs)
                    failures.Add($"User {userId}: Load time {elapsed}ms exceeded {latencyLimitMs}ms");
            }
            catch (Exception ex)
            {
                failures.Add($"User {userId}: Exception - {ex.Message}");
            }
        });

        await Task.WhenAll(tasks);

        Assert.That(failures, Is.Empty, $"Failures:\n{string.Join("\n", failures)}");
    }
}