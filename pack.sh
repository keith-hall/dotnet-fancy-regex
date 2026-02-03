#!/bin/bash
set -e

echo "Building NuGet package for DotNetFancyRegex..."

# Build Rust library in release mode
echo "Building Rust FFI library (release mode)..."
cd fancy-regex-ffi
cargo build --release
cd ..

# Build .NET library in release mode
echo "Building .NET library (release mode)..."
dotnet build -c Release src/DotNetFancyRegex/DotNetFancyRegex.csproj

# The NuGet package should already be created by the build process
NUPKG_PATH="src/DotNetFancyRegex/bin/Release/DotNetFancyRegex.0.1.0.nupkg"

if [ -f "$NUPKG_PATH" ]; then
    echo ""
    echo "✓ NuGet package created successfully:"
    echo "  $NUPKG_PATH"
    echo ""
    echo "To publish to NuGet.org, run:"
    echo "  dotnet nuget push $NUPKG_PATH --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json"
else
    echo "Error: NuGet package not found at $NUPKG_PATH"
    exit 1
fi
