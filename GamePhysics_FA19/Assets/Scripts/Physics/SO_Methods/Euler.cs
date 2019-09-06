using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Physics Methods/Euler")]
public class Euler : PhysicsMethods
{
    public override void UpdatePosition(ref Particle particle, float dt)
    {
        // x(t+dt) = x(t) + v(t)dt
        // Euler's Method
        // F(t+dt) = F(t) + f(t)dt
        //                + (df/dt)dt
        particle.mPosition += particle.mVelocity * dt;

        // v(t+dt) = v(t) + a(t)dt
        particle.mVelocity += particle.mAcceleration * dt;
    }

    public override void UpdateRotation(ref Particle particle, float dt)
    {
        // v(t+dt) = v(t) + a(t)dt
        particle.mAngularVelocity += particle.mAngularAcceleration * dt;
        // x(t+dt) = x(t) + v(t+dt) + 1/2(a(t)(dt*dt))
        particle.mRotation += (particle.mAngularVelocity * dt) + (.5f * (particle.mAngularAcceleration * (dt * dt)));
    }
}
