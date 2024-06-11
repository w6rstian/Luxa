using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;

namespace Luxa.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;
        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //true - wszystkie dodane,
        //false - min jeden już istnieje w bazie danych/nie został dodany
        //null żaden nie został dodany
        public bool? AddAll(List<string> tags)
        {
            bool isAtLeastOneAdded = false;
            bool isAtLeastOneNotAdded = false;
            foreach (var item in tags)
            {
                bool returned = true;//AddIfDifferent(item);
                if (returned) isAtLeastOneAdded = true;
                else isAtLeastOneNotAdded = true;
            }
            if (isAtLeastOneAdded && !isAtLeastOneNotAdded)
            {
                return true;
            }
            else if (!isAtLeastOneAdded && isAtLeastOneNotAdded)
            {
                return null;
            }
            else
            {
                return false;
            }
        }
        /*public bool AddIfDifferent(string tag)
		{

		}*/
        public bool Delete() { throw new NotImplementedException(); }
        public bool Save()
            => _context.SaveChanges() > 0;

        public bool IsTagExist(string tag)
            => _context.Tags.Any(t => t.TagName == tag);

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public bool Add(TagModel tag)
        {
            _context.Tags.Add(tag);
            return Save();
        }

        public List<TagModel> GetTagsFromCollection(List<string> tagsList)
            => _context.Tags
                .Where(t => tagsList.Contains(t.TagName))
                .ToList();

    }
}
