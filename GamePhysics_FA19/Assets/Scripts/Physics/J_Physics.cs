using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Physics
{
    static public float gravity = -9.8f;

    // 2D ------------------------------------------------------------------------------------

    static public void UpdatePosition2D(ref Vector2 position, ref Vector2 velocity, ref Vector2 acceleration, float dt)
    {
        // x(t+dt) = x(t) + v(t+dt) + 1/2(a(t)(dt*dt))
        position += (velocity * dt) + (.5f * (acceleration * (dt * dt)));
        // v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
    }

    static public void UpdateRotation2D(ref Vector3 rotation, ref Vector3 angularVelocity, ref Vector3 angularAcceleration, float dt)
    {
        // x(t+dt) = x(t) + v(t+dt) + 1/2(a(t)(dt*dt))
        rotation += (angularVelocity * dt) + (.5f * (angularAcceleration * (dt * dt)));
        // clamp degrees between 0 and 360
        rotation.z = rotation.z % 360;
        // v(t+dt) = v(t) + a(t)dt
        angularVelocity += angularAcceleration * dt;
    }

    static public void UpdateAcceleration2D(ref Vector2 acceleration, float massInv, ref Vector2 force)
    {
        //Newton 2
        acceleration = massInv * force;
        // All forces are applied for a frame and then reset
        force = Vector2.zero;
    }

    static public void UpdateAngularAcceleration2D(ref float angularAcceleration, float inertiaInv, ref float torque)
    {
        // Newton 2
        angularAcceleration = inertiaInv * torque;
        // All torques are applied for a frame and then reset
        torque = 0.0f;
    }

    // 3D ------------------------------------------------------------------------------------

    static public void UpdatePosition3DKinematic(ref Vector3 position, ref Vector3 velocity, ref Vector3 acceleration, float dt)
    {
        // x(t+dt) = x(t) + v(t+dt) + 1/2(a(t)(dt*dt))
        position += (velocity * dt) + (.5f * (acceleration * (dt * dt)));
        // v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
    }

    static public void UpdatePosition3DEuler(ref Vector3 position, ref Vector3 velocity, ref Vector3 acceleration, float dt)
    {
        // x(t+dt) = x(t) + v(t)dt
        //Euler's method
        //F(t+dt) = F(t) + f(t)dt
        //               + (dF/dt)dt
        position += velocity * dt;

        // v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
    }

    static public void UpdateRotation3D(ref J_Quaternion rotation, ref Vector3 angularVelocity, Vector3 angularAcceleration, float dt)
    {
        //Using the euler's method for rotation with quaternions

        //Rotation formula
        // w = angular velocity (It's omega but my computer can't make that symbol :( )
        // q' = q + w * q * dt / 2 
        //Sets up values
        J_Quaternion equationFirstPart = new J_Quaternion(rotation);
        J_Quaternion velQuat = new J_Quaternion(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0);

        equationFirstPart.MultiplyByQuat(velQuat);
        //(w * q) * dt
        equationFirstPart.Scale(dt * .5f);
        //(w * q * dt) / 2
        //equationFirstPart.Scale(.5f);
        // q' = q + (w * q * dt / 2 )
        rotation.AddQuaternion(equationFirstPart);

        //Normilizes to prevent weirdness with addition
        rotation.Normalize();

        //Update angular velocity
        angularVelocity += angularAcceleration * dt;       
    }

    static public void UpdateAcceleration3D(ref Vector3 acceleration, float massInv, ref Vector3 force)
    {
        //Newton 2
        acceleration = massInv * force;
        // All forces are applied for a frame and then reset
        force = Vector3.zero;
    }

    static public void UpdateAngularAcceleration3D(ref Vector3 angularAcceleration, float[,] inertiaInv, Vector3 torqueCummalative)
    {
        /*  Angular accell is decomposed into an axis & a rate of angular changes (AKA)
         *  Theta = RA | A is the axis and R is the tate of wich it is spinning (mesuared in radians per second)
         *  New angular accelerations is given by Theta = theta + W | W is the direcion of the spin
         *  
         */


        //Not using torque calculations at the moment, so just hard set an acceleration



        //angularAcceleration = inertiaInv * torqueCummalative; //Updates angular acceleration based on the delta time


    }
}
