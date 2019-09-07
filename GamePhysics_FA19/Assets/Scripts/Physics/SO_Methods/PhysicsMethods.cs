using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsMethods : ScriptableObject
{
    public enum MethodType
    {
        Euler = 0,
        Kinematic
    };
    
    public string MethodName;

    public abstract void UpdatePosition(ref Vector2 position, ref Vector2 velocity, ref Vector2 acceleration, float dt);
    public abstract void UpdateRotation(ref Vector3 rotation, ref Vector3 angularVelocity, ref Vector3 angularAcceleration, float dt);
}
