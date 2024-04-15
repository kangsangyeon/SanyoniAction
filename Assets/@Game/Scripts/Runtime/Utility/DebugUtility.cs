using System.Runtime.CompilerServices;
using UnityEngine;

public static class DebugUtility
{
    public static void Log(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string caller = null)
    {
        Debug.Log($"{caller}(at line {lineNumber}): {message}");
    }

    public static void LogWarning(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string caller = null)
    {
        Debug.LogWarning($"{caller}(at line {lineNumber}): {message}");
    }

    public static void LogError(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string caller = null)
    {
        Debug.LogError($"{caller}(at line {lineNumber}): {message}");
    }
}