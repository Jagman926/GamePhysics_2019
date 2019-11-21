using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPPlugin_Initialize : MonoBehaviour
{
    // Initialize GPPlugin information
    void Start()
    {
        GPPlugin.InitFoo(10);
        GPPlugin.TestDebugCalls();
        Debug.Log("FOO: " + GPPlugin.DoFoo(2));
    }
}
