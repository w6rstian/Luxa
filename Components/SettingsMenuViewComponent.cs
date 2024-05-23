using Microsoft.AspNetCore.Mvc;

namespace Luxa.Components
{
	public class SettingsMenuViewComponent : ViewComponent
	{
		public IViewComponentResult InvokeAsync()
		{
			return View();
		}
	}
}
