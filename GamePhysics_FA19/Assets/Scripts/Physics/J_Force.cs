using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Force
{
    // Vector2 GenerateForce_gravity(Vector2 worldUp, float gravitationalConstant, float particleMass);
    public static Vector2 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector2 worldUp)
    {
        // f = mg
        Vector2 f_gravity = particleMass * gravitationalConstant * worldUp;
        return f_gravity;
    }

    // Vector2 GenerateForce_normal(Vector2 f_gravity, Vector2 surfaceNormal_unit);
    public static Vector2 GenerateForce_Normal()
    {
        // f_normal = proj(f_gravity, surfaceNormal_unit)
        return Vector2.zero;
    }

    // Vector2 GenerateForce_sliding(Vector2 f_gravity, Vector2 f_normal);
    public static Vector2 GenerateForce_Sliding()
    {
        // f_sliding = f_gravity + f_normal
        return Vector2.zero;
    }

    // Vector2 GenerateForce_friction_static(Vector2 f_normal, Vector2 f_opposing, float frictionCoefficient_static);
    public static Vector2 GenerateForce_Friction_Static()
    {
        // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
        return Vector2.zero;
    }

    // Vector2 GenerateForce_friction_kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic);
    public static Vector2 GenerateForce_Friction_Kinectic()
    {
        // f_friction_k = -coeff*|f_normal| * unit(vel)
        return Vector2.zero;
    }

    // Vector2 GenerateForce_drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient);
    public static Vector2 GenerateForce_Drag()
    {
        // f_drag = (p * u^2 * area * coeff)/2
        return Vector2.zero;
    }

    // Vector2 GenerateForce_spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient);
    public static Vector2 GenerateForce_Spring()
    {
        // f_spring = -coeff*(spring length - spring resting length)
        return Vector2.zero;
    }
}
