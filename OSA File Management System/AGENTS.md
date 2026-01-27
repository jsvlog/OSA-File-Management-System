# Agent Coding Guidelines for OSA File Management System

WPF .NET 8.0 MVVM application for file/document management with MySQL integration.

---

## 1. Build, Lint, and Test Commands

### Build Commands
```bash
dotnet build "OSA File Management System.sln"
dotnet build "OSA File Management System/OSA File Management System.csproj"
dotnet build "OSA File Management System.sln" -c Release
```

### Linting
No explicit linting configuration (`.editorconfig`, StyleCop). Project uses default .NET SDK conventions. Follow existing code patterns and heed compiler warnings from `dotnet build`.

### Test Commands
No test project currently exists. Recommended approach to add unit testing:
```bash
dotnet new xunit -n OSAFileManagementTests
dotnet add reference ../OSA\ File\ Management\ System/OSA\ File\ Management\ System.csproj
```
Write test classes following `[ComponentName]Tests.cs` pattern (e.g., `DocumentViewModelTests.cs`).

Once tests exist:
```bash
dotnet test                                                     # Run all tests
dotnet test --filter "FullyQualifiedName~Namespace.Class.Method"  # Run specific test
dotnet test --filter "FullyQualifiedName~DocumentViewModel"     # Run all tests in class
```

---

## 2. Code Style Guidelines

### Imports (Using Directives)
Place `using` directives at top of file. Group in order: System namespaces → third-party → project namespaces. Sort alphabetically within groups. Blank line between groups.

### Formatting
- **Indentation:** 4 spaces, not tabs
- **Braces:** K&R style (opening brace on same line)
- **Blank lines:** Separate logical sections between methods, properties, regions
- **Line length:** ~120 characters max, break longer lines appropriately
- **Spacing:** Spaces after commas, around operators: `if (x == 0)`, not `if(x==0)`

### Naming Conventions
- **Namespaces:** `PascalCase` with underscores: `OSA_File_Management_System`, `OSA_File_Management_System.Model`
- **Classes/Interfaces/Enums:** `PascalCase`: `Document`, `INotifyPropertyChanged`, `RelayCommand`
- **Methods:** `PascalCase`: `LoadData`, `OnPropertyChanged`, `OpenAddDocumentForm`
- **Public Properties:** `PascalCase`: `Id`, `DocumentList`, `AddFormData`
- **Private Fields:** `camelCase`: `id`, `documentList`, `addFormData`
- **Local Variables:** `camelCase`: `selectedItem`, `filterResult`
- **Commands:** `PascalCase` with "Command" suffix: `ShowAddFormCommand`, `DeleteDocumentCommand`

### MVVM Pattern Implementation

**Models:** Implement `INotifyPropertyChanged` interface with `OnPropertyChanged(string propertyName)` method. All properties must raise property changed events on setters.

```csharp
private int id;
public int Id
{
    get { return id; }
    set { id = value; OnPropertyChanged("Id"); }
}
```

**ViewModels:** Expose `RelayCommand` properties for all UI actions. Initialize commands in constructor. Use `#region` blocks to organize related functionality (e.g., `#region Add Document`, `#region Search`).

**Views:** Keep code-behind minimal (only InitializeComponent). Set `DataContext` to ViewModel instance. Use XAML binding for all data display and user input.

**Commands:** Use `RelayCommand` class which supports both parameterized and non-parameterized constructors.

```csharp
private RelayCommand addDocument;
public RelayCommand AddDocument => addDocument ??= new RelayCommand(AddDocumentMethod);

private void AddDocumentMethod() { /* implementation */ }
private void EditMethod(object parameter) { /* implementation with parameter */ }
```

### Error Handling
Wrap I/O, database, and external operations in `try-catch` blocks. Display user-friendly error messages via `MessageBox.Show(ex.Message)`. Never silently catch exceptions.

```csharp
try
{
    var result = documentServices.addDocument(data);
    if (result) MessageBox.Show("Success");
    else MessageBox.Show("Error saving");
}
catch (Exception ex)
{
    MessageBox.Show(ex.Message);
}
```

### Regions and Organization
Use `#region` directives extensively to organize code into logical blocks. Common patterns:
- `#region INotify` or `#region Notify Property Change` for PropertyChanged implementation
- `#region [Feature Name]` (e.g., `#region Add Document`, `#region Search`, `#region Print`)
- Group related private fields, commands, and methods within each region

### Nullability and Type Safety
Project has `<Nullable>enable</Nullable>` in .csproj. Use nullable annotation (`?`) for nullable reference types. Check for null before accessing. Use pattern matching and null-coalescing.

```csharp
if (parameter is Document doc && doc.Type != null) { /* safe access */ }
string display = doc.Type ?? "N/A";
```

### Collections and LINQ
Use `ObservableCollection<T>` for bindable lists in ViewModels. Prefer LINQ for filtering and querying. Use `StringComparison.OrdinalIgnoreCase` for case-insensitive string comparisons.

```csharp
var filtered = DocumentList.Where(d => d.Date?.Year == 2024).ToList();
DocumentList = new ObservableCollection<Document>(filtered);
```

### File Operations
Use `OpenFileDialog` with appropriate filters for file selection. Example: `Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"`

---

## 3. Dependencies

- **MySql.Data** (9.1.0): MySQL database connectivity
- **SSH.NET** (2025.1.0): SSH functionality for remote operations

Add new packages: `dotnet add package [PackageName]`

---

## 4. Project Structure

```
OSA File Management System/
├── Model/      # Data models with INotifyPropertyChanged
├── ViewModel/  # ViewModels exposing commands and data
├── View/       # XAML views and minimal code-behind
├── Commands/   # RelayCommand implementation
└── images/     # Application images and icons
```

All code resides in root namespace: `OSA_File_Management_System`

---

## 5. Cursor/Copilot Rules

No `.cursor/rules/`, `.cursorrules`, or `.github/copilot-instructions.md` files found. Follow guidelines above.

---