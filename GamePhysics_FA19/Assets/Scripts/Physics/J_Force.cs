using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Force
{
    public static Vector2 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector2 worldUp)
    {
        // f = mg
        Vector2 f_gravity = particleMass * gravitationalConstant * worldUp;
        return f_gravity;
    }

    public static Vector2 GenerateForce_Normal(Vector2 f_gravity, Vector2 surfaceNormal_unit)
    {
        // f_normal = proj(-f_gravity, surfaceNormal_unit)
        Vector2 f_normal = Vector3.ProjectOnPlane(-f_gravity, surfaceNormal_unit);
        return f_normal;
    }

    public static Vector2 GenerateForce_Sliding(Vector2 f_gravity, Vector2 f_normal)
    {
        // f_sliding = f_gravity + f_normal
        Vector2 f_sliding = f_gravity + f_normal;
        return f_sliding;
    }

    public static Vector2 GenerateForce_Friction(Vector2 f_normal, Vector2 particleVelocity, Vector2 f_opposing, float frictionCoefficient_static, float frictionCoefficient_kinetic)
    {
        // static max
        float staticFrictionMax = f_normal.magnitude * frictionCoefficient_static;

        // If object is moving, calculate kinetic friction
        if (particleVelocity.magnitude > 0.0f)
        {
            // f_friction_k = -coeff*|f_normal| * unit(vel)
            return -frictionCoefficient_kinetic * f_normal.magnitude * particleVelocity.normalized;
        }
        // If opposing is less than max
        else if (-f_opposing.magnitude < staticFrictionMax)
        {
            // f_friction_s = -f_opposing if less than max (max amount is coeff*|f_normal|)
            return -f_opposing;
        }
        // If object is not moving, calculate static friction
        else if (particleVelocity.magnitude == 0.0f)
        {
            // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
            return -frictionCoefficient_static * f_normal;
        }
        return Vector2.zero;
    }

    public static Vector2 GenerateForce_Drag(Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        // v^2
        Vector2 vsqr = fluidVelocity.magnitude * fluidVelocity;
        // f_drag = (p * u^2 * area * coeff)/2
        Vector2 f_drag = -0.5f * (fluidDensity * vsqr * objectArea_crossSection * objectDragCoefficient);
        return f_drag;
    }

    public static Vector2 GenerateForce_Spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        // f_spring = -coeff*(spring length - spring resting length)
        Vector2 springLength = particlePosition - anchorPosition;
        Vector2 f_spring = -springStiffnessCoefficient * (springLength - new Vector2(0.0f, springRestingLength));
        return f_spring;
    }

    //---------------------------------------------------------------------------------------------------
}
