using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    [SerializeField]
    private PhysicsMethods positionPhysicsMethod = null;

    [SerializeField]
    private PhysicsMethods rotationPhysicsMethod = null;

    // Particle

    private Particle particle = null;

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
        particle = new Particle();
        InitializeParticle();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Update position and rotation
        positionPhysicsMethod.UpdatePosition(ref particle, Time.fixedDeltaTime);
        rotationPhysicsMethod.UpdateRotation(ref particle, Time.fixedDeltaTime);

        // apply to transform
        transform.position = position;
        transform.eulerAngles = rotation * 20.0f;
        // step 4
        // test
        acceleration.x = -Mathf.Sin(Time.fixedTime);
        angularAcceleration.z = -Mathf.Sin(Time.fixedTime);
    }

    private void InitializeParticle()
    {
        // Check Physics Methods
        if (positionPhysicsMethod == null)
            Debug.LogError("Position Physics Method Not Set for Particle2D!");
        if (rotationPhysicsMethod == null)
            Debug.LogError("Rotation Physics Method Not Set for Particle2D!");
        // Init Particle Data
        particle.InitParticle(position, velocity, acceleration, rotation, angularVelocity, angularAcceleration);
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
