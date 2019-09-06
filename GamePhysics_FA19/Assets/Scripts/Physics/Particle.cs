using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector2 mPosition, mVelocity, mAcceleration;
    public Vector3 mRotation, mAngularVelocity, mAngularAcceleration;

    public void InitParticle(Vector2 position, Vector2 velocity, Vector2 acceleration, Vector3 rotation, Vector3 angularVelocity, Vector3 angularAcceleration)
    {
        mPosition = position;
        mVelocity = velocity;
        mAcceleration = acceleration;
        mRotation = rotation;
        mAngularVelocity = angularVelocity;
        mAngularAcceleration = angularAcceleration;
    }
}
