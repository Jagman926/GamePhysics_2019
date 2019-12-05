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

    static public void UpdateAngularAcceleration3D(ref Vector3 angularAcceleration, ref Vector3 torque, float[,] inertiaInv,  Matrix4x4 transformMat)
    {

        //Bring torque into local space using tranform matrix
        Vector3 localTorque = J_Physics.WorldToLocalDirection(torque, transformMat);

        //Apply inverse inertia tensor to locallized torque
        Vector3 torqueTensorCross = J_Physics.Mat3Vec3Cross(inertiaInv, localTorque);

        //Move back to world giving us angular acceleration by transformation matrix
        angularAcceleration = J_Physics.LocalToWorldDirection(torqueTensorCross, transformMat);

        torque = Vector3.zero;
    }

    // Math Functions ------------------------------------------------------------------------------------

    static public Vector3 LocalToWorldDirection( Vector3 vector, Matrix4x4 matrix)
    {
        Vector3 output = new Vector3();

        output.x = vector.x * matrix[0] +
                    vector.y * matrix[1] +
                    vector.z * matrix[2];

        output.y = vector.x * matrix[4] +
                    vector.y * matrix[5] +
                    vector.z * matrix[6];

        output.z = vector.x * matrix[8] +
                    vector.y * matrix[9] +
                    vector.z * matrix[10];

        return output;
    }

    static public Vector3 WorldToLocalDirection( Vector3 vector, Matrix4x4 matrix)
    {
        Vector3 output = new Vector3();
        Matrix4x4 inverseMatrix = matrix.inverse;


        output.x = vector.x * inverseMatrix[0] +
                    vector.y * inverseMatrix[4] +
                    vector.z * inverseMatrix[8];

        output.y = vector.x * inverseMatrix[1] +
                    vector.y * inverseMatrix[5] +
                    vector.z * inverseMatrix[9];

        output.z = vector.x * inverseMatrix[2] +
                    vector.y * inverseMatrix[6] +
                    vector.z * inverseMatrix[10];

        return output;
    }

    static public Vector3 WorldToLocalPosition(Vector3 worldPosition, Vector3 center, float[,] rotationMatrixInverse)
    {
        Vector3 output = worldPosition;

        output -= center;

        output = Mat3Vec3Dot(rotationMatrixInverse, output);


        return output;
    }

    static public Vector3 LocalToWorldPosition(Vector3 localPosition, Vector3 center, float[,] rotationMatrix)
    {
        Vector3 output = new Vector3();
    
        output = Mat3Vec3Dot(rotationMatrix, localPosition);

        output += center;

        return output;
    }

    static public Vector3 Mat3Vec3Cross(float[,] matrix, Vector3 vector)
    {
        Vector3 output = new Vector3();

        output.x = vector.x * matrix[0,0] +
            vector.y * matrix[1,0] +
            vector.z * matrix[2,0];

        output.y = vector.x * matrix[0,1] +
                    vector.y * matrix[1,1] +
                    vector.z * matrix[2,1];

        output.z = vector.x * matrix[0,2] +
                    vector.y * matrix[1,2] +
                    vector.z * matrix[2,2];

        return output;
    }

    static public Vector3 Mat3Vec3Dot(float[,] matrix, Vector3 vector)
    {
        Vector3 output = new Vector3();

        output.x = vector.x * matrix[0, 0] +
            vector.y * matrix[0, 1] +
            vector.z * matrix[0, 2];

        output.y = vector.x * matrix[1, 0] +
                    vector.y * matrix[1, 1] +
                    vector.z * matrix[1, 2];

        output.z = vector.x * matrix[2, 0] +
                    vector.y * matrix[2, 1] +
                    vector.z * matrix[2, 2];

        return output;
    }

    static public float[,] GetMat3Transpose(float[,] mat3)
    {
        float[,] output = new float[3, 3];

        output[0, 0] = mat3[0, 0];
        output[0, 1] = mat3[1, 0];
        output[0, 2] = mat3[2, 0];

        output[1, 0] = mat3[0, 1];
        output[1, 1] = mat3[1, 1];
        output[1, 2] = mat3[2, 1];

        output[2, 0] = mat3[0, 2];
        output[2, 1] = mat3[1, 2];
        output[2, 2] = mat3[2, 2];

        return output;
    }

    static public float[,] GetMat3Inverse(float[,] mat3)
    {
        float[,] output = mat3;

        output[0, 0] = 1 / output[0, 0];
        output[1, 1] = 1 / output[1, 1];
        output[2, 2] = 1 / output[2, 2];

        return output;
    }
}
