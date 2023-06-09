# Labb3 Databaser .NET22

This was a project done as part of the "Utveckling mot databas och databasadministration" course at IT-Högskolan.

The task was to develop an application that communicates with a MongoDB database using MongoDb.Driver. The choice was to either create a console or a WPF application. I chose to create a WPF application because I enjoyed working with WPF in previous school projects and I had grown especially fond with working in WPF using the MVVM design pattern. 

The application connects to a MongoDB database locally on the users machine thus requiring the user to have a MongoDB server installed and running on their device. In the root folder of the project there is a folder called 'Collections' which contain JSON documents containing collections which you need to export into a local MongoDB database to get the most out of the application.

The instructions given to us for the assignment were the following (in Swedish):

# Labb 3 - Applikation med MongoDB Code First
## Utveckla en databasapp med MongoDB.Driver
I denna övning utvecklar vi en applikation i C# som använder MongoDB.Driver
för att låta användaren läsa och uppdatera data i en databas. Ni väljer själva om ni
vill göra en Console- eller WPF-App, så länge funktionaliteten är på plats.
Ni kan välja på ett av de föreslagna projekten nedan, eller hitta på en egen idé.
## Förslag 1
Skriv om eran Quiz-applikation från Labb 3 i C# för att använda MongoDB istället för json.
Detta ska göras med en collection för frågor(en frågebank) och en för quizzar.
Användaren ska kunna lista frågor för de olika quizzarna, samt
kunna lägga till och ta bort frågor från quizzarna. När man lägger till frågor ska
man kunna välja från samtliga frågor som redan finns (i frågebanken).

För VG ska användaren dessutom kunna sätta kategorier på frågorna och sortera efter dessa,
och då kunna välja bland befintliga kategorier. Man ska även kunna lägga till nya
kategorier. När man lägger in nya frågor ska man kunna mata in allt som behövs för frågan.
När man ändrar i ett Quiz ska man kunna sortera frågor på kategorier och söka efter frågor baserat på namn.
## Förslag 2
Skriv om eran Butiks-applikation från Labb 2 i C# för att använda MongoDB.
Detta ska göras med en collection för produkter(en alla produkter) och en för användare med kundvagnar.
Man ska kunna handla så som man kunde i Labb 2 och läga till/ta bort produkter ur sortimentet. 
När användaren väljer att checka ut/bekräfta köp ska kundvagnen för den aktuella användaren tömmas.

För VG ska man implementera `butiker`. En butik ska ha sitt eget lager med produkter och antal av sådana.
När en användare väljer att handla ska hen kunna välja butik och sedan placera ordrar från den butiken.
Det behövs således en collection för butiker och en för ordrar.
## Egna projekt
Ni kan föreslå egna projekt, men kom ihåg att ni i så fall ska beskriva idén och
omfånget och få den godkänd av mig innan ni påbörjar utvecklingsarbetet.
Minst två tabeller och att användaren både kan läsa
och uppdatera data i databasen på något sätt.
Samt ytterligare någon eller några tabeller och mer funktionalitet.

För VG se förslag 1 och 2 ovan för att få en idé om hur stort omfånget på projektet bör vara. (Överenskommelse krävs)

Några förslag på idéer:

App där man kan bygga ihop lekar av pokemon-/magic-kort från en databas.

Kokbok-app där användare kan lägga in recept och kommentera och betygsätta. 

## Redovising
Detta är en individuell uppgift. Repo för genomförande finnes på: 

## Betygskriterier

### För godkänt krävs

* Man ska kunna utföra samtliga CRUD-operationer mot en MongoDB-databas med MongoDB.Driver
* Applikationen ska ansvara för att skapa det som behövs i MongoDB.
* Applikationen ska använda minst två collections.
* Det ska gå att clona repot och köra applikationen på ett smidigt sätt.

### För väl godkänt krävs:

* Samtliga krav för godkänt är uppfyllda.
* VG-uppgiften för det valda förslaget är uppfllt.

## Deadline är 22/01 23:59
### Sen inlämning kommer inte rättas och innebär komplettering i Maj.
