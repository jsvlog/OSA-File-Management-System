# Agent Coding Guidelines for OSA File Management System

This document outlines the conventions and commands for agentic coding agents operating within the `OSA File Management System` repository. Adhering to these guidelines ensures consistency, maintainability, and compatibility with the existing codebase.

---

## 1. Build, Lint, and Test Commands

### 1.1. Build Command

To build the entire project, use the standard .NET CLI command:

```bash
dotnet build "OSA File Management System.csproj"
```

This command compiles the project and its dependencies.

### 1.2. Linting and Code Style Checks

This project leverages default C# coding conventions enforced by the .NET SDK and Visual Studio. While no explicit linting configuration (e.g., `.editorconfig` or dedicated linting tools) was found, agents should strive to adhere to the established code style observed in existing files.

Basic code analysis warnings and errors will be reported during the `dotnet build` process.

### 1.3. Test Commands

**No dedicated unit testing framework or test projects were identified in the current codebase structure.**

If unit tests are to be introduced, the recommended approach would be to:
1. Create a new xUnit or NUnit test project within the solution.
2. Add a reference from the test project to the project under test.
3. Write test classes and methods following a clear naming convention (e.g., `[ComponentName]Tests.cs`).

Once a test project is set up, the general command to run all tests would be:

```bash
dotnet test
```

To run a specific test, you would typically use a command similar to:

```bash
dotnet test --filter "FullyQualifiedName~[YourNamespace.YourTestClass.YourTestMethod]"
```

**Agents are advised to propose the creation of a test project and initial tests if tasked with implementing new features or fixing bugs, to establish a robust testing culture within the repository.**

---

## 2. Code Style Guidelines

The following guidelines are derived from an analysis of the existing C# codebase. Agents should mimic these patterns to ensure seamless integration of new or modified code.

### 2.1. Imports (Using Directives)

- Place `using` directives at the top of the file.
- Group `System` namespaces first, followed by third-party libraries (if any), and then project-specific namespaces.
- Sort `using` directives alphabetically within each group.

Example:
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32; // Example for a third-party or specific framework reference

using OSA_File_Management_System.Commands;
using OSA_File_Management_System.Model;
using OSA_File_Management_System.View;
using OSA_File_Management_System.ViewModel;
```

### 2.2. Formatting

- **Indentation:** Use 4 spaces for indentation, not tabs.
- **Braces:** Use K&R style for braces (opening brace on the same line as the declaration).
- **Blank Lines:** Use blank lines to separate logical sections of code (e.g., between methods, properties, or regions).
- **Line Length:** Aim for a reasonable line length (e.g., 120 characters) for readability, breaking longer lines where appropriate.

### 2.3. Types and Naming Conventions

- **Namespaces:** `PascalCase` (e.g., `OSA_File_Management_System`, `OSA_File_Management_System.Model`).
- **Classes, Interfaces, Enums:** `PascalCase` (e.g., `Document`, `INotifyPropertyChanged`, `RelayCommand`).
- **Methods:** `PascalCase` (e.g., `LoadData`, `OnPropertyChanged`, `OpenAddDocumentForm`).
- **Public Properties:** `PascalCase` (e.g., `Id`, `DocumentList`, `AddFormData`).
- **Private Fields:** `camelCase` (e.g., `id`, `documentList`, `addFormData`).
- **Local Variables:** `camelCase` (e.g., `selectedItem`, `navigationTag`).
- **Constants:** `PascalCase` or `UPPER_SNAKE_CASE` for global constants. Local constants can be `camelCase`. Prefer `PascalCase` if not used as `const string` fields.

### 2.4. Error Handling

- Use `try-catch` blocks for operations that might throw exceptions, especially when interacting with I/O, databases, or external systems.
- Provide meaningful error messages, often using `MessageBox.Show(ex.Message)` for UI feedback.
- Avoid swallowing exceptions; log them or rethrow if appropriate for the context.

Example:
```csharp
try
{
    var isSaved = documentServices.addDocument(AddFormData);
    if (isSaved)
    {
        MessageBox.Show("Saving Successful");
        // ...
    }
    else
    {
        MessageBox.Show("Error saving");
    }
}
catch (Exception ex)
{
    MessageBox.Show(ex.Message);
}
```

### 2.5. Comments and Documentation

- **Inline Comments:** Use sparingly for complex logic or non-obvious code sections. Prefer `//` for single-line comments.
- **Regions:** Utilize `#region` and `#endregion` to logically group related code (e.g., properties, methods for a specific feature, `INotifyPropertyChanged` implementation). Existing regions like `#region INotify`, `#region Add Document` should be maintained.
- **XML Documentation Comments:** Use `///` for public classes, interfaces, methods, and properties to provide summary documentation. This is not extensively used in the observed files but should be adopted for new public API surfaces.

### 2.6. MVVM Pattern Adherence

- This project follows the Model-View-ViewModel (MVVM) pattern for WPF.
- **Models:** Represent data and business logic (e.g., `Document.cs`). Implement `INotifyPropertyChanged` for data binding.
- **Views:** XAML files and their code-behind (e.g., `MainWindow.xaml`, `MainWindow.xaml.cs`). Should contain minimal logic, primarily for UI events that defer to the ViewModel.
- **ViewModels:** Expose data and commands to the View (e.g., `DocumentViewModel.cs`). Implement `INotifyPropertyChanged` and use `RelayCommand` for commanding.
- **Commands:** Use `RelayCommand` (or similar `ICommand` implementation) for UI actions.

### 2.7. Nullability

- The project uses `Nullable>enable</Nullable>` in the `.csproj`, indicating nullable reference types are enabled.
- Use nullable annotations (`?`) appropriately for reference types that can be null.

---

## 3. Cursor/Copilot Rules

No `.cursor/rules/`, `.cursorrules`, or `.github/copilot-instructions.md` files were found in the repository. Therefore, there are no specific Cursor or Copilot rules to include at this time. Agents should rely solely on the C# coding guidelines provided above.

---
