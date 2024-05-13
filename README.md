Luxa

Instrukcja:
1. Połączenie kodu z bazą danych (Podejście code first)
	- Utwórz nową bazę danych o np. nazwie LuxaDb 
	(Widok->SQL Server Object Explorer -> (wybierz dostępny serwer np. SQL EXPRESS albo LocalDB (Istnieje bez instalacji czegokolwiek))-> Databases -> Add new database)
	- Rozwiń drzewo utworzonej bazy danych i wejdź w właściwości, wyszukaj connection string i skopiuj całość
	- Wklej w appsettings.json w cudzysłowach przy LuxaDb
	- Wejdź w konsole menadżera pakietów wpisz add-migration "dowolna nazwa" i update-database
	- Smacznego
2. Ścieżki do łączenia się do stron
	- SignUp: ~/Account/SignUp lub ~/SignUp
	- SignIn: ~/Account/SignIn lub ~/SignIn
	- CreateUser: ~/Account/CreateUser
	- UsersList: ~/Account/UsersList