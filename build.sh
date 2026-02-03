#!/bin/bash
set -e

echo "Building Rust FFI library..."
cd fancy-regex-ffi
cargo build --release
cd ..

echo "Copying native library to .NET output directories..."

# Determine the library extension based on OS
LIB_EXT=""
LIB_NAME=""
if [ "$(uname)" == "Darwin" ]; then
    LIB_EXT="dylib"
    LIB_NAME="libfancy_regex_ffi.dylib"
elif [ "$(expr substr $(uname -s) 1 5)" == "Linux" ]; then
    LIB_EXT="so"
    LIB_NAME="libfancy_regex_ffi.so"
else
    LIB_EXT="dll"
    LIB_NAME="fancy_regex_ffi.dll"
fi

# Function to copy library to output directory
copy_lib() {
    local dest_dir=$1
    mkdir -p "$dest_dir"
    if [ -f "fancy-regex-ffi/target/release/$LIB_NAME" ]; then
        cp "fancy-regex-ffi/target/release/$LIB_NAME" "$dest_dir/"
        echo "  Copied to $dest_dir"
    fi
}

# Copy to all necessary directories
copy_lib "src/DotNetFancyRegex/bin/Debug/net10.0"
copy_lib "src/DotNetFancyRegex/bin/Release/net10.0"
copy_lib "tests/DotNetFancyRegex.Tests/bin/Debug/net10.0"
copy_lib "tests/DotNetFancyRegex.Tests/bin/Release/net10.0"
copy_lib "benchmarks/DotNetFancyRegex.Benchmarks/bin/Debug/net10.0"
copy_lib "benchmarks/DotNetFancyRegex.Benchmarks/bin/Release/net10.0"

echo "Building .NET solution..."
dotnet build

echo "Copying native library after build..."
# Copy again after build in case directories were recreated
copy_lib "tests/DotNetFancyRegex.Tests/bin/Debug/net10.0"
copy_lib "benchmarks/DotNetFancyRegex.Benchmarks/bin/Debug/net10.0"

echo "Build complete!"

