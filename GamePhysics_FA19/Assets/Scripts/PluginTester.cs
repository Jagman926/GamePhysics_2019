using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MyUnityPlugin.InitFoo(10);
        Debug.Log("FOO: " + MyUnityPlugin.DoFoo(2));
    }


}
