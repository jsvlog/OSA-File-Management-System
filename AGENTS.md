# Agent Coding Guidelines for OSA File Management System

Dual-project solution: WPF .NET 8.0 MVVM desktop app + ASP.NET Core 9.0 WebAPI. MySQL database integration.

---

## 1. Build, Lint, Test Commands

### Build
```bash
dotnet build "OSA File Management System.sln"                    # Full solution
dotnet build "OSA File Management System.sln" -c Release         # Release build
dotnet build "OSAWebAPI/OSAWebAPI.csproj"                        # WebAPI only
dotnet build "OSA File Management System/OSA File Management System.csproj"  # WPF only
```

### Lint
No explicit linter. Use default .NET SDK conventions. Address compiler warnings.

### Test
No test project exists. Create one with:
```bash
dotnet new xunit -n OSAFileManagementTests
cd OSAFileManagementTests
dotnet add reference "../OSA\ File\ Management\ System/OSA\ File\ Management\ System.csproj"
```
After tests exist:
```bash
dotnet test                                                    # All tests
dotnet test --filter "FullyQualifiedName~Namespace.Class.Method"  # Single test
dotnet test --filter "FullyQualifiedName~DocumentViewModel"     # Class tests
```

### Run
```bash
dotnet run --project "OSAWebAPI/OSAWebAPI.csproj"               # WebAPI on port 5000
```

---

## 2. Code Style Guidelines

### Imports (Using Directives)
Order: System → third-party → project. Blank line between groups. Alphabetical within.

```csharp
using System;
using System.Collections.ObjectModel;
using System.Windows;
using MySql.Data.MySqlClient;
using OSA_File_Management_System.Commands;
using OSA_File_Management_System.Model;
```

### Formatting
- **Indentation:** 4 spaces, no tabs
- **Braces:** K&R (opening on same line)
- **Blank lines:** Separate methods, properties, regions
- **Line length:** ~120 chars
- **Spacing:** `if (x == 0)`, `a, b`, `x + y`

### Naming Conventions
- **Namespaces:** `PascalCase` with underscores: `OSA_File_Management_System`, `OSA_File_Management_System.Model`
- **Classes/Interfaces/Enums:** `PascalCase`: `Document`, `INotifyPropertyChanged`, `RelayCommand`, `RegionComService`
- **Methods:** `PascalCase`: `LoadData`, `OnPropertyChanged`, `GetAllDocuments`
- **Properties:** `PascalCase`: `Id`, `DocumentList`, `SearchTextInventory`
- **Private Fields:** `camelCase`: `id`, `documentList`, `addFormData`, `_service` (WPF), `_connectionString` (WebAPI)
- **Commands:** `PascalCase` with "Command" suffix: `ShowAddFormCommand`, `DeleteDocumentCommand`
- **API Controllers:** `PascalCase` with "Controller" suffix: `RegionComController`

### MVVM Pattern (WPF)

**Models:** Implement `INotifyPropertyChanged`. Use `#region INotify`. All properties raise events.

```csharp
private int id;
public int Id { get { return id; } set { id = value; OnPropertyChanged("Id"); } }

#region INotify
public event PropertyChangedEventHandler? PropertyChanged;
private void OnPropertyChanged(string propertyName) {
    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
}
#endregion
```

**ViewModels:** Expose `RelayCommand` properties. Initialize in constructor. Use `#region` for features. `INotifyPropertyChanged` for ViewModel properties.

**Views:** Minimal code-behind (only `InitializeComponent`). Set `DataContext` in code-behind or XAML.

**Commands:** Use `RelayCommand` with/without parameter. Null-coalescing for lazy init.

```csharp
private RelayCommand addDocument;
public RelayCommand AddDocument => addDocument ??= new RelayCommand(AddDocumentMethod);
private void AddDocumentMethod() { /* impl */ }
private void EditMethod(object param) { /* with param */ }
```

### WebAPI Patterns

**Controllers:** Dependency inject services. Use `[HttpGet]` attributes. Return `IActionResult`. `NotFound()` for missing records.

```csharp
public class RegionComController : Controller {
    private readonly RegionComService _service;
    public RegionComController(RegionComService service) { _service = service; }
    public IActionResult Index(int? year, string? type) { /* impl */ }
}
```

**Services:** Use `using` statements for connections. `GetConnection()` factory. `MapFromReader` for entity mapping. Query with `@parameters`.

### Database Operations

**WPF:** Check `connection.State == ConnectionState.Closed` before `Open()`. Use `MySqlDataReader` for SELECT, `ExecuteNonQuery` for INSERT/UPDATE/DELETE. Handle `DBNull`.

```csharp
if (connection.State == ConnectionState.Closed) connection.Open();
Date = reader["date"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["date"]);
cmd.Parameters.AddWithValue("@date", objDocument.Date.Value.Date);
```

**WebAPI:** Use `using var connection = GetConnection()`. Parameterized queries.

### Error Handling
Wrap I/O, database, external ops in `try-catch`. Show `MessageBox.Show(ex.Message)` in WPF. Never silently catch.

```csharp
try { var result = documentServices.addDocument(data); if (result) MessageBox.Show("Success"); }
catch (Exception ex) { MessageBox.Show(ex.Message); }
```

### Regions and Organization
Use `#region` extensively. Patterns: `#region INotify`, `#region Notify Property Change`, `#region [Feature Name]`. Group related fields, commands, methods.

### Nullability and Type Safety
`<Nullable>enable</Nullable>`. Use `?` annotation. Check for null. Pattern matching: `if (parameter is Document doc && doc.Type != null)`.

### Collections and LINQ
`ObservableCollection<T>` for bindable WPF lists. `List<T>` for WebAPI. LINQ for filtering. `StringComparison.OrdinalIgnoreCase`.

```csharp
var filtered = DocumentList.Where(d => d.Date?.Year == 2024).ToList();
DocumentList = new ObservableCollection<Document>(filtered);
```

### File Operations
`OpenFileDialog` with filters: `Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"`.

---

## 3. Dependencies

**WPF Project:** `MySql.Data` (9.1.0), `SSH.NET` (2025.1.0)

**WebAPI:** `MySql.Data` (9.6.0), `Pomelo.EntityFrameworkCore.MySql` (9.0.0)

Add: `dotnet add package [PackageName]`

---

## 4. Project Structure

```
OSA File Management System/
├── Model/          # WPF data models + Service classes (INotifyPropertyChanged)
├── ViewModel/      # WPF ViewModels with commands
├── View/           # WPF XAML views (minimal code-behind)
├── Commands/       # RelayCommand implementation
└── images/         # WPF images/icons

OSAWebAPI/
├── Controllers/    # API controllers
├── Models/         # API models
└── Services/       # Business logic services
```

**Root Namespaces:** `OSA_File_Management_System` (WPF), `OSAWebAPI` (WebAPI)

---

## 5. Cursor/Copilot Rules

No `.cursor/rules/`, `.cursorrules`, or `.github/copilot-instructions.md`. Follow guidelines above.

---
