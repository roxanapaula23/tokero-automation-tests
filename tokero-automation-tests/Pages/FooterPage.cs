using Microsoft.Playwright;

namespace tokero_automation_tests.tokero_automation_tests.Pages;

public class FooterPage
{
    private readonly IPage _page;
    private readonly string _acceptCookiesSelector = "button:has-text('Accept all cookies')";
    private readonly string _policiesLinkSelector = "a[href$='/policies/']";

    public FooterPage(IPage page)
    {
        _page = page;
    }

    public async Task AcceptCookiesIfVisibleAsync()
    {
        var cookieButton = _page.Locator(_acceptCookiesSelector);
        if (await cookieButton.IsVisibleAsync())
        {
            await cookieButton.ClickAsync();
        }
    }

    public async Task NavigateToPoliciesPageAsync()
    {
        await _page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
        await _page.WaitForTimeoutAsync(1000);

        await _page.ClickAsync(_policiesLinkSelector);
    }
}