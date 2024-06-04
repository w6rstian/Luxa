using Luxa.Models;

namespace Luxa.Interfaces
{
	public interface ITagRepository
	{
		bool IsTagExist(string tag);
		bool Add(TagModel tag);
		List<TagModel> GetTagsFromCollection(List<string> tagsList);

	}
}
