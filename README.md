# Dragon Ball API Sync App 🐲
#### 👩‍💻 Author: *María José Menjivar*
---

An ASP.NET Core project following Clean Architecture principles that syncs characters and transformations from the [Dragon Ball API](https://dragonball-api.com/) and stores them in a SQL Server database. 

## Descrpition
This application syncs Dragon Ball characters, filtering only **Saiyans** with **Z Fighter** affiliation, and saves their transformations to a SQL Server database.  
A `Seeder` runs this logic automatically when the app starts.

## 🧱 Project Architecture

- **DragonBallAPI.Core** → Entities (`Character`, `Transformation`)
- **DragonBallAPI.Application** → Reserved for future business logic
- **DragonBallAPI.Infrastructure** → Data access layer with EF Core + Seeder logic
- **DragonBallAPI.API** → Main project that exposes the API and triggers the seeder

## 🛠️ Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- Internet connection (required to access the external API)

---
> ⚠️ **IMPORTANT:** Before proceeding, make sure to create the database named `DragonBallDB`.
>
> You can do this using SQL Server Management Studio or by running:
>
> ```sql
> CREATE DATABASE DragonBallDB;
> ```
---
## ⚙️ How to Run the Project
### 1. Clone the repository

```bash
git clone https://github.com/your-username/dragonball-api-sync.git
cd dragonball-api-sync

```
> ✅ **Set `DragonBallAPI` as the Startup Project:**
>
> In Visual Studio, right-click on the `DragonBallAPI` project and select **"Set as Startup Project"**.

---

### 2. Configure the database connection

Update the `DragonBallAPI.API/appsettings.json` file with your SQL Server details:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=DragonBallDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

```
**⚠️ Make sure to replace `localhost` and DragonBallDB with your actual SQL Server instance name and preferred database name.**

---
### 3. Update the Database using NuGet Package Manager Console:
Open the **Package Manager Console** (`Tools > NuGet Package Manager > Package Manager Console`) and run:
```bash
Update-Database
```
This applies the EF Core migrations and sets up the required tables (`Characters` and `Transformations`).

---
### 4. Run the application
Once everything is configured, start the application with:

```bash
dotnet run --project DragonBallAPI.API
```

The application will automatically trigger the `SeederFromApi` during startup.
---
### 5. Seeder behavior
The seeder checks the database before inserting:

✅ If both Characters and Transformations tables are empty, it will:

- Fetch characters from the Dragon Ball API

- Filter only Saiyan characters with Z Fighter affiliation

- Store their associated transformations

**⚠️ If tables already contain data, it will not proceed and return:**

```pgsql
The database already contains data. Please clean the tables before syncing again.

```

**✅ If tables are empty and sync is successful:**

```rust
Sync completed successfully.

```

### 6. Verify the inserted data (Optional)
You can verify the inserted data by connecting to SQL Server and executing:

```sql
SELECT * FROM Characters;
SELECT * FROM Transformations;

```

Or by checking the output in your database management tool (like SSMS or Azure Data Studio).


## 🧪 Useful commands

```bash
# Crear una migración
dotnet ef migrations add NombreMigracion -s DragonBallAPI.API -p DragonBallAPI.Infrastructure

# Aplicar migraciones
dotnet ef database update -s DragonBallAPI.API -p DragonBallAPI.Infrastructure

# Ejecutar la app
dotnet run --project DragonBallAPI.API

```
