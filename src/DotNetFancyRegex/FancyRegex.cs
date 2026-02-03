using System.Runtime.InteropServices;

namespace DotNetFancyRegex;

/// <summary>
/// A regular expression using the fancy-regex Rust crate.
/// Supports advanced features like backreferences, lookahead, and lookbehind.
/// </summary>
public class FancyRegex : IDisposable
{
    private IntPtr _handle;
    private bool _disposed;

    /// <summary>
    /// Gets the pattern string used to create this regex.
    /// </summary>
    public string Pattern { get; }

    /// <summary>
    /// Creates a new FancyRegex from the given pattern.
    /// </summary>
    /// <param name="pattern">The regular expression pattern</param>
    /// <exception cref="ArgumentNullException">Thrown when pattern is null</exception>
    /// <exception cref="FancyRegexException">Thrown when the pattern is invalid</exception>
    public FancyRegex(string pattern)
    {
        if (pattern == null)
            throw new ArgumentNullException(nameof(pattern));

        Pattern = pattern;
        _handle = Native.fancy_regex_new(pattern);

        if (_handle == IntPtr.Zero)
        {
            // Try to get the error message
            var errorPtr = Native.fancy_regex_get_error(pattern);
            string errorMessage = "Invalid regex pattern";
            
            if (errorPtr != IntPtr.Zero)
            {
                try
                {
                    errorMessage = Marshal.PtrToStringUTF8(errorPtr) ?? "Invalid regex pattern";
                }
                finally
                {
                    Native.fancy_regex_free_string(errorPtr);
                }
            }

            throw new FancyRegexException(errorMessage);
        }
    }

    /// <summary>
    /// Checks if the text matches the pattern.
    /// </summary>
    /// <param name="text">The text to match against</param>
    /// <returns>True if the text matches, false otherwise</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null</exception>
    /// <exception cref="FancyRegexException">Thrown when an error occurs during matching</exception>
    public bool IsMatch(string text)
    {
        ThrowIfDisposed();
        
        if (text == null)
            throw new ArgumentNullException(nameof(text));

        var result = Native.fancy_regex_is_match(_handle, text);
        
        if (result == -1)
            throw new FancyRegexException("Error occurred during matching");
        
        return result == 1;
    }

    /// <summary>
    /// Finds the first match in the text.
    /// </summary>
    /// <param name="text">The text to search</param>
    /// <returns>The matched string, or null if no match found</returns>
    /// <exception cref="ArgumentNullException">Thrown when text is null</exception>
    public string? Find(string text)
    {
        ThrowIfDisposed();
        
        if (text == null)
            throw new ArgumentNullException(nameof(text));

        var resultPtr = Native.fancy_regex_find(_handle, text);
        
        if (resultPtr == IntPtr.Zero)
            return null;

        try
        {
            return Marshal.PtrToStringUTF8(resultPtr);
        }
        finally
        {
            Native.fancy_regex_free_string(resultPtr);
        }
    }

    /// <summary>
    /// Replaces all matches in the text with the replacement string.
    /// </summary>
    /// <param name="text">The text to search</param>
    /// <param name="replacement">The replacement string</param>
    /// <returns>The text with all matches replaced</returns>
    /// <exception cref="ArgumentNullException">Thrown when text or replacement is null</exception>
    /// <exception cref="FancyRegexException">Thrown when an error occurs during replacement</exception>
    public string ReplaceAll(string text, string replacement)
    {
        ThrowIfDisposed();
        
        if (text == null)
            throw new ArgumentNullException(nameof(text));
        if (replacement == null)
            throw new ArgumentNullException(nameof(replacement));

        var resultPtr = Native.fancy_regex_replace_all(_handle, text, replacement);
        
        if (resultPtr == IntPtr.Zero)
            throw new FancyRegexException("Error occurred during replacement");

        try
        {
            return Marshal.PtrToStringUTF8(resultPtr) ?? text;
        }
        finally
        {
            Native.fancy_regex_free_string(resultPtr);
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(FancyRegex));
    }

    /// <summary>
    /// Releases the unmanaged resources used by the FancyRegex.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            if (_handle != IntPtr.Zero)
            {
                Native.fancy_regex_free(_handle);
                _handle = IntPtr.Zero;
            }
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Finalizer to ensure native resources are freed.
    /// </summary>
    ~FancyRegex()
    {
        Dispose();
    }
}

/// <summary>
/// Exception thrown when a FancyRegex operation fails.
/// </summary>
public class FancyRegexException : Exception
{
    public FancyRegexException(string message) : base(message) { }
    public FancyRegexException(string message, Exception innerException) : base(message, innerException) { }
}
