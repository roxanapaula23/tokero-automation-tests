using Microsoft.Playwright;

namespace tokero_automation_tests.tokero_automation_tests.Pages
{
    public static class PageFlowHelper
    {
        public static async Task NavigateToPoliciesPageWithSetupAsync(IPage page, string lang)
        {
            await page.GotoAsync("https://tokero.dev/en/");

            var footerPage = new FooterPage(page);
            await footerPage.AcceptCookiesIfVisibleAsync();

            var homePage = new HomePage(page);
            await homePage.SwitchLanguageAsync(lang);

            await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
            await page.WaitForTimeoutAsync(1000);

            await footerPage.NavigateToPoliciesPageAsync();
        }
    }
}
