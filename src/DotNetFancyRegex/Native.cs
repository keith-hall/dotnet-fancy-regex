using System.Runtime.InteropServices;

namespace DotNetFancyRegex;

internal static class Native
{
    private const string LibraryName = "fancy_regex_ffi";

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr fancy_regex_new([MarshalAs(UnmanagedType.LPUTF8Str)] string pattern);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void fancy_regex_free(IntPtr handle);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int fancy_regex_is_match(IntPtr handle, [MarshalAs(UnmanagedType.LPUTF8Str)] string text);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr fancy_regex_find(IntPtr handle, [MarshalAs(UnmanagedType.LPUTF8Str)] string text);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void fancy_regex_free_string(IntPtr str);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr fancy_regex_replace_all(
        IntPtr handle,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string text,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string replacement);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr fancy_regex_get_error([MarshalAs(UnmanagedType.LPUTF8Str)] string pattern);
}
