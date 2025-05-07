using Microsoft.Playwright;
using NUnit.Framework;

namespace tokero_automation_tests.tokero_automation_tests.Pages;

public class PolicyPage
{
    private readonly IPage _page;
    private readonly string _policyHeaderTileSelector = "h1";

    public PolicyPage(IPage page)
    {
        _page = page;
    }

    public async Task<string> GetPolicyPageHtmlContentAsync()
    {
        await _page.WaitForSelectorAsync(_policyHeaderTileSelector);
        return await _page.EvaluateAsync<string>(
            "document.documentElement.outerHTML");
    }
}