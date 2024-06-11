using System.ComponentModel.DataAnnotations;

namespace Luxa.Data.Enums
{
    //Kategorie użytkowników zapisywane w bazie danych o dziwo w formie intów z przedziału <0,4>
    public enum CategoryOfContact
    {
        [Display(Name = "Zgłoś błąd")]
        Error = 0,
        [Display(Name = "Nawiąż kontakt")]
        Contact = 1,

    }
}
