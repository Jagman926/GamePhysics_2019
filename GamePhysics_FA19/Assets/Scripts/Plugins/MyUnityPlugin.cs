using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

using System.Runtime.InteropServices;

public class MyUnityPlugin
{
    [DllImport("MyUnityPlugin")]
    public static extern int InitFoo(int f_new = 0);
    [DllImport("MyUnityPlugin")]
    public static extern int DoFoo(int barS = 0);
    [DllImport("MyUnityPlugin")]
    public static extern int TermFoo();
}
