using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    // lab 2 step 1
    [Header("Mass Variables")]
    [SerializeField]
    private float startingMass;
    private float mass, massInv;

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

    [Header("Test Variables")]
    [SerializeField]
    private bool gravity;
    [SerializeField]
    private bool normal;
    [SerializeField]
    private bool friction;
    [SerializeField]
    private bool drag;
    [SerializeField]
    private bool spring;

    private Vector2 force;

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

    void Start()
    {
        InitStartingVariables();
    }

    void FixedUpdate()
    {
        // Update position and rotation
        J_Physics.UpdatePosition(ref position, ref velocity, ref acceleration, Time.fixedDeltaTime);
        J_Physics.UpdateRotation(ref rotation, ref angularVelocity, ref angularAcceleration, Time.fixedDeltaTime);

        // apply to transform
        transform.position = position;
        transform.eulerAngles = rotation;

        // Update acceleration
        UpdateAcceleration();

        // Surface normal
        Vector2 surfaceNormal = new Vector2(Mathf.Cos(inclineDegrees * Mathf.Deg2Rad), Mathf.Sin(inclineDegrees * Mathf.Deg2Rad));
        // gravity
        f_gravity = J_Force.GenerateForce_Gravity(mass, J_Physics.gravity, Vector2.up);
        if (gravity && !normal)
        {
            AddForce(f_gravity);
        }
        // normal
        f_normal = J_Force.GenerateForce_Normal(f_gravity, surfaceNormal);
        if (normal && !gravity)
        {
            AddForce(f_normal);
        }
        // sliding
        if (gravity && normal)
        {
            f_sliding = J_Force.GenerateForce_Sliding(f_gravity, f_normal);
            AddForce(f_sliding);
        }
        // friction
        if (friction)
        {
            f_friction = J_Force.GenerateForce_Friction(f_normal, velocity, f_sliding, frictionCoeff_s, frictionCoeff_k);
            AddForce(f_friction);
        }
        // drag
        if (drag)
        {
            f_drag = J_Force.GenerateForce_Drag(velocity, fluidDensity, 1.0f, dragCoefficient);
            AddForce(f_drag);
        }
        if (spring)
        {
            f_spring = J_Force.GenerateForce_Spring(position, anchorPosition, springRestingLength, springStiffnessCoefficient);
            AddForce(f_spring);
        }
    }

    private void InitStartingVariables()
    {
        // Init starting mass
        SetMass(startingMass);
        // Init starting position and rotation
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
        // Init anchorPosition
        anchorPosition = new Vector2(anchorObject.transform.position.x, anchorObject.transform.position.y);
    }

    public void SetMass(float newMass)
    {
        mass = Mathf.Max(0.0f, newMass);
        massInv = Mathf.Max(0.0f, 1.0f / mass);
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

    private void UpdateAcceleration()
    {
        //Newton 2
        acceleration = massInv * force;
        // All forces are applied for a frame and then reset
        force = Vector2.zero;
    }
}
