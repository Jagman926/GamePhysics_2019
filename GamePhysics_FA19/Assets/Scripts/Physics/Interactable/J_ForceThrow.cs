using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_ForceThrow : MonoBehaviour
{
    [SerializeField]
    Vector3 throwForce;
    [SerializeField]
    Particle3D ball;
    [SerializeField]
    private float forcePerTick = 0f;

    bool thrown = false;

    SpawnBall spawnBall = null;

    private float forceMultiplyer = 0f;

    public Transform followTransform = null;


    // Start is called before the first frame update
    void Start()
    {
        ball.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            forceMultiplyer += forcePerTick;
        }
        else if(Input.GetKeyUp(KeyCode.Space) && !thrown)
        {

            ball.isKinematic = true;
            ball.AddForce(throwForce * forceMultiplyer);
            forceMultiplyer = 0;
            thrown = true;
        }

        if (!thrown)
        {
            transform.position = followTransform.position;
        }
    }
}
