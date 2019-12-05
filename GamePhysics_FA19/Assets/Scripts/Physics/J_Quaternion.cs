﻿using System.Collections;
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

    public J_Quaternion(J_Quaternion quaternion)
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

    public void AddQuaternion(J_Quaternion quatToAdd)
    {
        x += quatToAdd.x;
        y += quatToAdd.y;
        z += quatToAdd.z;
        w += quatToAdd.w;
    }

    public float[,] GetToRotationMatrix()
    {
        float[,] output = new float[3,3];
        float wSquared = (w * w);
        float xSquared = (x * x);
        float ySquared = (y * y);
        float zSquared = (z * z);


        output[0, 0] = wSquared + xSquared - ySquared - zSquared;
        output[0, 1] = 2 * (x * y - w * z);
        output[0, 2] = 2 * (x * z + w * y);

        output[1, 0] = 2 * (x * y + w * z);
        output[1, 1] = wSquared - xSquared + ySquared - zSquared;
        output[1, 2] = 2 * (y * z - w * x);

        output[2, 0] = 2 * (x * z - w * y);
        output[2, 1] = 2 * (z * y + w * x);
        output[2, 2] = wSquared - xSquared - ySquared + zSquared;

        return output;
    }

    public float[,] GetRotationMatrixInverse()
    {
        float[,] output = new float[3, 3];
        float[,] rotMatrix = GetRotationMatrixInverse();

        output[0, 0] = rotMatrix[0, 0];
        output[0, 1] = rotMatrix[1, 0];
        output[0, 2] = rotMatrix[2, 0];

        output[1, 0] = rotMatrix[0, 1];
        output[1, 1] = rotMatrix[1, 1];
        output[1, 2] = rotMatrix[2, 1];

        output[2, 0] = rotMatrix[0, 2];
        output[2, 1] = rotMatrix[1, 2];
        output[2, 2] = rotMatrix[2, 2];

        return output;
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
        quatA = Quaternion.FromToRotation(fromDirection, toDirection);
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
        Quaternion quatA = new Quaternion(x, y, z, w);
        J_Quaternion newJQuat = new J_Quaternion(Quaternion.Inverse(quatA));
        return newJQuat;
    }

    public float kEpsilon()
    {
        return Quaternion.kEpsilon;
    }

    public J_Quaternion Lerp(J_Quaternion a, J_Quaternion b, float t)
    {
        Quaternion quatA = new Quaternion(a.x, a.y, a.z, a.w);
        Quaternion quatB = new Quaternion(b.x, b.y, b.z, b.w);
        J_Quaternion newJQuat = new J_Quaternion(Quaternion.Lerp(quatA, quatB, t));
        return newJQuat;
    }

    public J_Quaternion LookRotation(Vector3 forwards, Vector3 upwards)
    {
        Quaternion quatA = new Quaternion(x, y, z, w);
        quatA = Quaternion.LookRotation(forwards, upwards);
        J_Quaternion newJQuat = new J_Quaternion(quatA);
        return newJQuat;
    }

    public void MultiplyByVector3(Vector3 a)
    {


        x *= a.x;
        y *= a.y;
        z *= a.z;


        //Vector3 quatEuler = new J_Quaternion(x,y,z,w).ToEulerAngles();
        //quatEuler = Vector3.Scale(quatEuler, a);
        //SetQuaterntion(Quaternion.Euler(quatEuler.x, quatEuler.y, quatEuler.z));
    }

    // Multipy the inputed vector by the quat
    public void MultiplyVector3ByQuat(ref Vector3 a)
    {
        a.x = x * a.x;
        a.y = y * a.y;
        a.z = z * a.z;
    }

    public void MultiplyByQuat(J_Quaternion a)
    {
        Quaternion quatA = new Quaternion(a.x, a.y, a.z, a.w);
        Quaternion quatB = new Quaternion(x, y, z, w);

        SetQuaterntion(quatB * quatA);
    }

    public J_Quaternion Normalize()
    {
        Quaternion quatA = new Quaternion(x, y, z, w);
        quatA.Normalize();
        J_Quaternion newJQuat = new J_Quaternion(quatA);
        return newJQuat;
    }

    public J_Quaternion RotateTowards(J_Quaternion from, J_Quaternion to, float maxDegreesDelta)
    {
        Quaternion quatA = new Quaternion(from.x, from.y, from.z, from.w);
        Quaternion quatB = new Quaternion(to.x, to.y, to.z, to.w);
        J_Quaternion newJQuat = new J_Quaternion(Quaternion.RotateTowards(quatA, quatB, maxDegreesDelta));
        return newJQuat;
    }

    public void Scale(float scalar)
    {
        x *= scalar;
        y *= scalar;
        z *= scalar;
        w *= scalar;
    }

    public J_Quaternion Slerp(J_Quaternion a, J_Quaternion b, float t)
    {
        Quaternion quatA = new Quaternion(a.x, a.y, a.z, a.w);
        Quaternion quatB = new Quaternion(b.x, b.y, b.z, b.w);
        J_Quaternion newJQuat = new J_Quaternion(Quaternion.Slerp(quatA, quatB, t));
        return newJQuat;
    }

    public Vector3 ToEulerAngles()
    {
        Quaternion quatA = new Quaternion(x, y, z, w);
        return quatA.eulerAngles;
    }
}
