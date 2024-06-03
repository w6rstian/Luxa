//using Luxa.Data;
//using Microsoft.EntityFrameworkCore;

//namespace Luxa.Repository
//{
//	public class TagRepository : ITagsRepository
//	{
//		private readonly ApplicationDbContext _context;
//		public TagRepository(ApplicationDbContext context) 
//		{
//			_context = context;
//		}
//		//true - wszystkie dodane,
//		//false - min jeden już istnieje w bazie danych/nie został dodany
//		//null żaden nie został dodany
//		public bool? AddAll(List<string> tags) 
//		{
//			bool isAtLeastOneAdded = false;
//			bool isAtLeastOneNotAdded = false;
//			foreach (var item in tags)
//			{
//				bool returned = AddIfDifferent(item);
//				if (returned)  isAtLeastOneAdded = true; 
//				else isAtLeastOneNotAdded = true;
//			}
//			if (isAtLeastOneAdded && !isAtLeastOneNotAdded)
//			{
//				return true;
//			}
//			else if (!isAtLeastOneAdded && isAtLeastOneNotAdded)
//			{
//				return null;
//			}
//			else
//			{
//				return false;
//			}
//		}
//		public bool AddIfDifferent(string tag) 
//		{
			
//		}
//		public bool Delete() { throw new NotImplementedException();}
//		public bool Save() 
//			=> _context.SaveChanges() > 0;
//		public bool Update() { throw new NotImplementedException();
//		}
//	}
//}
