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
    private Vector3 centerOfMassLocal = Vector3.zero,
                    centerOfMassWorld = Vector3.zero; //Multiply local by 


    [Header("Force Variables")]
    [SerializeField]
    //private Matrix4x4 inertia = Matrix4x4.zero, inertiaInv = Matrix4x4.zero;
    //Unity does not support 3x3 matrix so we can either make our own or use a 4x4 matrix
    private static float[,] inertia = new float[3,3];
    private static float[,] inertiaInv = new float[3, 3];
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
    private static Matrix4x4 transformationMatrix;
    private static Matrix4x4 transformationMatrixInv;
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

    [Header("Test Variables")]
    [SerializeField]
    Vector3 testForce;
    [SerializeField]
    GameObject momentArmObject;
    [SerializeField]
    Vector3 momentArm;

    void Start()
    {
        InitStartingVariables();
    }

    void FixedUpdate()
    {
        
        //Updates the moment arm
        //UpdateMomentArm();
        //Applying torque from testforce
        //ApplyTorque(testForce, momentArm);
        PlayerTestForceControl(); //Player controlled torque for testing

        // Update position and rotation
        //J_Physics.UpdatePosition3DKinematic(ref position, ref velocity, ref acceleration, Time.fixedDeltaTime);
        //J_Physics.UpdateRotation3D(ref rotation, ref angularVelocity, angularAcceleration, Time.fixedDeltaTime);
        UpdateTransformationMatrix();

        // apply to transform
        transform.position = position;
        //transform.eulerAngles = rotation;
        transform.rotation = rotation.ToUnityQuaterntion();
        // Update acceleration | angular acceleration
        J_Physics.UpdateAcceleration3D(ref acceleration, massInv, ref force);
        J_Physics.UpdateAngularAcceleration3D(ref angularAcceleration, ref torque, inertiaInv,  transformationMatrix);

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

    void PlayerTestForceControl()
    {
        if (Input.GetKey(KeyCode.W))
            ApplyTorque(new Vector3(0,1,0), momentArm);
        if (Input.GetKey(KeyCode.S))
            ApplyTorque(new Vector3(0, -1, 0), momentArm);
        if (Input.GetKey(KeyCode.A))
            ApplyTorque(new Vector3(-1, 0, 0), momentArm);
        if (Input.GetKey(KeyCode.D))
            ApplyTorque(new Vector3(1, 0, 0), momentArm);
        if (Input.GetKey(KeyCode.Q))
            ApplyTorque(new Vector3(0, 0, -1), momentArm);
        if (Input.GetKey(KeyCode.E))
            ApplyTorque(new Vector3(0, 0, 1), momentArm);

    }

    public void UpdateTransformationMatrix()
    {
        float[,] quatMatrix = rotation.GetToRotationMatrix();

        //rotation into transformation matrix
        transformationMatrix[0, 0] = quatMatrix[0,0];
        transformationMatrix[0, 1] = quatMatrix[0,1];
        transformationMatrix[0, 2] = quatMatrix[0,2];

        transformationMatrix[1, 0] = quatMatrix[1, 0];
        transformationMatrix[1, 1] = quatMatrix[1, 1];
        transformationMatrix[1, 2] = quatMatrix[1, 2];

        transformationMatrix[2, 0] = quatMatrix[2, 0];
        transformationMatrix[2, 1] = quatMatrix[2, 1];
        transformationMatrix[2, 2] = quatMatrix[2, 2];

        //Set transform into transformation matrix
        transformationMatrix[0, 3] = position.x;
        transformationMatrix[1, 3] = position.y;
        transformationMatrix[2, 3] = position.z;

        //Set last idenity row
        transformationMatrix[3, 0] = 0;
        transformationMatrix[3, 1] = 0;
        transformationMatrix[3, 2] = 0;
        transformationMatrix[3, 3] = 1;
    }

    void UpdateMomentArm()
    {
        //Updates momemnt arm position
        momentArm = momentArmObject.transform.position;
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

        //Zero's out the inertia
        inertia[0, 0] = 0;
        inertia[0, 1] = 0;
        inertia[0, 2] = 0;
        inertia[1, 0] = 0;
        inertia[1, 1] = 0;
        inertia[1, 2] = 0;
        inertia[2, 0] = 0;
        inertia[2, 1] = 0;
        inertia[2, 2] = 0;


        switch (shapeType)
        {
            // Solid Sphere
            case Shape.SolidSphere:
                inertia[0,0] = (2f / 5f) * mass * (radiusOuter * radiusOuter);
                inertia[1,1] = (2f / 5f) * mass * (radiusOuter * radiusOuter);
                inertia[2,2] = (2f / 5f) * mass * (radiusOuter * radiusOuter);
                break;
            // Hollow Sphere
            case Shape.HollowSphere:
                inertia[0,0] = (2f / 3f) * mass * (radiusOuter * radiusOuter);
                inertia[1,1] = (2f / 3f) * mass * (radiusOuter * radiusOuter);
                inertia[2,2] = (2f / 3f) * mass * (radiusOuter * radiusOuter);
                break;
            // Solid Box
            case Shape.SolidBox:
                float temp = (1f / 12f) * mass * ((height * height) + (length * length));
                inertia[0,0] = (1f / 12f) * mass * ((height * height) + (length * length));
                inertia[1,1] = (1f / 12f) * mass * ((length * length) + (width * width));
                inertia[2,2] = (1f / 12f) * mass * ((height * height) + (width * width));
                break;
            // Hollow Box
            case Shape.HollowBox:
                inertia[0,0] = (5f / 3f) * mass * ((height * height) + (length * length));
                inertia[1,1] = (5f / 3f) * mass * ((length * length) + (width * width));
                inertia[2,2] = (5f / 3f) * mass * ((height * height) + (width * width));
                break;
            // Solid Cylinder
            case Shape.SolidCylinder:
                inertia[0,0] = (1f / 12f) * mass * (3*(radiusOuter * radiusOuter) + (height * height));
                inertia[1,1] = (1f / 12f) * mass * (3*(radiusOuter * radiusOuter) + (height * height));
                inertia[2,2] = (1f / 2f) * mass * (radiusOuter * radiusOuter);
                break;
            //Solid Cone
            case Shape.SolidCone:
                inertia[0,0] = (3f / 5f) * mass * (height * height) + (3/20) * mass * (radiusOuter * radiusOuter);
                inertia[1,1] = (3f / 5f) * mass * (height * height) + (3/20) * mass * (radiusOuter * radiusOuter);
                inertia[2,2] = (3f / 10f) * mass * (radiusOuter * radiusOuter);
                break;
            // Default Case
            default:
                Debug.Log("Shape Type not set");
                break;
        }

        //Calcualte inverse inertia
        inertiaInv = J_Physics.GetMat3Inverse(inertia); 

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

    public void ApplyTorque(Vector3 force, Vector3 momentArm)
    {
        //Torque in 3D
        // momentarm = pos - center of mass
        // torque += moment arm * force
        //torque += (force * (momentArm - centerOfMassLocal).magnitude);
        
        torque += force * (momentArm - centerOfMassLocal).magnitude;


    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public void SetPosition(Vector3 newPosition)
    {
        position = newPosition;
    }
}
