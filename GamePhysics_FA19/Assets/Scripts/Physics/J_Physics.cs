using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Physics
{
    static public float gravity = 9.8f;

    static public void UpdatePosition(ref Vector2 position, ref Vector2 velocity, ref Vector2 acceleration, float dt)
    {
        // v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
        // x(t+dt) = x(t) + v(t+dt) + 1/2(a(t)(dt*dt))
        position += (velocity * dt) + (.5f * (acceleration * (dt * dt)));
    }

    static public void UpdateRotation(ref Vector3 rotation, ref Vector3 angularVelocity, ref Vector3 angularAcceleration, float dt)
    {
        // v(t+dt) = v(t) + a(t)dt
        angularVelocity += angularAcceleration * dt;
        // x(t+dt) = x(t) + v(t+dt) + 1/2(a(t)(dt*dt))
        rotation += (angularVelocity * dt) + (.5f * (angularAcceleration * (dt * dt)));
    }
}
