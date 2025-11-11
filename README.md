
# ResourceForkReader

ResourceForkReader is a .NET library for reading and parsing classic Mac resource fork files. It provides a simple API to extract resources, inspect headers, and work with resource data in modern .NET applications.

---

## Features

- Read and parse resource fork files (e.g., from classic Mac applications)
- Access resource types, IDs, names, and data
- Support for common resource record types
- Utility methods for working with resource data
- Low-level access to resource fork structure (headers, maps, attributes)

---

## Installation

Install via NuGet:

```sh
dotnet add package ResourceForkReader
```

Or via the NuGet Package Manager:

```sh
Install-Package ResourceForkReader
```

---

## Quick Start Example

```csharp
using ResourceForkReader;
using System.IO;

// Open a resource fork file
using var stream = File.OpenRead("MyResourceFile.res");

// Parse the resource fork
var resourceFork = new ResourceFork(stream);

// List all resource types and entries
foreach (var (type, entries) in resourceFork.Map.Types)
{
    Console.WriteLine($"Type: {type}");
    foreach (var entry in entries)
    {
        Console.WriteLine($"  ID: {entry.ID}, Attributes: {entry.Attributes}");
        // Read resource data
        byte[] data = resourceFork.GetResourceData(entry);
        // ... process data ...
    }
}
```

## License

MIT License.
