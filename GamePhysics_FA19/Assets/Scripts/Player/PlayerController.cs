using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Input Keys
    private KeyCode keycode_yaw_CW = KeyCode.RightArrow;
    private KeyCode keycode_yaw_CCW = KeyCode.LeftArrow;
    private KeyCode keycode_forward = KeyCode.UpArrow;
    private KeyCode keyCode_reverse = KeyCode.DownArrow;

    // Particle2D
    private Particle2D p2D;

    // Movement Variables
    [Header("Movement")]
    [SerializeField]
    private float forceRotation2D;
    [SerializeField]
    private float forceForward2D;
    [SerializeField]
    private float forceReverse2D;

    // Rotation Moment Arms
    [Header("Rotation Moment Arms")]
    [SerializeField]
    private Vector2 rotationMomentArmRight;
    [SerializeField]
    private Vector2 rotationMomentArmLeft;

    void Start()
    {
        p2D = gameObject.GetComponent<Particle2D>();
    }

    void Update()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        // Clockwise Rotation
        if (Input.GetKey(keycode_yaw_CW))
        {
            p2D.ApplyTorque(-forceRotation2D, rotationMomentArmRight);
        }
        // Counter-Clockwise Rotation
        if (Input.GetKey(keycode_yaw_CCW))
        {
            p2D.ApplyTorque(forceRotation2D, rotationMomentArmRight);
        }
        // Forward Movement
        if (Input.GetKey(keycode_forward))
        {
            p2D.AddForceForward(forceForward2D);
        }
        if (Input.GetKey(keyCode_reverse))
        {
            p2D.AddForceForward(-forceReverse2D);
        }
    }
}
