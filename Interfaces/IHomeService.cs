using Microsoft.AspNetCore.Mvc.Rendering;

namespace Luxa.Interfaces
{
	public interface IHomeService
	{
		List<SelectListItem> GetOrderBySelectListItem();
	}
}
