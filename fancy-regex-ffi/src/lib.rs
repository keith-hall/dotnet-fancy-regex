use fancy_regex::Regex;
use std::ffi::{CStr, CString};
use std::os::raw::c_char;
use std::ptr;

/// Opaque pointer to a Regex object
pub struct RegexHandle {
    regex: Regex,
}

/// Create a new regex from a pattern string
/// Returns a pointer to the regex handle, or null on error
#[no_mangle]
pub extern "C" fn fancy_regex_new(pattern: *const c_char) -> *mut RegexHandle {
    if pattern.is_null() {
        return ptr::null_mut();
    }

    let pattern_str = unsafe {
        match CStr::from_ptr(pattern).to_str() {
            Ok(s) => s,
            Err(_) => return ptr::null_mut(),
        }
    };

    match Regex::new(pattern_str) {
        Ok(regex) => Box::into_raw(Box::new(RegexHandle { regex })),
        Err(_) => ptr::null_mut(),
    }
}

/// Free a regex handle
#[no_mangle]
pub extern "C" fn fancy_regex_free(handle: *mut RegexHandle) {
    if !handle.is_null() {
        unsafe {
            let _ = Box::from_raw(handle);
        }
    }
}

/// Check if text matches the regex pattern
/// Returns 1 if matches, 0 if not, -1 on error
#[no_mangle]
pub extern "C" fn fancy_regex_is_match(handle: *const RegexHandle, text: *const c_char) -> i32 {
    if handle.is_null() || text.is_null() {
        return -1;
    }

    let regex = unsafe { &(*handle).regex };
    let text_str = unsafe {
        match CStr::from_ptr(text).to_str() {
            Ok(s) => s,
            Err(_) => return -1,
        }
    };

    match regex.is_match(text_str) {
        Ok(true) => 1,
        Ok(false) => 0,
        Err(_) => -1,
    }
}

/// Find the first match in text
/// Returns a pointer to the matched string (caller must free), or null if no match or error
#[no_mangle]
pub extern "C" fn fancy_regex_find(handle: *const RegexHandle, text: *const c_char) -> *mut c_char {
    if handle.is_null() || text.is_null() {
        return ptr::null_mut();
    }

    let regex = unsafe { &(*handle).regex };
    let text_str = unsafe {
        match CStr::from_ptr(text).to_str() {
            Ok(s) => s,
            Err(_) => return ptr::null_mut(),
        }
    };

    match regex.find(text_str) {
        Ok(Some(m)) => {
            match CString::new(m.as_str()) {
                Ok(s) => s.into_raw(),
                Err(_) => ptr::null_mut(),
            }
        }
        _ => ptr::null_mut(),
    }
}

/// Free a string returned by fancy_regex_find
#[no_mangle]
pub extern "C" fn fancy_regex_free_string(s: *mut c_char) {
    if !s.is_null() {
        unsafe {
            let _ = CString::from_raw(s);
        }
    }
}

/// Replace all matches in text with replacement
/// Returns a pointer to the new string (caller must free), or null on error
#[no_mangle]
pub extern "C" fn fancy_regex_replace_all(
    handle: *const RegexHandle,
    text: *const c_char,
    replacement: *const c_char,
) -> *mut c_char {
    if handle.is_null() || text.is_null() || replacement.is_null() {
        return ptr::null_mut();
    }

    let regex = unsafe { &(*handle).regex };
    let text_str = unsafe {
        match CStr::from_ptr(text).to_str() {
            Ok(s) => s,
            Err(_) => return ptr::null_mut(),
        }
    };
    let replacement_str = unsafe {
        match CStr::from_ptr(replacement).to_str() {
            Ok(s) => s,
            Err(_) => return ptr::null_mut(),
        }
    };

    let result = regex.replace_all(text_str, replacement_str);
    match CString::new(result.as_ref()) {
        Ok(s) => s.into_raw(),
        Err(_) => ptr::null_mut(),
    }
}

/// Get the last error message (if any)
/// Returns a pointer to the error string (caller must free), or null if no error
#[no_mangle]
pub extern "C" fn fancy_regex_get_error(pattern: *const c_char) -> *mut c_char {
    if pattern.is_null() {
        return ptr::null_mut();
    }

    let pattern_str = unsafe {
        match CStr::from_ptr(pattern).to_str() {
            Ok(s) => s,
            Err(_) => return ptr::null_mut(),
        }
    };

    match Regex::new(pattern_str) {
        Ok(_) => ptr::null_mut(),
        Err(e) => match CString::new(format!("{}", e)) {
            Ok(s) => s.into_raw(),
            Err(_) => ptr::null_mut(),
        },
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    use std::ffi::CString;

    #[test]
    fn test_basic_matching() {
        let pattern = CString::new(r"\d+").unwrap();
        let handle = fancy_regex_new(pattern.as_ptr());
        assert!(!handle.is_null());

        let text = CString::new("hello 123 world").unwrap();
        let result = fancy_regex_is_match(handle, text.as_ptr());
        assert_eq!(result, 1);

        fancy_regex_free(handle);
    }

    #[test]
    fn test_find() {
        let pattern = CString::new(r"\d+").unwrap();
        let handle = fancy_regex_new(pattern.as_ptr());
        assert!(!handle.is_null());

        let text = CString::new("hello 123 world").unwrap();
        let found = fancy_regex_find(handle, text.as_ptr());
        assert!(!found.is_null());

        let found_str = unsafe { CStr::from_ptr(found).to_str().unwrap() };
        assert_eq!(found_str, "123");

        fancy_regex_free_string(found);
        fancy_regex_free(handle);
    }
}
