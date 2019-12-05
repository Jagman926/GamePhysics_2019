using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Rotatable : MonoBehaviour
{
    // Main Camera
    Camera mainCamera = null;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, 1, 0));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, -1, 0));
        }
    }
}
