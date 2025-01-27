# TranslationTabsDemo

## Overview

TranslationTabsDemo is a modular ASP.NET Core application designed using **Layered Architecture**. This architecture separates the application into distinct layers such as Application, Domain, and Infrastructure, ensuring maintainability and scalability. The application supports multi-language translation forms and implements core design patterns like Repository Pattern, Unit of Work, Dependency Injection, and Result Pattern.

## Features
- **Layered Architecture**: Clear separation of responsibilities into Application, Domain, and Infrastructure layers.
- **Repository and Unit of Work Patterns**: Efficient and testable data access.
- **Custom Mapping**: AutoMapper integration for DTOs and entities.
- **SaveChanges Interceptor**: Custom logic during database operations.
- **Dependency Injection**: Easy-to-manage services and dependencies.
- **Result Pattern**: Consistent handling of operation results.


## Quick Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/harunozbilgi/TranslationTabsDemo.git
   cd TranslationTabsDemo
   ```
2. Update the database connection string in `appsettings.json`.
3. Apply migrations:
   ```bash
   dotnet ef database update
   ```
4. Run the application:
   ```bash
   dotnet run
   ```


