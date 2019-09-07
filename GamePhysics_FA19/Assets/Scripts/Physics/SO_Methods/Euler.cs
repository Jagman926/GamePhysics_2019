﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Physics Methods/Euler")]
public class Euler : PhysicsMethods
{
    public override void UpdatePosition(ref Vector2 position, ref Vector2 velocity, ref Vector2 acceleration, float dt)
    {
        // x(t+dt) = x(t) + v(t)dt
        // Euler's Method
        // F(t+dt) = F(t) + f(t)dt
        //                + (df/dt)dt
        position += velocity * dt;

        // v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;

        // Debug
        Debug.Log("EULER POSITION");
    }

    public override void UpdateRotation(ref Vector3 rotation, ref Vector3 angularVelocity, ref Vector3 angularAcceleration, float dt)
    {
        // x(t+dt) = x(t) + v(t)dt
        // Euler's Method
        // F(t+dt) = F(t) + f(t)dt
        //                + (df/dt)dt
        rotation += angularVelocity * dt;
        // v(t+dt) = v(t) + a(t)dt
        angularVelocity += angularAcceleration * dt;

        Debug.Log("EULER ROTATION");
    }
}
