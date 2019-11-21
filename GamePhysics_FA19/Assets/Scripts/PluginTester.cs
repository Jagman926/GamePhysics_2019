using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

public class PluginTester : MonoBehaviour
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DebugDelegate(string str);

    static void CallBackFunction(string str) { Debug.Log(str); }
    public delegate void DebugCallback(IntPtr request, int color, int size);

    [DllImport("Physics_UnityPlugin")]
    public static extern void RegisterDebugCallback(DebugCallback cb);


    enum Color
    {
        red,
        green,
        black,
        blue,
        white,
        yellow,
        orange
    }

    [MonoPInvokeCallback(typeof(DebugCallback))]
    static void OnDebugCallback(IntPtr request, int color, int size)
    {
        string debug_string = Marshal.PtrToStringAnsi(request, size);
        debug_string = String.Format("{0}{1}{2}{3}{4}",
            "<color=",
            ((Color)color).ToString(),
            ">",
            debug_string,
            "</color>"
            );

        Debug.Log(debug_string);
    }


    // Start is called before the first frame update
    void Start()
    {
        MyUnityPlugin.InitFoo(10);
        MyUnityPlugin.TestDebugCalls();
        Debug.Log("FOO: " + MyUnityPlugin.DoFoo(2));
    }

    private void OnEnable()
    {
        RegisterDebugCallback(OnDebugCallback);

    }


}
