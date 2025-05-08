using System.Text.RegularExpressions;
using Microsoft.Playwright;

public class HomePage
{
    private readonly IPage _page;

    public HomePage(IPage page)
    {
        _page = page;
    }

    private readonly string _dropDownToggle =
        "button.languageSwitcher_dropdownToggle__YEf9b.languageSwitcher_topDropdownToggle__QXn26";

    private readonly string _italianSelector = "button.languageSwitcher_dropdownMenuOptionBtn__NON0i >> text=IT";
    private readonly string _germanSelector = "button.languageSwitcher_dropdownMenuOptionBtn__NON0i >> text=DE";
    private readonly string _frenchSelector = "button.languageSwitcher_dropdownMenuOptionBtn__NON0i >> text=FR";

    public async Task SwitchLanguageAsync(string lang)
    {
        await _page.WaitForSelectorAsync(_dropDownToggle);
        await _page.Locator(_dropDownToggle).ClickAsync();

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

    public string CurrentUrl => _page.Url;
}