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

    // Edge of Map Variables
    private float edgeRight = 18.0f;
    private float edgeLeft = -18.0f;
    private float edgeTop = 10.3f;
    private float edgeBot = -10.3f;
    private float buffer = 0.1f;

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
        CheckEdgeOfView();
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

    void CheckEdgeOfView()
    {
        // If moves off right side of view, move to left side
        if (p2D.position.x > edgeRight + buffer)
            p2D.position = new Vector2(edgeLeft - buffer, transform.position.y);
        // If moves off left side of view, move to right side
        else if (p2D.position.x < edgeLeft - buffer)
            p2D.position = new Vector2(edgeRight + buffer, transform.position.y);
        // If moves off top of view, move to bottom side
        else if (p2D.position.y > edgeTop + buffer)
            p2D.position = new Vector2(transform.position.x, edgeBot - buffer);
        // If moves off bottom of view, move to top side
        else if (p2D.position.y < edgeBot - buffer)
            p2D.position = new Vector2(transform.position.x, edgeTop + buffer);
    }
}
