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

    private Vector2 force;

    // Test Variables
    private bool touchingFloor;
    Vector2 f_gravity, f_normal;

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

        // Add force
        Vector2 surfaceNormal = new Vector2(Mathf.Sin(-30), Mathf.Cos(-30));
        // gravity
        f_gravity = J_Force.GenerateForce_Gravity(mass, J_Physics.gravity, Vector2.up);
        AddForce(f_gravity);
        // normal
        f_normal = J_Force.GenerateForce_Normal(f_gravity, surfaceNormal);
        if (touchingFloor)
            AddForce(f_normal);
    }

    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag == "Floor")
        {
            touchingFloor = true;
        }
        else
        {
            touchingFloor = false;
        }
    }

    private void InitStartingVariables()
    {
        // Init starting mass
        SetMass(startingMass);
        // Init starting position and rotation
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
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
