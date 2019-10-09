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

    // Game Manager
    public GameObject gameManagerObject;
    private GameManager gm;

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
        // Particle2D
        p2D = gameObject.GetComponent<Particle2D>();
        // GameManager
        gm = gameManagerObject.GetComponent<GameManager>();
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

//  TO-DO: Move this to GameManager and let it take in the particle2D by reference to adjust for asteroids too!
    void CheckEdgeOfView()
    {
        // If moves off right side of view, move to left side
        if (p2D.position.x > gm.edgeRight + gm.buffer)
            p2D.position = new Vector2(gm.edgeLeft - gm.buffer, transform.position.y);
        // If moves off left side of view, move to right side
        else if (p2D.position.x < gm.edgeLeft - gm.buffer)
            p2D.position = new Vector2(gm.edgeRight + gm.buffer, transform.position.y);
        // If moves off top of view, move to bottom side
        else if (p2D.position.y > gm.edgeTop + gm.buffer)
            p2D.position = new Vector2(transform.position.x, gm.edgeBot - gm.buffer);
        // If moves off bottom of view, move to top side
        else if (p2D.position.y < gm.edgeBot - gm.buffer)
            p2D.position = new Vector2(transform.position.x, gm.edgeTop + gm.buffer);
    }
}
