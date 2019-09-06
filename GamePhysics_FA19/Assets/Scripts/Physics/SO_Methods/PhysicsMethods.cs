using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsMethods : ScriptableObject
{
    public string MethodName;

    public abstract void UpdatePosition(ref Particle particle, float dt);
    public abstract void UpdateRotation(ref Particle particle, float dt);
}
