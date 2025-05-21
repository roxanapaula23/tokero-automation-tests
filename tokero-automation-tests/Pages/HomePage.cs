using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace tokero_automation_tests.tokero_automation_tests.Pages;

public class HomePage
{
    private readonly IPage _page;

    public HomePage(IPage page)
    {
        _page = page;
    }

    private readonly string _langToggleSelector = "xpath=//button[.//span[text()='EN']]";
    private readonly string _italianSelector = "a[href='/it/']";
    private readonly string _germanSelector = "a[href='/de/']";
    private readonly string _frenchSelector = "a[href='/fr/']";

    public async Task SwitchLanguageAsync(string lang)
    {
        await _page.WaitForSelectorAsync(_langToggleSelector);
        await _page.Locator(_langToggleSelector).ClickAsync();

        switch (lang.ToLowerInvariant())
        {
            case "it":
                await _page.Locator(_italianSelector).ClickAsync();
                break;
            case "en":
                break;
            case "de":
                await _page.Locator(_germanSelector).ClickAsync();
                break;
            case "fr":
                await _page.Locator(_frenchSelector).ClickAsync();
                break;
            default:
                throw new ArgumentException($"Unsupported language code: {lang}", nameof(lang));
        }
        await _page.WaitForURLAsync(new Regex($"/{lang}/", RegexOptions.IgnoreCase));
    }
}