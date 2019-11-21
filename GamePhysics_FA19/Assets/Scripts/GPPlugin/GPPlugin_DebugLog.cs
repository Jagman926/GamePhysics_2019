using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

public class GPPlugin_DebugLog : MonoBehaviour
{
    // Enum for adjusting color of Debug String from Plugin
    enum Color { red, green, black, blue, white, yellow, orange }

    // Pointer for debug log delegate
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DebugDelegate(string str);

    // Function delegate for calling debug information as string
    static void CallBackFunction(string str) { Debug.Log(str); }
    public delegate void DebugCallback(IntPtr request, int color, int size);

    // Import for registering debug
    [DllImport("Physics_UnityPlugin")]
    public static extern void RegisterDebugCallback(DebugCallback cb);

    // Debug string creation from plugin using format to apply color from Plugin
    [MonoPInvokeCallback(typeof(DebugCallback))]
    static void OnDebugCallback(IntPtr request, int color, int size)
    {
        string debug_string = Marshal.PtrToStringAnsi(request, size);
        debug_string = String.Format(
            "{0}{1}{2}{3}{4}",
            "<color=",
            ((Color)color).ToString(),
            ">",
            debug_string,
            "</color>"
            );

        Debug.Log(debug_string);
    }

    // Registers Debug event with Plugin
    private void OnEnable()
    {
        RegisterDebugCallback(OnDebugCallback);
    }
}
