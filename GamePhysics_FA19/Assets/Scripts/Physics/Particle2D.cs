using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    enum Shape
    {
        RECTANGLE = 0,  // I = 1/12 * m * (h^2 + w^2)
        DISK,           // I = 1/2 * m * r^2
        RING,           // I = 1/2 * m * (ro^2 + ri^2)
        ROD,            // I = 1/12 * m * l^2
    }

    // lab 2 step 1
    [Header("Object Variables")]
    [SerializeField]
    private Shape shapeType;
    [SerializeField]
    private float height;
    [SerializeField]
    private float length;
    [SerializeField]
    private float width;
    [SerializeField]
    private float radiusOuter;
    [SerializeField]
    private float radiusInner;

    [Header("Mass Variables")]
    [SerializeField]
    private float startingMass;
    private float mass, massInv;
    [SerializeField]
    private Vector2 centerOfMass;

    [Header("Inertia Variables")]
    [SerializeField]
    private float inertia, inertiaInv;

    [Header("Position Variables")]
    [SerializeField]
    private Vector2 position;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private Vector2 acceleration;

    [Header("Rotation Variables")]
    [SerializeField]
    private Vector3 rotation;
    [SerializeField]
    private Vector3 angularVelocity;
    [SerializeField]
    private Vector3 angularAcceleration;

    private Vector2 force;
    private float torque;

    // Force Variables
    Vector2 f_gravity, f_normal, f_sliding, f_friction, f_drag, f_spring;
    private float inclineDegrees = 340.0f;
    private float frictionCoeff_s = 0.61f, frictionCoeff_k = 0.47f;     // For: Aluminum | Mild Steel | Clean and Dry 
                                                                        // https://www.engineeringtoolbox.com/friction-coefficients-d_778.html
    private Vector2 fluidVelocity;
    private float fluidDensity = 0.001225f, dragCoefficient = 1.05f;    // For: Cube Drag Coefficient
                                                                        // https://en.wikipedia.org/wiki/Drag_coefficient 
                                                                        // For: Fluid Density
                                                                        // https://en.wikipedia.org/wiki/Density_of_air
    public GameObject anchorObject;
    private Vector2 anchorPosition;
    private float springRestingLength = 5.0f, springStiffnessCoefficient = 6.4f;

    // Test variables
    public Vector2 testForce;
    public Vector2 momentArm;


    void Start()
    {
        InitStartingVariables();
    }

    void TestTorque()
    {
        // Test cases

        // Left Arrow
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ApplyTorque(testForce, momentArm);
        // Right Arrow
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            ApplyTorque(testForce, momentArm);
    }

    void FixedUpdate()
    {
        // Update position and rotation
        J_Physics.UpdatePosition(ref position, ref velocity, ref acceleration, Time.fixedDeltaTime);
        J_Physics.UpdateRotation(ref rotation, ref angularVelocity, ref angularAcceleration, Time.fixedDeltaTime);

        // apply to transform
        transform.position = position;
        transform.eulerAngles = rotation;

        // Update acceleration | angular acceleration
        UpdateAcceleration();
        UpdateAngularAcceleration();

        TestTorque();

        /*
        // Surface normal
        Vector2 surfaceNormal = new Vector2(Mathf.Cos(inclineDegrees * Mathf.Deg2Rad), Mathf.Sin(inclineDegrees * Mathf.Deg2Rad));
        // gravity
        f_gravity = J_Force.GenerateForce_Gravity(mass, J_Physics.gravity, Vector2.up);
        // normal
        f_normal = J_Force.GenerateForce_Normal(f_gravity, surfaceNormal);
        // sliding
        f_sliding = J_Force.GenerateForce_Sliding(f_gravity, f_normal);
        AddForce(f_sliding);
        // friction
        f_friction = J_Force.GenerateForce_Friction(f_normal, velocity, f_sliding, frictionCoeff_s, frictionCoeff_k);
        AddForce(f_friction);
        // drag
        f_drag = J_Force.GenerateForce_Drag(velocity, fluidDensity, 1.0f, dragCoefficient);
        AddForce(f_drag);
        // spring
        // f_spring = J_Force.GenerateForce_Spring(particlePosition, anchorPosition, springRestingLength, springStiffnessCoefficient);
        // AddForce(f_spring);
        */
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

    public void AddForce(Vector2 newForce)
    {
        //D'Alembert
        force += newForce;
    }

    public void ApplyTorque(Vector2 force, Vector2 momentArm)
    {
        //D'Alembert
        // T = pf x F: T
        // pf = moment arm (point of applied force relative to center of mass)
        // F = applied force at pf
        torque += (force.magnitude * (momentArm - centerOfMass).magnitude);
    }

    private void UpdateAcceleration()
    {
        //Newton 2
        acceleration = massInv * force;
        // All forces are applied for a frame and then reset
        force = Vector2.zero;
    }

    private void UpdateAngularAcceleration()
    {
        // Newton 2
        angularAcceleration.z = inertiaInv * torque;
        // All torques are applied for a frame and then reset
        torque = 0.0f;
    }
}
