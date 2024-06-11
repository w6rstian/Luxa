using Microsoft.AspNetCore.Mvc;

namespace Luxa.Components
{
    public class SettingsMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync() => View();

    }
}
