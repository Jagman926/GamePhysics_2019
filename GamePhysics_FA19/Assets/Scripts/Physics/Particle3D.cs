using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    enum Shape
    {
        SolidSphere,
        HollowSphere,
        SolidBox,
        HollowBox,
        SolidCylinder,
        SolidCone
    }


    [Header("Object Shape Variables")]
    [SerializeField]
    private Shape shapeType = 0;
    public float height = 0.0f;
    public float length = 0.0f;
    public float width = 0.0f;
    public float radiusOuter = 0.0f;
    public float radiusInner = 0.0f;

    [Header("Mass Variables")]
    [SerializeField]
    private float startingMass = 0.0f;
    private float mass = 0.0f, massInv = 0.0f;
    [SerializeField]
    private Vector3 centerOfMass = Vector3.zero;

    [Header("Force Variables")]
    [SerializeField]
    //private Matrix4x4 inertia = Matrix4x4.zero, inertiaInv = Matrix4x4.zero;
    //Unity does not support 3x3 matrix so we can either make our own or use a 4x4 matrix
    private static float[][] inertia =
    {
        new float[]{0, 0, 0},
        new float[]{0, 0, 0},
        new float[]{0, 0, 0}
    };
    private static float[][] inertiaInv =
{
        new float[]{0, 0, 0},
        new float[]{0, 0, 0},
        new float[]{0, 0, 0}
    };
    private Vector3 force = Vector3.zero;
    private Vector3 torque = Vector3.zero;
    //
    Vector3 f_gravity, f_normal, f_sliding, f_friction, f_drag, f_spring;
    private float inclineDegrees = 340.0f;
    private float frictionCoeff_s = 0.61f, frictionCoeff_k = 0.47f;
    private Vector3 fluidVelocity;
    private float fluidDensity = 0.001225f, dragCoefficient = 1.05f;
    // Spring variables
    [SerializeField]
    private GameObject anchorObject = null;
    private Vector3 anchorPosition = Vector3.zero;
    private float springRestingLength = 5.0f, springStiffnessCoefficient = 6.4f;

    [Header("Position Variables")]
    public Vector3 position = Vector3.zero;
    [SerializeField]
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private float maxVelocity = 0.0f;
    [SerializeField]
    private Vector3 acceleration = Vector3.zero;

    [Header("Rotation Variables")]
    public J_Quaternion rotation;
    [SerializeField]
    private Vector3 angularVelocity = Vector3.zero;
    [SerializeField]
    private Vector3 angularAcceleration = Vector3.zero;

    void Start()
    {
        InitStartingVariables();
    }

    void FixedUpdate()
    {
        // Update position and rotation
        J_Physics.UpdatePosition3DKinematic(ref position, ref velocity, ref acceleration, Time.fixedDeltaTime);
        J_Physics.UpdateRotation3D(ref rotation, ref angularVelocity, angularAcceleration, Time.fixedDeltaTime);

        // apply to transform
        transform.position = position;
        //transform.eulerAngles = rotation;
        transform.rotation = rotation.ToUnityQuaterntion();

        // Update acceleration | angular acceleration
        J_Physics.UpdateAcceleration3D(ref acceleration, massInv, ref force);
        J_Physics.UpdateAngularAcceleration3D(ref angularAcceleration, inertiaInv, torque);
    }

    private void InitStartingVariables()
    {
        // Init starting mass
        SetMass(startingMass);
        // Init starting inertia
        SetInertia();
        // Init starting position and rotation
        position = transform.position;
        rotation = new J_Quaternion();
        rotation.SetQuaterntion(transform.rotation);
        //rotation.Zero();
        // Init anchorPosition
        //anchorPosition = new Vector3(anchorObject.transform.position.x, anchorObject.transform.position.y, anchorObject.transform.position.z);
    }

    public void SetMass(float newMass)
    {
        mass = Mathf.Max(0.0f, newMass);
        massInv = Mathf.Max(0.0f, 1.0f / mass);
    }

    public void SetInertia()
    {
        /*
        Inertia Equations from slides in class (deck05angular.pdf)
        -----------------------------------------
                Solid Sphere 
        |-----------------------------|
        |(2/5)mr^2|         |         |
        |         |(2/5)mr^2|         |
        |         |         |(2/5)mr^2|
        |-----------------------------|
                Hollow Sphere (shell)
        |-----------------------------|
        |(2/3)mr^2|         |         |
        |         |(2/3)mr^2|         |
        |         |         |(2/3)mr^2|
        |-----------------------------|
                Solid Box
        |---------------------------------------------------|
        |(1/12)m(h^2+d^2) |                |                |
        |                 |(1/12)m(d^2+w^2)|                |
        |                 |                |(1/12)m(w^2+h^2)|
        |---------------------------------------------------|
                Hollow Box
        |-----------------------------------------------|
        |(5/3)m(h^2+d^2)|               |               |
        |               |(5/3)m(d^2+w^2)|               |
        |               |               |(5/3)m(w^2+h^2)|
        |-----------------------------------------------|
                Solid Cylinder
        |-----------------------------------------------|
        |(1/12)m(3r^2+h^2)|                 |           |
        |                 |(1/12)m(3r^2+h^2)|           |
        |                 |                 |(1/2)mr^2  |
        |-----------------------------------------------|
                Solid Cone
        |-----------------------------------------------------|
        |(3/5)mh^2+(3/20)mr^2|                    |           |
        |                    |(3/5)mh^2+(3/20)mr^2|           |
        |                    |                    |(3/10)mr^2 |
        |-----------------------------------------------------|

        -----------------------------------------
        */
        switch (shapeType)
        {
            // Solid Sphere
            case Shape.SolidSphere:
                inertia[0][0] = (2 / 5) * mass * (radiusOuter * radiusOuter);
                inertia[1][1] = (2 / 5) * mass * (radiusOuter * radiusOuter);
                inertia[2][2] = (2 / 5) * mass * (radiusOuter * radiusOuter);
                break;
            // Hollow Sphere
            case Shape.HollowSphere:
                inertia[0][0] = (2 / 3) * mass * (radiusOuter * radiusOuter);
                inertia[1][1] = (2 / 3) * mass * (radiusOuter * radiusOuter);
                inertia[2][2] = (2 / 3) * mass * (radiusOuter * radiusOuter);
                break;
            // Solid Box
            case Shape.SolidBox:
                inertia[0][0] = (1 / 12) * mass * ((height * height) + (length * length));
                inertia[1][1] = (1 / 12) * mass * ((length * length) + (width * width));
                inertia[2][2] = (1 / 12) * mass * ((height * height) + (width * width));
                break;
            // Hollow Box
            case Shape.HollowBox:
                inertia[0][0] = (5 / 3) * mass * ((height * height) + (length * length));
                inertia[1][1] = (5 / 3) * mass * ((length * length) + (width * width));
                inertia[2][2] = (5 / 3) * mass * ((height * height) + (width * width));
                break;
            // Solid Cylinder
            case Shape.SolidCylinder:
                inertia[0][0] = (1 / 12) * mass * (3*(radiusOuter * radiusOuter) + (height * height));
                inertia[1][1] = (1 / 12) * mass * (3*(radiusOuter * radiusOuter) + (height * height));
                inertia[2][2] = (1 / 2) * mass * (radiusOuter * radiusOuter);
                break;
            //Solid Cone
            case Shape.SolidCone:
                inertia[0][0] = (3 / 5) * mass * (height * height) + (3/20) * mass * (radiusOuter * radiusOuter);
                inertia[1][1] = (3 / 5) * mass * (height * height) + (3/20) * mass * (radiusOuter * radiusOuter);
                inertia[2][2] = (3 / 10) * mass * (radiusOuter * radiusOuter);
                break;
            // Default Case
            default:
                Debug.Log("Shape Type not set");
                break;
        }
        //inertia = Mathf.Max(0.0f, inertia);
        //inertiaInv = Mathf.Max(0.0f, 1.0f / inertia);
    }

    public float GetMass()
    {
        return mass;
    }

    public float GetInvMass()
    {
        return massInv;
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        velocity = newVelocity;

        velocity.x = Mathf.Clamp(velocity.x, -maxVelocity, maxVelocity);
        velocity.y = Mathf.Clamp(velocity.y, -maxVelocity, maxVelocity);
    }

    public void AddForce(Vector3 newForce)
    {
        //D'Alembert
        force += newForce;
    }

    public void AddForceForward(float forceIntensity)
    {
        // Get rotated vector
        Vector2 rotatedVector = gameObject.transform.rotation * Vector3.up * forceIntensity;
        // add force
        AddForce(rotatedVector);
    }

    public void ApplyTorque(float force, Vector3 momentArm)
    {
        //D'Alembert
        // T = pf x F: T
        // pf = moment arm (point of applied force relative to center of mass)
        // F = applied force at pf
        torque += (force * (momentArm - centerOfMass)); //Got rid of mangitude to get working
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public void SetPosition(Vector2 newPosition)
    {
        position = newPosition;
    }
}
