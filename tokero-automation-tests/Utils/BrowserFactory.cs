using Microsoft.Playwright;

namespace tokero_automation_tests.tokero_automation_tests.Utils;

public class BrowserFactory
{
    private IPlaywright _playwright;
    private IBrowser _browser;

    public async Task<IBrowser> GetBrowserAsync()
    {
        var rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
        var propertiesFileName = Environment.GetEnvironmentVariable("PROPERTIES_FILE_NAME") ?? "properties.json";
        var properties = PropertiesReader.Load(rootDirectory + "/tokero-automation-tests/Config/" + propertiesFileName);
        
        _playwright = await Playwright.CreateAsync();

        switch (properties?.Browser)
        {
            case "chromium":
                _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = properties?.Headless,
                    Channel = properties?.Browser 
                });
                break;
            case "firefox":
                _browser = await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = properties?.Headless,
                    Channel = properties?.Browser 
                });
                break;
            case "webkit":
                _browser = await _playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions()
                {
                    Headless = properties?.Headless,
                    Channel = properties?.Browser 
                });
                break;
            default:
                _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = properties?.Headless,
                    Channel = properties?.Browser 
                });
                break;
        }
        return _browser;
    }

    public async Task DisposeAsync()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
        }
        _playwright?.Dispose();
    }
}