# 🍎 Entity Framework Core Commands for macOS

This guide contains the most commonly used Entity Framework Core commands when working on **macOS** with **Terminal**, **Visual Studio for Mac**, or **Visual Studio Code**.

---

# 📦 Install Entity Framework CLI

Check if EF CLI is installed:

```bash
dotnet ef
```

Install globally:

```bash
dotnet tool install --global dotnet-ef
```

Update EF CLI:

```bash
dotnet tool update --global dotnet-ef
```

Check installed version:

```bash
dotnet ef --version
```

---

# 📦 Install Required NuGet Packages

SQL Server Provider

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

EF Design Package

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Tools Package (optional)

```bash
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

Restore packages

```bash
dotnet restore
```

---

# 🏗 Build Project

Build project

```bash
dotnet build
```

Clean project

```bash
dotnet clean
```

---

# 🗄 Database First (Scaffolding)

Generate models from an existing database.

```bash
dotnet ef dbcontext scaffold "Server=localhost,1433;Database=AcademicRecordsDB;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models
```

Generate models and DbContext

```bash
dotnet ef dbcontext scaffold "Server=localhost,1433;Database=AcademicRecordsDB;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Data --context AcademicRecordsDBContext
```

Overwrite existing generated files

```bash
dotnet ef dbcontext scaffold "Server=localhost,1433;Database=AcademicRecordsDB;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Data --context AcademicRecordsDBContext --force
```

---

# 🔄 Code First Migrations

Create migration

```bash
dotnet ef migrations add InitialCreate
```

Example

```bash
dotnet ef migrations add AddStudents
```

```bash
dotnet ef migrations add AddRelationships
```

```bash
dotnet ef migrations add RenameStudentsCourses
```

List all migrations

```bash
dotnet ef migrations list
```

Remove the last migration (if not applied)

```bash
dotnet ef migrations remove
```

---

# 🗃 Update Database

Apply all pending migrations

```bash
dotnet ef database update
```

Update database to a specific migration

```bash
dotnet ef database update InitialCreate
```

Rollback database to previous migration

```bash
dotnet ef database update PreviousMigrationName
```

---

# ❌ Delete Database

Delete database

```bash
dotnet ef database drop
```

Delete database without confirmation

```bash
dotnet ef database drop --force
```

---

# 🔁 Typical Workflow

Create migration

```bash
dotnet ef migrations add InitialCreate
```

Update database

```bash
dotnet ef database update
```

Modify models

```bash
dotnet ef migrations add AddNewFeature
```

Update database again

```bash
dotnet ef database update
```

---

# 🔄 Rename Tables / Classes

After renaming classes or tables:

1. Rename the files.
2. Rename the classes.
3. Rename navigation properties.
4. Create a new migration.

```bash
dotnet ef migrations add RenameTables
```

```bash
dotnet ef database update
```

---

# 🛠 Common Errors

## Remove last migration

```bash
dotnet ef migrations remove
```

---

## Rollback database

```bash
dotnet ef database update PreviousMigrationName
```

---

## Rebuild project

```bash
dotnet clean
dotnet build
```

---

## Restore NuGet packages

```bash
dotnet restore
```

---

## Generate SQL Script

```bash
dotnet ef migrations script
```

Generate SQL script between two migrations

```bash
dotnet ef migrations script InitialCreate AddStudents
```

---

# 🚀 Helpful Commands

Project Information

```bash
dotnet --info
```

Installed SDKs

```bash
dotnet --list-sdks
```

Installed Runtimes

```bash
dotnet --list-runtimes
```

---

# 💡 Notes

- On **macOS** use **Terminal** instead of Package Manager Console.
- The command `Scaffold-DbContext` is for Visual Studio on Windows.
- The equivalent command on macOS is:

```bash
dotnet ef dbcontext scaffold
```

- SQL Server running in Docker usually uses:

```text
Server=localhost,1433;
User Id=sa;
Password=YOUR_PASSWORD;
TrustServerCertificate=True;
Encrypt=False;
```

instead of

```text
Trusted_Connection=True;
```