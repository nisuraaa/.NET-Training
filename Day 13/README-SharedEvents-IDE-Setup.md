# SharedEvents Project - IDE Setup Guide

This guide explains how to properly load and work with the SharedEvents project in different IDEs.

## Problem Solved

The SharedEvents project was missing from the main solution file (`Microservices.sln`), which caused issues when loading the project in various IDEs. This has been fixed by adding the SharedEvents project to the solution.

## IDE-Specific Instructions

### 1. Visual Studio

#### ✅ **Recommended Approach**
1. **Open the Solution File**: Always open `Microservices.sln` (not individual project files)
2. **Location**: `/Users/nisura/Desktop/NET-Training/Day 13/Microservices.sln`
3. **Restore Packages**: Right-click on solution → "Restore NuGet Packages"
4. **Build Solution**: Build → Rebuild Solution

#### What You'll See
- **Solution Explorer** will show:
  ```
  Microservices
  └── Services
      ├── DepartmentServices
      ├── EmployeeService  
      ├── ProjectService
      └── SharedEvents ← Now visible!
  ```

#### Troubleshooting
- If SharedEvents doesn't appear, close Visual Studio and reopen the solution
- If you get build errors, try: Build → Clean Solution → Rebuild Solution

### 2. JetBrains Rider

#### ✅ **Recommended Approach**
1. **Open Solution**: File → Open → Select `Microservices.sln`
2. **Wait for Indexing**: Let Rider finish indexing the solution
3. **Restore Packages**: Right-click on solution → "Restore NuGet Packages"
4. **Build**: Build → Rebuild All

#### What You'll See
- **Solution Explorer** will show the SharedEvents project under Services
- **Project References** will be properly resolved
- **IntelliSense** will work for SharedEvents types

#### Troubleshooting
- If references aren't resolved: Tools → NuGet → Restore All
- If IntelliSense doesn't work: File → Invalidate Caches and Restart

### 3. Visual Studio Code

#### ✅ **Recommended Approach**
1. **Open Workspace**: File → Open Folder → Select the root directory
2. **Install Extensions**:
   - C# for Visual Studio Code (ms-dotnettools.csharp)
   - .NET Core Tools (ms-dotnettools.vscode-dotnet-runtime)
3. **Open Terminal**: Terminal → New Terminal
4. **Restore and Build**:
   ```bash
   dotnet restore
   dotnet build
   ```

#### What You'll See
- **Explorer** will show all projects including SharedEvents
- **IntelliSense** will work for SharedEvents types
- **Go to Definition** will work across projects

#### Troubleshooting
- If IntelliSense doesn't work: Ctrl+Shift+P → "Omnisharp: Restart OmniSharp"
- If build fails: Check the Output panel for detailed error messages

### 4. Visual Studio for Mac

#### ✅ **Recommended Approach**
1. **Open Solution**: File → Open → Select `Microservices.sln`
2. **Restore Packages**: Right-click on solution → "Restore NuGet Packages"
3. **Build**: Build → Rebuild All

## Project Structure

The SharedEvents project contains:

```
Services/SharedEvents/
├── Events/
│   ├── DepartmentEvents.cs    # Department-related events
│   ├── EmployeeEvents.cs      # Employee-related events
│   └── ProjectEvents.cs       # Project-related events
├── SharedEvents.csproj        # Project file
└── bin/                       # Build output
```

## Event Types Available

### Department Events
- `DepartmentCreatedEvent`
- `DepartmentUpdatedEvent`
- `DepartmentDeletedEvent`

### Employee Events
- `EmployeeCreatedEvent`
- `EmployeeUpdatedEvent`
- `EmployeeDeletedEvent`

### Project Events
- `ProjectCreatedEvent`
- `ProjectUpdatedEvent`
- `ProjectDeletedEvent`

## Usage in Other Projects

The SharedEvents project is referenced by all microservices:

```xml
<ItemGroup>
  <ProjectReference Include="..\SharedEvents\SharedEvents.csproj" />
</ItemGroup>
```

### Example Usage
```csharp
using SharedEvents.Events;

// Publish an event
var departmentCreated = new DepartmentCreatedEvent
{
    DepartmentId = "dept-123",
    Name = "Engineering",
    CreatedAt = DateTime.UtcNow
};

// Consume an event
public class DepartmentEventConsumer : IConsumer<DepartmentCreatedEvent>
{
    public async Task Consume(ConsumeContext<DepartmentCreatedEvent> context)
    {
        var @event = context.Message;
        // Handle the event
    }
}
```

## Common Issues and Solutions

### Issue 1: "SharedEvents not found" or "Cannot resolve reference"
**Solution**: 
1. Make sure you opened the solution file, not individual projects
2. Restore NuGet packages
3. Rebuild the solution

### Issue 2: IntelliSense not working for SharedEvents types
**Solution**:
1. **Visual Studio**: Build → Rebuild Solution
2. **Rider**: File → Invalidate Caches and Restart
3. **VS Code**: Ctrl+Shift+P → "Omnisharp: Restart OmniSharp"

### Issue 3: Build errors related to SharedEvents
**Solution**:
1. Check that the project reference path is correct
2. Ensure all projects target the same .NET version (net9.0)
3. Clean and rebuild the solution

### Issue 4: "Project not loaded" in Visual Studio
**Solution**:
1. Right-click on the solution → "Reload Project"
2. If that doesn't work, close Visual Studio and reopen the solution

## Verification Steps

To verify everything is working correctly:

1. **Open the solution** in your preferred IDE
2. **Check Solution Explorer** - SharedEvents should be visible
3. **Open a microservice project** (e.g., DepartmentServices)
4. **Look for using statements** like `using SharedEvents.Events;`
5. **Check IntelliSense** - typing `DepartmentCreated` should show autocomplete
6. **Build the solution** - should complete without errors

## Development Workflow

1. **Make changes to events** in SharedEvents project
2. **Build the solution** to ensure changes are compiled
3. **Other projects** will automatically pick up the changes
4. **No need to restart** the IDE (unless you add new files)

## Best Practices

1. **Always open the solution file** rather than individual projects
2. **Keep all projects in sync** with the same .NET version
3. **Use meaningful event names** that clearly describe what happened
4. **Include all necessary data** in events for consumers to process
5. **Version your events** if you need to make breaking changes

## Troubleshooting Commands

If you're still having issues, try these commands in the terminal:

```bash
# Navigate to the solution directory
cd "/Users/nisura/Desktop/NET-Training/Day 13"

# Clean all projects
dotnet clean

# Restore all packages
dotnet restore

# Build all projects
dotnet build

# Build specific project
dotnet build Services/SharedEvents/SharedEvents.csproj
```

This should resolve any issues with loading SharedEvents in your IDE!
