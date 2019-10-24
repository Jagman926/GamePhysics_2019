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

    public J_Quaternion()
    {
        SetQuaterntion(0, 0, 0, 0);
    }

    public J_Quaternion(float newX, float newY, float newZ, float newW)
    {
        x = newX;
        y = newY;
        z = newZ;
        w = newW;
    }

    public J_Quaternion(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }

    private void SetQuaterntion(float newX, float newY, float newZ, float newW)
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

    public J_Quaternion ToJQuaterntion(Quaternion quaternion)
    {
        return new J_Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }

    public void Zero()
    {
        SetQuaterntion(0, 0, 0, 0);
    }

    public float Angle(J_Quaternion a, J_Quaternion b)
    {
        Quaternion quatA = new Quaternion(a.x, a.y, a.z, a.w);
        Quaternion quatB = new Quaternion(b.x, b.y, b.z, b.w);

        return Quaternion.Angle(quatA, quatB);
    }

    public J_Quaternion AngleAxis(float angle, Vector3 axis)
    {
        J_Quaternion newJQuat = new J_Quaternion();
        return newJQuat.ToJQuaterntion(Quaternion.AngleAxis(angle, axis));
    }

    public float Dot(J_Quaternion a, J_Quaternion b)
    {
        Quaternion quatA = new Quaternion(a.x, a.y, a.z, a.w);
        Quaternion quatB = new Quaternion(b.x, b.y, b.z, b.w);

        return Quaternion.Dot(quatA, quatB);
    }

    public bool Equals(J_Quaternion a, J_Quaternion b)
    {
        Quaternion quatA = new Quaternion(a.x, a.y, a.z, a.w);
        Quaternion quatB = new Quaternion(b.x, b.y, b.z, b.w);

        return Quaternion.Equals(quatA, quatB);
    }

    public J_Quaternion Euler(float x, float y, float z)
    {
        Quaternion quatA = new Quaternion();
        quatA = Quaternion.Euler(x, y, z);
        J_Quaternion newJQuat = new J_Quaternion(quatA);
        return newJQuat;
    }

    public J_Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
    {
        Quaternion quatA = new Quaternion();
        Quaternion.FromToRotation(fromDirection, toDirection);
        J_Quaternion newJQuat = new J_Quaternion(quatA);
        return newJQuat;
    }

    public J_Quaternion Identity()
    {
        J_Quaternion newJQuat = new J_Quaternion(Quaternion.identity);
        return newJQuat;
    }

    public J_Quaternion Inverse()
    {
        return null;
    }

    /* Quaturnien functions
 * Inverse
 * kEpsilon
 * Lerp
 * LookRotaion
 * Normilize
 * RotateTowards
 * Slerp
 * ToEulerAngles
 */
}
