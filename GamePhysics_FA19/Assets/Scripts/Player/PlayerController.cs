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

    [Header("Particle System")]
    // Particle Effects
    [SerializeField]
    private ParticleSystem psL;
    [SerializeField]
    private ParticleSystem psR;
    [SerializeField]
    private ParticleSystem psB;
    [SerializeField]
    private ParticleSystem psF;
    [SerializeField]
    private GameObject explosionPrefab;

    void Start()
    {
        // Particle2D
        p2D = gameObject.GetComponent<Particle2D>();
        // GameManager
        gm = gameManagerObject.GetComponent<GameManager>();
    }

    void Update()
    {
        gm.CheckEdgeOfView(ref p2D);
        UpdateMovement();
    }

    void OnDestroy()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    void UpdateMovement()
    {
        // Clockwise Rotation
        if (Input.GetKey(keycode_yaw_CW))
        {
            p2D.ApplyTorque(-forceRotation2D, rotationMomentArmRight);
            psR.Emit(1);
        }
        // Counter-Clockwise Rotation
        if (Input.GetKey(keycode_yaw_CCW))
        {
            p2D.ApplyTorque(forceRotation2D, rotationMomentArmRight);
            psL.Emit(1);
        }
        // Forward Movement
        if (Input.GetKey(keycode_forward))
        {
            p2D.AddForceForward(forceForward2D);
            psB.Emit(1);
        }
        if (Input.GetKey(keyCode_reverse))
        {
            p2D.AddForceForward(-forceReverse2D);
            psF.Emit(1);
        }
    }
}
