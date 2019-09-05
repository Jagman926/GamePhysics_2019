using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsMethods : ScriptableObject
{
    public string MethodName;

    public abstract void UpdatePosition(float position, float velocity, float acceleration, float dt);
    public abstract void UpdateRotation(float rotation, float angularVelocity, float angularAcceleration, float dt);
}
