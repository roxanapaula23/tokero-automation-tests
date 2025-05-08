using Microsoft.Playwright;

namespace tokero_automation_tests.tokero_automation_tests.Pages;

public class CoinsInfoPage
{
    private readonly IPage _page;

    private readonly string _coinsInfoLinkSelector = "a.footerLink_linkItem__vHH3t[title='Coins info']";
    private readonly string _layer1ButtonSelector = "button:has-text('Layer1')";
    private readonly string _cardanoAdaLinkSelector = "a[href='/en/info/cardano-ada/']";
    private readonly string _cardanoLinkSelector = "a[href='https://cardano.org/']";

    public CoinsInfoPage(IPage page)
    {
        _page = page;
    }

    public async Task NavigateToCoinsInfoPageAsync()
    {
        await _page.ClickAsync(_coinsInfoLinkSelector);
    }

    public async Task ClickLayer1ButtonAsync()
    {
        await _page.ClickAsync(_layer1ButtonSelector);
    }

    public async Task ClickCardanoAdaLinkAsync()
    {
        await _page.ClickAsync(_cardanoAdaLinkSelector);
    }

    public async Task<string> GetCurrentUrlAsync()
    {
        return _page.Url;
    }

    public async Task ClickCardanoExternalLinkAsync()
    {
        var cardanoLink = _page.Locator(_cardanoLinkSelector);
        await cardanoLink.ClickAsync();
    }

    public async Task<(bool isVisible, string href)> GetCardanoExternalLinkDetailsAsync()
    {
        var cardanoLink = _page.Locator(_cardanoLinkSelector);
        await cardanoLink.ScrollIntoViewIfNeededAsync();
        var isVisible = await cardanoLink.IsVisibleAsync();
        var href = await cardanoLink.GetAttributeAsync("href");
        return (isVisible, href);
    }
}