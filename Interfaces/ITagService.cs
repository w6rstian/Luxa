using Luxa.Models;

namespace Luxa.Interfaces
{
	public interface ITagService
	{
		bool Add(string tags);
		List<TagModel> GetTagsFromString(string tags);
	}
}
