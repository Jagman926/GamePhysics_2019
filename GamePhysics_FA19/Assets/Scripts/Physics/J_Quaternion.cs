using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Quaternion
{
    public float x, y, z, w;

    public void SetQuaterntion(float newX, float newY, float newZ, float newW)
    {
        x = newX;
        y = newY;
        z = newZ;
        w = newW;
    }

    public void Zero()
    {
        SetQuaterntion(0, 0, 0, 0);
    }
}
