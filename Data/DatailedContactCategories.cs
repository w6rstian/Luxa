using Luxa.Data.Enums;

namespace Luxa.Data
{
    public static class DatailedContactCategories
    {
        //Zgłoś błąd
        public static (CategoryOfContact, string) DisplayDesignError = (CategoryOfContact.Error, "Stona wyświetla się niepoprawnie");
        public static (CategoryOfContact, string) DisplaySiteError = (CategoryOfContact.Error, "Strona się nie wyświetla");
        public static (CategoryOfContact, string) DisplayFormError = (CategoryOfContact.Error, "Formularz wyświetla niepoprawnie dane");
        public static (CategoryOfContact, string) AddPhotoError = (CategoryOfContact.Error, "Wystąpił problem podczas dodania zdjęć");
        public static (CategoryOfContact, string) ChangeDataError = (CategoryOfContact.Error, "Nastąpił problem podczas zmiany ustawień/danych");
        //Nawiąż kontakt
        public static (CategoryOfContact, string) SuggestionContact = (CategoryOfContact.Contact, "Propozycja zmiany na stronie");
        public static (CategoryOfContact, string) BusinessContact = (CategoryOfContact.Contact, "Nawiązanie współpracy biznesowej");
        public static (CategoryOfContact, string) ChangeDataContact = (CategoryOfContact.Contact, "Zmiana danych niemożliwa z poziomu ustawień");



    }
}
