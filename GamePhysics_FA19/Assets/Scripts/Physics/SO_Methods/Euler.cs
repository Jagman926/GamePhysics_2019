using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Physics Methods/Euler")]
public class Euler : PhysicsMethods
{
    public override void UpdatePosition(float position, float velocity, float acceleration, float dt)
    {
        // x(t+dt) = x(t) + v(t)dt
        // Euler's Method
        // F(t+dt) = F(t) + f(t)dt
        //                + (df/dt)dt
        position += velocity * dt;

        // v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
    }

    public override void UpdateRotation(float rotation, float angularVelocity, float angularAcceleration, float dt)
    {
        // v(t+dt) = v(t) + a(t)dt
        angularVelocity += angularAcceleration * dt;
        // x(t+dt) = x(t) + v(t+dt) + 1/2(a(t)(dt*dt))
        rotation += (angularVelocity * dt) + (.5f * (angularAcceleration * (dt * dt)));
    }
}
