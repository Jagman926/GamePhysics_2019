using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Quaternion
{
    public float x, y, z, w;

    /*  w = cos(theta/2)
     *  x = xsin(theta/2)
     *  y = ysin(theta/2)
     *  z = zsin(theta/2)
     */

     /* Quaturnien functions
      * Angle (quatA, quatB)
      * AngleAxis
      * AxisAngle
      * Dot
      * Equals
      * Euler
      * EulerAngles
      * EulterRotaion
      * FromToRotation
      * Identity
      * Inverse
      * kEpsilon
      * Lerp
      * LookRotaion
      * Normilize
      * RotateTowards
      * Slerp
      * ToEulerAngles
      */

    public void SetQuaterntion(float newX, float newY, float newZ, float newW)
    {
        x = newX;
        y = newY;
        z = newZ;
        w = newW;
    }

    public void SetQuaterntion(Quaternion newQuat)
    {
        x = newQuat.x;
        y = newQuat.y;
        z = newQuat.z;
        w = newQuat.w;
    }

    public Quaternion ToUnityQuaterntion()
    {
        return new Quaternion(x, y, z, w);
    }

    public void Zero()
    {
        SetQuaterntion(0, 0, 0, 0);
    }
}
