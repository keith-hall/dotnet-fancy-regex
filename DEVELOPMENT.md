# DotNetFancyRegex - Development Guide

## Quick Start

### Building the Project

```bash
# Build both Rust FFI and .NET components
./build.sh

# Run tests
LD_LIBRARY_PATH=fancy-regex-ffi/target/release:$LD_LIBRARY_PATH dotnet test

# Run example
cd examples/DotNetFancyRegex.Example
LD_LIBRARY_PATH=../../fancy-regex-ffi/target/release:$LD_LIBRARY_PATH dotnet run
```

### Creating NuGet Package

```bash
./pack.sh
```

This creates a NuGet package at `src/DotNetFancyRegex/bin/Release/DotNetFancyRegex.0.1.0.nupkg`

## Architecture

### Components

1. **fancy-regex-ffi** (Rust)
   - Wraps the fancy-regex crate with C-compatible FFI
   - Exposes functions for creating, using, and freeing regex objects
   - Built as a cdylib (shared library)

2. **DotNetFancyRegex** (C# Library)
   - P/Invoke layer to call Rust functions
   - Managed wrapper with .NET-friendly API
   - Implements IDisposable for proper resource cleanup

3. **DotNetFancyRegex.Tests** (xUnit Tests)
   - Unit tests for all functionality
   - Tests for advanced regex features

4. **DotNetFancyRegex.Benchmarks** (BenchmarkDotNet)
   - Performance comparison with .NET Regex

5. **DotNetFancyRegex.Example** (Console App)
   - Usage examples and demos

### Memory Management

The library uses a careful memory management strategy:

- **Rust Side**: 
  - Regex objects are allocated with `Box::into_raw()`
  - Freed with `Box::from_raw()` in `fancy_regex_free()`
  - Strings are allocated with `CString::into_raw()`
  - Freed with `CString::from_raw()` in `fancy_regex_free_string()`

- **.NET Side**:
  - `FancyRegex` implements `IDisposable`
  - Calls native free functions in `Dispose()`
  - Has a finalizer as a safety net

### Cross-Platform Support

The library supports multiple platforms:
- Linux (libfancy_regex_ffi.so)
- macOS (libfancy_regex_ffi.dylib)
- Windows (fancy_regex_ffi.dll)

The NuGet package includes native libraries for all platforms in the appropriate `runtimes/` directories.

## API Overview

### Creating a Regex

```csharp
using var regex = new FancyRegex(@"\d+");
```

### Matching

```csharp
bool matches = regex.IsMatch("hello 123");  // true
```

### Finding

```csharp
string? match = regex.Find("hello 123");  // "123"
```

### Replacing

```csharp
string result = regex.ReplaceAll("hello 123", "XXX");  // "hello XXX"
```

## Advanced Features

### Backreferences

```csharp
using var regex = new FancyRegex(@"(\w+)\s+\1");
regex.IsMatch("hello hello");  // true - repeated word
```

### Lookahead

```csharp
using var regex = new FancyRegex(@"\d+(?=px)");
regex.Find("100px");  // "100" - number followed by "px"
```

### Lookbehind

```csharp
using var regex = new FancyRegex(@"(?<=\$)\d+");
regex.Find("$100");  // "100" - number preceded by "$"
```

## Testing

### Run All Tests

```bash
LD_LIBRARY_PATH=fancy-regex-ffi/target/release:$LD_LIBRARY_PATH dotnet test
```

### Test Coverage

- Basic pattern matching
- Find operations
- Replace operations
- Error handling
- Backreferences
- Lookahead assertions
- Lookbehind assertions

## Performance

In quick tests, FancyRegex shows competitive performance with .NET's built-in Regex:

```
Pattern: \d+
Text: 'hello 123 world 456 test 789'
Iterations: 10,000
.NET Regex: 1.34ms
FancyRegex: 0.80ms
Ratio: 0.60x (FancyRegex is faster!)
```

Note: Performance varies by pattern complexity and operation type.

## Publishing to NuGet

1. Build the package:
   ```bash
   ./pack.sh
   ```

2. Publish to NuGet.org:
   ```bash
   dotnet nuget push src/DotNetFancyRegex/bin/Release/DotNetFancyRegex.0.1.0.nupkg \
     --api-key YOUR_API_KEY \
     --source https://api.nuget.org/v3/index.json
   ```

## Troubleshooting

### Library Not Found Error

If you get `DllNotFoundException`, ensure:
1. The native library is built: `cargo build --release -p fancy-regex-ffi`
2. The library is in the output directory or in `LD_LIBRARY_PATH`
3. For tests/examples, copy the library to the bin directory

### Build Failures

- Ensure Rust toolchain is installed: `rustc --version`
- Ensure .NET SDK 10.0 or later: `dotnet --version`
- Clean and rebuild: `cargo clean && dotnet clean && ./build.sh`

## Future Enhancements

Potential improvements:
- Support for more regex operations (captures, splits, etc.)
- Async/cancellable operations
- Regex compilation caching
- More comprehensive benchmarks
- CI/CD integration
- Multi-platform native library builds

## License

MIT License - See LICENSE file for details.
