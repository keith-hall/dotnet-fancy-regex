# DotNetFancyRegex

A .NET wrapper for the Rust [fancy-regex](https://github.com/fancy-regex/fancy-regex) crate, providing advanced regular expression features including backreferences, lookahead, and lookbehind that are not supported in .NET's built-in `Regex` class.

## Features

- ✨ Advanced regex features via Rust's fancy-regex crate:
  - Backreferences (`\1`, `\2`, etc.)
  - Positive and negative lookahead (`(?=...)`, `(?!...)`)
  - Positive and negative lookbehind (`(?<=...)`, `(?<!...)`)
- 🚀 Native performance through Rust FFI
- 🔄 Simple, familiar API similar to .NET's `Regex`
- 📦 Ready to publish as a NuGet package
- 🔍 Benchmarked against .NET's built-in `Regex` class

## Installation

### From Source

1. Clone the repository:
```bash
git clone https://github.com/keith-hall/dotnet-fancy-regex.git
cd dotnet-fancy-regex
```

2. Build the project:
```bash
./build.sh
```

### From NuGet (Future)

```bash
dotnet add package DotNetFancyRegex
```

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or later
- [Rust](https://www.rust-lang.org/tools/install) (for building from source)

## Usage

### Basic Examples

```csharp
using DotNetFancyRegex;

// Create a regex pattern
using var regex = new FancyRegex(@"\d+");

// Check if text matches
bool matches = regex.IsMatch("hello 123 world"); // true

// Find the first match
string? match = regex.Find("hello 123 world"); // "123"

// Replace all matches
string result = regex.ReplaceAll("hello 123 world 456", "XXX"); // "hello XXX world XXX"
```

### Advanced Features

#### Backreferences

```csharp
using var regex = new FancyRegex(@"(\w+)\s+\1");

// Match repeated words
regex.IsMatch("hello hello"); // true
regex.IsMatch("hello world"); // false
```

#### Lookahead

```csharp
using var regex = new FancyRegex(@"\d+(?=px)");

// Match digits followed by "px"
regex.Find("width: 100px"); // "100"
```

#### Lookbehind

```csharp
using var regex = new FancyRegex(@"(?<=\$)\d+");

// Match digits preceded by "$"
regex.Find("price: $100"); // "100"
```

## API Reference

### `FancyRegex` Class

#### Constructor
```csharp
public FancyRegex(string pattern)
```
Creates a new `FancyRegex` from the given pattern.

**Throws:**
- `ArgumentNullException` - if pattern is null
- `FancyRegexException` - if the pattern is invalid

#### Methods

```csharp
public bool IsMatch(string text)
```
Checks if the text matches the pattern.

```csharp
public string? Find(string text)
```
Finds the first match in the text. Returns `null` if no match found.

```csharp
public string ReplaceAll(string text, string replacement)
```
Replaces all matches in the text with the replacement string.

## Building

The project consists of two main components:

1. **Rust FFI Library** (`fancy-regex-ffi/`) - Wraps the fancy-regex crate for FFI
2. **.NET Library** (`src/DotNetFancyRegex/`) - Provides the managed wrapper

To build both:

```bash
./build.sh
```

Or manually:

```bash
# Build Rust library
cd fancy-regex-ffi
cargo build --release
cd ..

# Build .NET solution
dotnet build
```

## Testing

Run the test suite:

```bash
# Set library path and run tests
LD_LIBRARY_PATH=fancy-regex-ffi/target/release:$LD_LIBRARY_PATH dotnet test
```

## Benchmarks

Compare performance between `FancyRegex` and .NET's built-in `Regex`:

```bash
cd benchmarks/DotNetFancyRegex.Benchmarks
LD_LIBRARY_PATH=../../fancy-regex-ffi/target/release:$LD_LIBRARY_PATH dotnet run -c Release
```

## Project Structure

```
dotnet-fancy-regex/
├── fancy-regex-ffi/          # Rust FFI wrapper
│   ├── src/
│   │   └── lib.rs           # FFI implementation
│   └── Cargo.toml
├── src/
│   └── DotNetFancyRegex/    # .NET library
│       ├── FancyRegex.cs    # Main wrapper class
│       └── Native.cs        # P/Invoke declarations
├── tests/
│   └── DotNetFancyRegex.Tests/  # Unit tests
├── benchmarks/
│   └── DotNetFancyRegex.Benchmarks/  # Performance benchmarks
├── build.sh                 # Build script
└── DotNetFancyRegex.sln    # Solution file
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- [fancy-regex](https://github.com/fancy-regex/fancy-regex) - The underlying Rust regex engine

