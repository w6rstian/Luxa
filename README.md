Luxa

Instruction:
1. Connect your code to the database (Code first approach)

	- Create a new database with e.g. LuxaDb name (View->SQL Server Object Explorer -> (select an available server e.g. SQL EXPRESS or LocalDB (Exists without installing anything))-> Databases -> Add new database)
	- Expand the tree of the created database and go into properties, search for connection string and copy the whole thing
	- Paste in appsettings. json in quotes by LuxaDb
	- Go into the package manager console type add-migration "any-name" and update-database
	- Enjoy
2. For Google login to work you need to download the package "Microsoft.AspNetCore.Authentication.Google"
 	- https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.Google (accessed 13/06/2024)

Translated with DeepL.com (free version)

Instrukcja:
1. Połączenie kodu z bazą danych (Podejście code first)
	- Utwórz nową bazę danych o np. nazwie LuxaDb 
	(Widok->SQL Server Object Explorer -> (wybierz dostępny serwer np. SQL EXPRESS albo LocalDB (Istnieje bez instalacji czegokolwiek))-> Databases -> Add new database)
	- Rozwiń drzewo utworzonej bazy danych i wejdź w właściwości, wyszukaj connection string i skopiuj całość
	- Wklej w appsettings.json w cudzysłowach przy LuxaDb
	- Wejdź w konsole menadżera pakietów wpisz add-migration "dowolna nazwa" i update-database
	- Smacznego

2. Żeby logowanie Google działało należy pobrać pakiet "Microsoft.AspNetCore.Authentication.Google"
   	- https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.Google (dostęp: 13/06/2024)
