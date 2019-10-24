using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Quaternion
{
    public float x, y, z, w;
    public J_Quaternion zero = new J_Quaternion();

    public J_Quaternion()
    {
        SetQuaterntion(0,0,0,0);
    }

    public void SetQuaterntion(float newX, float newY, float newZ, float newW)
    {
        x = newX;
        y = newY;
        z = newZ;
        w = newW;
    }
}
