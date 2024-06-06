using Luxa.Interfaces;
using Luxa.Models;

namespace Luxa.Services
{
	public class TagService : ITagService
	{
		private readonly ITagRepository _tagRepository;
		public TagService(ITagRepository tagRepository)
		{
			_tagRepository = tagRepository;

		}
		private List<string> TagsToList(string tags)
            => tags?.Split(',').ToList() ?? new List<string>();
        public bool Add(string tags)
		{
			List<string> listTagsFromString = TagsToList(tags);
			foreach (var item in listTagsFromString)
			{
				if (!_tagRepository.IsTagExist(item))
				{
					TagModel tag = new()
					{
						TagName = item
					};
					_tagRepository.Add(tag);
				}

			}
			return true;
		}

		public List<TagModel> GetTagsFromString(string? tags)
		{
			List<string> listTagsFromString = TagsToList(tags);
			return _tagRepository.GetTagsFromCollection(listTagsFromString);
		}
	}
}
