using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    enum Shape
    {
        RECTANGLE = 0,  // I = 1/12 * m * (h^2 + w^2)
        DISK,           // I = 1/2 * m * r^2
        RING,           // I = 1/2 * m * (ro^2 + ri^2)
        ROD,            // I = 1/12 * m * l^2
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
    private float inertia = 0.0f, inertiaInv = 0.0f;
    private Vector3 force = Vector3.zero;
    private float torque = 0.0f;
    //
    Vector2 f_gravity, f_normal, f_sliding, f_friction, f_drag, f_spring;
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
    public Vector3 rotation = Vector3.zero;
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
        J_Physics.UpdatePosition3D(ref position, ref velocity, ref acceleration, Time.fixedDeltaTime);
        J_Physics.UpdateRotation3D(ref rotation, ref angularVelocity, ref angularAcceleration, Time.fixedDeltaTime);

        // apply to transform
        transform.position = position;
        transform.eulerAngles = rotation;

        // Update acceleration | angular acceleration
        J_Physics.UpdateAcceleration3D(ref acceleration, massInv, ref force);
        J_Physics.UpdateAngularAcceleration3D(ref angularAcceleration, inertiaInv, ref torque);
    }

    private void InitStartingVariables()
    {
        // Init starting mass
        SetMass(startingMass);
        // Init starting inertia
        SetInertia();
        // Init starting position and rotation
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
        // Init anchorPosition
        // anchorPosition = new Vector2(anchorObject.transform.position.x, anchorObject.transform.position.y);
    }

    public void SetMass(float newMass)
    {
        mass = Mathf.Max(0.0f, newMass);
        massInv = Mathf.Max(0.0f, 1.0f / mass);
    }

    public void SetInertia()
    {
        /*
        Inertia Equations: Ian Millington - Game Physics Engine Development (pg 493)
        -----------------------------------------
        RECTANGLE ||  I = 1/12 * m * (h^2 + w^2)
        DISK      ||  I = 1/2 * m * r^2
        RING      ||  I = 1/2 * m * (ro^2 + ri^2)
        ROD       ||  I = 1/12 * m * l^2
        -----------------------------------------
        */
        switch (shapeType)
        {
            // Rectangle
            case Shape.RECTANGLE:
                inertia = 0.083f * mass * ((height * height) + (width * width));
                break;
            // Disk
            case Shape.DISK:
                inertia = 0.5f * mass * (radiusOuter * radiusOuter);
                break;
            // Ring
            case Shape.RING:
                inertia = 0.5f * mass * ((radiusOuter * radiusOuter) + (radiusInner * radiusInner));
                break;
            // Rod
            case Shape.ROD:
                inertia = 0.083f * mass * (length * length);
                break;
            // Default Case
            default:
                Debug.Log("Shape Type not set");
                inertia = 0.0f;
                break;
        }
        inertia = Mathf.Max(0.0f, inertia);
        inertiaInv = Mathf.Max(0.0f, 1.0f / inertia);
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
        torque += (force * (momentArm - centerOfMass).magnitude);
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
