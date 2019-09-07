using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    [SerializeField]
    private PhysicsMethods.MethodType positionMethodType = 0;
    private PhysicsMethods positionPhysicsMethod = null;

    [SerializeField]
    private PhysicsMethods.MethodType rotationMethodType = 0;
    private PhysicsMethods rotationPhysicsMethod = null;

    [SerializeField]
    private List<PhysicsMethods> physicsMethods = null;

    [Header("Behavior")]
    [SerializeField]
    private bool oscilatePosition = false;
    [SerializeField]
    private bool oscilateRotation = false;
    [SerializeField]
    [Range(0, 10)]
    private float accelerationSpeed = 0.0f;
    [SerializeField]
    [Range(0, 10)]
    private float angularAccelerationSpeed = 0.0f;

    // step 1
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

    void Awake()
    {
        // Assert physics methods
        Debug.Assert(physicsMethods[0]);
        Debug.Assert(physicsMethods[1]);

        // Init Variables
        InitStartingVariables();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Update method
        UpdatePhysicsMethod();

        // Update position and rotation
        positionPhysicsMethod.UpdatePosition(ref position, ref velocity, ref acceleration, Time.fixedDeltaTime);
        rotationPhysicsMethod.UpdateRotation(ref rotation, ref angularVelocity, ref angularAcceleration, Time.fixedDeltaTime);

        // apply to transform
        transform.position = position;
        transform.eulerAngles = rotation * 20.0f; // * 20.0f is to make rotation noticable
        // step 4
        // test
        if (oscilatePosition)
        {
            acceleration.x = -Mathf.Sin(Time.fixedTime);
        }
        else
        {
            acceleration.x = accelerationSpeed;
        }
        if (oscilateRotation)
        {
            angularAcceleration.z = -Mathf.Sin(Time.fixedTime);
        }
        else
        {
            angularAcceleration.z = angularAccelerationSpeed;
        }
    }

    // Update physics methods if they change
    private void UpdatePhysicsMethod()
    {
        positionPhysicsMethod = physicsMethods[(int)positionMethodType];
        rotationPhysicsMethod = physicsMethods[(int)rotationMethodType];
    }

    private void InitStartingVariables()
    {
        // Init test values
        oscilatePosition = true;
        oscilateRotation = true;
        accelerationSpeed = 1.0f;
        angularAccelerationSpeed = 1.0f;

        // Init starting position and rotation
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
    }
}
