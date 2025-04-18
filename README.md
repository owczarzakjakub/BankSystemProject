BankEase to aplikacja konsolowa napisana w języku C#, która symuluje prosty system bankowy. Umożliwia użytkownikom logowanie się oraz rejestrację nowych kont. 
Po zalogowaniu użytkownik ma dostęp do funkcji takich jak tworzenie i usuwanie kont bankowych, wpłaty, wypłaty, przelewy oraz sprawdzanie salda. 
Dostępna jest również historia transakcji dla danego konta. 
Administratorzy mają rozszerzone uprawnienia, które pozwalają na zarządzanie użytkownikami i kontami, w tym tworzenie nowych użytkowników, ich usuwanie oraz modyfikowanie sald na kontach. 
System opiera się na modelu RBAC (Role-Based Access Control), który kontroluje dostęp do funkcji w zależności od przypisanej roli, takiej jak Admin, Manager, Employee czy User.
Aby uruchomić projekt, wymagane jest zainstalowanie środowiska .NET (np. .NET 6 lub nowszego). 
Należy otworzyć projekt w Visual Studio lub innym edytorze obsługującym C#, a następnie zbudować aplikację. 
Program korzysta z pliku tekstowego Users.txt jako prostej bazy danych, który powinien znajdować się w katalogu C:\Users\User\source\repos\BankEase\Data\Users.txt. 
W razie potrzeby można zmienić ścieżkę do pliku w kodzie źródłowym. Po poprawnym zbudowaniu aplikacji można ją uruchomić poprzez Visual Studio lub za pomocą polecenia dotnet run w terminalu.
Projekt BankEase został zorganizowany w sposób modułowy i zawiera osobne klasy dla modeli danych (takich jak użytkownik, klient, konto, transakcja), usług logiki biznesowej (logowanie, rejestracja, obsługa kont i transakcji), systemu uprawnień RBAC oraz obsługi pliku danych. 
Aplikacja jest czytelna, łatwa do rozbudowy i idealnie nadaje się jako projekt edukacyjny lub podstawa do budowy bardziej złożonych systemów bankowych.

