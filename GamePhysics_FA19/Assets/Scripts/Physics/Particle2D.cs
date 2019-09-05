using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    enum PositionMethod
    {
        EULER = 0,
        KINEMATICS
    }
    enum RotationMethod
    {
        EULER = 0,
        KINEMATICS
    }

    [SerializeField]
    private PhysicsMethods physicsMethod;

    [Header("Position Method")]
    [SerializeField]
    private PositionMethod positionMethod = 0;

    [Header("Rotation Method")]
    [SerializeField]
    private RotationMethod rotationMethod = 0;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (positionMethod)
        {
            case PositionMethod.EULER:
                UpdatePositionEulerExplicit(Time.fixedDeltaTime);
                break;

            case PositionMethod.KINEMATICS:
                UpdatePositionKinematic(Time.fixedDeltaTime);
                break;
        }

        switch (rotationMethod)
        {
            case RotationMethod.EULER:
                UpdateRotationEulerExplicit(Time.fixedDeltaTime);
                break;

            case RotationMethod.KINEMATICS:
                UpdateRotationKinematics(Time.fixedDeltaTime);
                break;
        }

        // apply to transform
        transform.position = position;
        transform.eulerAngles = rotation * 20.0f;
        // step 4
        // test
        acceleration.x = -Mathf.Sin(Time.fixedTime);
        angularAcceleration.z = -Mathf.Sin(Time.fixedTime);
    }

    // step 2
    private void UpdatePositionEulerExplicit(float dt)
    {
        // x(t+dt) = x(t) + v(t)dt
        // Euler's Method
        // F(t+dt) = F(t) + f(t)dt
        //                + (df/dt)dt
        position += velocity * dt;

        // v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
    }

    private void UpdatePositionKinematic(float dt)
    {
        // v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
        // x(t+dt) = x(t) + v(t+dt) + 1/2(a(t)(dt*dt))
        position += (velocity * dt) + (.5f * (acceleration * (dt * dt)));
    }

    private void UpdateRotationEulerExplicit(float dt)
    {
        // x(t+dt) = x(t) + v(t)dt
        // Euler's Method
        // F(t+dt) = F(t) + f(t)dt
        //                + (df/dt)dt
        rotation += angularVelocity * dt;
        // v(t+dt) = v(t) + a(t)dt
        angularVelocity += angularAcceleration * dt;
    }

    private void UpdateRotationKinematics(float dt)
    {
        // v(t+dt) = v(t) + a(t)dt
        angularVelocity += angularAcceleration * dt;
        // x(t+dt) = x(t) + v(t+dt) + 1/2(a(t)(dt*dt))
        rotation += (angularVelocity * dt) + (.5f * (angularAcceleration * (dt * dt)));
    }


}
