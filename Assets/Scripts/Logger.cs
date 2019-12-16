using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger {
    public static void Log(object message) {
        Debug.Log(message);
    }
    public static void LogError(object message) {
        Debug.LogError(message);
    }
}
