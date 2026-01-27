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
No explicit linting configuration. Uses default .NET SDK conventions. Follow existing patterns and heed compiler warnings.

### Test Commands
No test project currently exists. Recommended approach:
```bash
dotnet new xunit -n OSAFileManagementTests
dotnet add reference ../OSA\ File\ Management\ System/OSA\ File\ Management\ System.csproj
```
Write tests as `[ComponentName]Tests.cs`. Once tests exist:
```bash
dotnet test                                                     # Run all tests
dotnet test --filter "FullyQualifiedName~Namespace.Class.Method"  # Run specific test
dotnet test --filter "FullyQualifiedName~DocumentViewModel"     # Run all tests in class
```

---

## 2. Code Style Guidelines

### Imports (Using Directives)
Place at top of file. Group: System → third-party → project. Blank line between groups. Sort alphabetically within groups.

### Formatting
- **Indentation:** 4 spaces, not tabs
- **Braces:** K&R style (opening brace on same line)
- **Blank lines:** Separate methods, properties, regions
- **Line length:** ~120 characters max
- **Spacing:** Spaces after commas, around operators: `if (x == 0)`

### Naming Conventions
- **Namespaces:** `PascalCase` with underscores: `OSA_File_Management_System`, `OSA_File_Management_System.Model`
- **Classes/Interfaces/Enums:** `PascalCase`: `Document`, `INotifyPropertyChanged`, `RelayCommand`
- **Methods:** `PascalCase`: `LoadData`, `OnPropertyChanged`
- **Properties:** `PascalCase`: `Id`, `DocumentList`
- **Private Fields:** `camelCase`: `id`, `documentList`, `addFormData`
- **Commands:** `PascalCase` with "Command" suffix: `ShowAddFormCommand`, `DeleteDocumentCommand`

### MVVM Pattern

**Models:** Implement `INotifyPropertyChanged` with `OnPropertyChanged(string propertyName)`. All properties raise changed events.

```csharp
private int id;
public int Id { get { return id; } set { id = value; OnPropertyChanged("Id"); } }
```

**ViewModels:** Expose `RelayCommand` properties. Initialize in constructor. Use `#region` blocks for features (`#region Add Document`, `#region Search`).

**Views:** Minimal code-behind (only InitializeComponent). Set `DataContext` to ViewModel. Use XAML binding.

**Commands:** Use `RelayCommand` class with parameter/non-parameter support.

```csharp
private RelayCommand addDocument;
public RelayCommand AddDocument => addDocument ??= new RelayCommand(AddDocumentMethod);
private void AddDocumentMethod() { /* impl */ }
private void EditMethod(object parameter) { /* impl with param */ }
```

### Error Handling
Wrap I/O, database, external ops in `try-catch`. Show messages via `MessageBox.Show(ex.Message)`. Never silently catch.

```csharp
try { var result = documentServices.addDocument(data); if (result) MessageBox.Show("Success"); }
catch (Exception ex) { MessageBox.Show(ex.Message); }
```

### Regions and Organization
Use `#region` extensively. Patterns: `#region INotify`/`#region Notify Property Change`, `#region [Feature Name]`. Group related fields, commands, methods.

### Nullability and Type Safety
Project has `<Nullable>enable</Nullable>`. Use `?` annotation. Check for null. Use pattern matching and null-coalescing.

```csharp
if (parameter is Document doc && doc.Type != null) { /* safe access */ }
string display = doc.Type ?? "N/A";
```

### Collections and LINQ
Use `ObservableCollection<T>` for bindable lists. Prefer LINQ. Use `StringComparison.OrdinalIgnoreCase` for case-insensitive comparisons.

```csharp
var filtered = DocumentList.Where(d => d.Date?.Year == 2024).ToList();
DocumentList = new ObservableCollection<Document>(filtered);
```

### Database Operations
Check `connection.State == ConnectionState.Closed` before opening. Use `MySqlDataReader` for queries, `ExecuteNonQuery` for INSERT/UPDATE/DELETE. Handle DBNull values.

```csharp
if (connection.State == ConnectionState.Closed) connection.Open();
Date = reader["date"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["date"]);
```

### File Operations
Use `OpenFileDialog` with filters: `Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"`

---

## 3. Dependencies

- **MySql.Data** (9.1.0): MySQL database connectivity
- **SSH.NET** (2025.1.0): SSH functionality

Add packages: `dotnet add package [PackageName]`

---

## 4. Project Structure

```
OSA File Management System/
├── Model/      # Data models + Service classes (INotifyPropertyChanged)
├── ViewModel/  # ViewModels with commands and data
├── View/       # XAML views (minimal code-behind)
├── Commands/   # RelayCommand implementation
└── images/     # Application images/icons
```

Root namespace: `OSA_File_Management_System`

---

## 5. Cursor/Copilot Rules

No `.cursor/rules/`, `.cursorrules`, or `.github/copilot-instructions.md` files. Follow guidelines above.

---