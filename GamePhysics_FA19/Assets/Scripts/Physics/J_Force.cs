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

    public static Vector2 GenerateForce_Friction_Static(Vector2 f_normal, Vector2 f_opposing, float frictionCoefficient_static)
    {
        // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
        if (-f_opposing.x < (f_normal.magnitude * frictionCoefficient_static))
        {
            return -f_opposing;
        }
        return -frictionCoefficient_static * f_normal;
    }

    public static Vector2 GenerateForce_Friction_Kinectic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {
        // f_friction_k = -coeff*|f_normal| * unit(vel)
        Vector2 f_friction_k = -frictionCoefficient_kinetic * particleVelocity / f_normal.magnitude;
        return f_friction_k;
    }

    public static Vector2 GenerateForce_Friction(Vector2 f_normal, Vector2 particleVelocity, Vector2 f_opposing, float frictionCoefficient_static, float frictionCoefficient_kinetic)
    {
        Vector2 f_friction = Vector2.zero;
        // If object is not moving, calculate static friction
        if(particleVelocity.magnitude == 0.0f)
        {
            f_friction = GenerateForce_Friction_Static(f_normal, f_opposing, frictionCoefficient_static);
        }
        // If object is moving, calculate kinetic friction
        else if(particleVelocity.magnitude > 0.0f)
        {
            f_friction = GenerateForce_Friction_Kinectic(f_normal, particleVelocity, frictionCoefficient_kinetic);
        }
        return f_friction;
    }

    public static Vector2 GenerateForce_Drag(Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        // v^2
        Vector2 vsqr = fluidVelocity * fluidVelocity;
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

        private static Vector2 Vec2Sqr(Vector2 v2)
    {
        return (v2 * v2);
    }
}
