using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    enum MotionMethod
    {
        EULER = 0,
        KINEMATICS
    }

    [Header("Motion Method")]
    [SerializeField]
    private bool EulerExplicit = false;

    // step 1
    [Header("Motion Variables")]
    [SerializeField]
    private Vector2 position;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private Vector2 acceleration;
    [SerializeField]
    private Vector2 rotation;
    [SerializeField]
    private Vector2 angularVelocity;
    [SerializeField]
    private Vector2 angularAcceleration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (EulerExplicit)
        {
            // step 3
            // integrate
            UpdatePositionEulerExplicit(Time.fixedDeltaTime);
            UpdateRotationEulerExplicit(Time.fixedDeltaTime);
            // apply to transform
            transform.position = position;
            transform.eulerAngles = rotation;
            // step 4
            // test
            acceleration.x = -Mathf.Sin(Time.fixedTime);
            angularAcceleration.x = -Mathf.Sin(Time.fixedTime);
        }
        else
        {
            // step 3
            // integrate
            UpdatePositionKinematic(Time.fixedDeltaTime);
            UpdateRotationKinematics(Time.fixedDeltaTime);
            // apply to transform
            transform.position = position;
            transform.eulerAngles = rotation;
            // step 4
            // test
            acceleration.x = -Mathf.Sin(Time.fixedTime);
            angularAcceleration.x = -Mathf.Sin(Time.fixedTime);
        }
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
        rotation += angularVelocity * dt;

        angularVelocity += angularAcceleration * dt;
    }

    private void UpdateRotationKinematics(float dt)
    {

    }


}
