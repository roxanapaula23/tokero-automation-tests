using Microsoft.Playwright;

namespace tokero_automation_tests.tokero_automation_tests.Pages;

public class PoliciesPage
{
    private readonly IPage _page;
    private readonly string _policyTitlesSelector = "blazor-policies_v5 h4";
    private readonly string _policyLinksSelector = "blazor-policies_v5 a";

    public PoliciesPage(IPage page)
    {
        _page = page;
    }

    public async Task<string[]> GetPolicyTitlesAsync()
    {
        await _page.WaitForSelectorAsync(_policyTitlesSelector);
        return await _page.EvalOnSelectorAllAsync<string[]>(
            _policyLinksSelector,
            "elements => elements.map(e => e.textContent.trim())"
        );
    }

    public async Task<string[]> GetPolicyLinksAsync()
    {
        await _page.WaitForSelectorAsync(_policyLinksSelector);
        return await _page.EvalOnSelectorAllAsync<string[]>(
            _policyLinksSelector,
            "elements => elements.map(e => e.href)"
        );
    }
}