using System.ComponentModel.DataAnnotations;

namespace Luxa.Data.Enums
{
    public enum CategoryOfPhotos
    {
		[Display(Name = "Inna")]
		None = 0,
		[Display(Name = "Zwierzęta")]
		Animals = 1,
		[Display(Name = "Ludzie")]
		People = 2,
		[Display(Name = "Motoryzacja")]
		Automotive = 3,
		[Display(Name = "Przyroda")]
		Nature = 4,
		[Display(Name = "Przedmioty")]
		Things = 5,
		[Display(Name = "Moda")]
		Style =6,
		[Display(Name = "Jedzenie")]
		Food = 7,
		[Display(Name = "Rękodzieła")]
		Drawnings = 8,
		[Display(Name = "Reklama")]
		Advertisement = 9,
		[Display(Name = "Gry")]
		Games = 10,        
    }
}
