# Waterlily Labs - C# Web Full-Stack Development Test
![Screenshot 2025-06-06 140719](https://github.com/user-attachments/assets/20be5fd6-c332-4cc1-b69f-da9e10be8f02)

This project is a submission for the C# Web Full-Stack Development Test by Waterlily Labs. It's an ASP.NET Core MVC (.NET 8) application demonstrating skills in web development, database interaction, layered architecture, caching, and business logic implementation.

## Features
![Screenshot 2025-06-06 141631](https://github.com/user-attachments/assets/d2fcfd80-606e-4a1e-aded-d4437ff681ae)

*   **Employee Management (CRUD):**
    *   Create, Read, Update, and Delete employee records.
    *   Employee records consist of: Id, Name, Email, and Job Position.
 
      ![Screenshot 2025-06-06 141914](https://github.com/user-attachments/assets/5850e454-39c5-4ba0-9d1d-5eeb8d98662a)

*   **Working Days Calculator:**
    *   A page to calculate the number of working days between two specified dates (inclusive).
    *   Considers weekends (Saturdays and Sundays are non-working days).
    *   Excludes public holidays (fetched from a database table).
    *   The start date for calculation must be a weekday.

## Technical Stack & Architecture

*   **Platform:** ASP.NET Core MVC (.NET 8)
*   **Language:** C#
*   **Database:** SQL Server (designed to work with any RDBMS supported by EF Core)
*   **Data Access:** Entity Framework Core (EF Core) 8
    *   Database-First approach (`Scaffold-DbContext` used to generate models from an existing schema).
    *   LINQ to Entities for querying.
*   **Architecture:** Layered Architecture
    *   **Presentation Layer:** ASP.NET Core MVC Controllers and Razor Views.
    *   **Service Layer:** Contains business logic (`EmployeeService`, `WorkingDaysService`).
    *   **Repository Layer:** Abstracted data access using Generic Repository Pattern (`IRepository<T>`, `Repository<T>`) and specific repositories (`IEmployeeRepository`, `IPublicHolidayRepository`).
*   **Caching:**
    *   In-memory caching (`IMemoryCache`) implemented via a dedicated `CachingService`.
    *   `CachedAsync<T>`: Caches data for a specified duration (e.g., 5 minutes for employee list).
    *   `CachedLongAsync<T>`: Caches data for an extended period (e.g., individual employee details, "indefinitely" for the scope of the application session or until invalidated).
*   **Delegates:**
    *   Used for a simple notification pattern (e.g., `EntityChangedNotification` delegate in `EmployeeService` to signal changes to employee data).
*   **UI / Scripting:**
    *   **Bootstrap 5:** For styling and responsive layout.
    *   **jQuery:** For client-side enhancements, primarily used by ASP.NET Core's unobtrusive validation.
    *   Custom CSS can be added to `wwwroot/css/site.css`.


## Setup and Running the Project

1.  **Prerequisites:**
    *   .NET 8 SDK
    *   SQL Server (Express, LocalDB, or any other edition)
    *   Visual Studio 2022 (recommended) or any other .NET IDE/editor.

2.  **Database Setup:**
    *   Create a new database in SQL Server (e.g., `WaterlilyDB`).
    *   Execute the SQL script provided in `DatabaseSetup.sql` (or see below) to create the `Employees` and `PublicHolidays` tables and insert some sample data.

    ```sql
    -- File: DatabaseSetup.sql (Example content - ensure you have this or similar)
    -- Ensure you are connected to your SQL Server instance
    -- and have created a database (e.g., WaterlilyDB) before running this.

    -- USE YourDatabaseName; -- Replace YourDatabaseName if needed
    -- GO

    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' and xtype='U')
    BEGIN
        CREATE TABLE Employees (
            Id INT PRIMARY KEY IDENTITY(1,1),
            Name NVARCHAR(100) NOT NULL,
            Email NVARCHAR(100) NOT NULL,
            JobPosition NVARCHAR(100) NOT NULL
        );
        PRINT 'Table Employees created.';
    END
    GO

    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PublicHolidays' and xtype='U')
    BEGIN
        CREATE TABLE PublicHolidays (
            Id INT PRIMARY KEY IDENTITY(1,1),
            HolidayDate DATE NOT NULL UNIQUE,
            Description NVARCHAR(200) NOT NULL
        );
        PRINT 'Table PublicHolidays created.';
    END
    GO

    -- Insert some sample holidays for testing
    IF NOT EXISTS (SELECT 1 FROM PublicHolidays WHERE HolidayDate = '2024-01-01')
        INSERT INTO PublicHolidays (HolidayDate, Description) VALUES ('2024-01-01', 'New Year''s Day 2024');
    IF NOT EXISTS (SELECT 1 FROM PublicHolidays WHERE HolidayDate = '2024-12-25')
        INSERT INTO PublicHolidays (HolidayDate, Description) VALUES ('2024-12-25', 'Christmas Day 2024');
    IF NOT EXISTS (SELECT 1 FROM PublicHolidays WHERE HolidayDate = '2021-01-01')
        INSERT INTO PublicHolidays (HolidayDate, Description) VALUES ('2021-01-01', 'Test New Year''s Day 2021'); -- For calculation example
    PRINT 'Sample public holidays inserted (if not already present).';
    GO

    -- Insert some sample employees
    IF NOT EXISTS (SELECT 1 FROM Employees WHERE Email = 'alice@example.com')
        INSERT INTO Employees (Name, Email, JobPosition) VALUES ('Alice Smith', 'alice@example.com', 'Software Engineer');
    IF NOT EXISTS (SELECT 1 FROM Employees WHERE Email = 'bob@example.com')
        INSERT INTO Employees (Name, Email, JobPosition) VALUES ('Bob Johnson', 'bob@example.com', 'Project Manager');
    PRINT 'Sample employees inserted (if not already present).';
    GO
    ```

3.  **Configure Connection String:**
    *   Open `appsettings.json` (and `appsettings.Development.json` if it exists).
    *   Update the `DefaultConnection` string to point to your SQL Server instance and the database you created.
        Example for SQL LocalDB:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=WaterlilyDB;Trusted_Connection=True;TrustServerCertificate=True;"
        }
        ```
        Example for SQL Server Express:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=YourServerName\\SQLEXPRESS;Database=WaterlilyDB;Trusted_Connection=True;TrustServerCertificate=True;"
        }
        ```

4.  **Build and Run:**
    *   Open the solution (`WaterlilyLabs.sln`) in Visual Studio.
    *   Build the solution (Build > Build Solution).
    *   Run the project (Debug > Start Debugging, or press F5).
    *   The application should open in your default web browser.

## Key Code Highlights

*   **Repository Pattern:** See `Repositories/IRepository.cs` and `Repositories/Repository.cs`.
*   **Service Layer:** Business logic resides in `Services/EmployeeService.cs` and `Services/WorkingDaysService.cs`.
*   **Caching Implementation:** `Services/CachingService.cs` demonstrates the `CachedAsync` and `CachedLongAsync` methods.
*   **Working Days Calculation Logic:** Found in `Services/WorkingDaysService.cs`.
*   **Delegate Usage:** `Services/Delegates/NotificationDelegates.cs` and its implementation in `EmployeeService`.



