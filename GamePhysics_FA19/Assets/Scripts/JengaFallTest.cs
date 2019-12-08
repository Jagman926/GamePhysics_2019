using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengaFallTest : MonoBehaviour
{
    [SerializeField]
    JengaManager jengaManager;

    [SerializeField]
    float FallPositionMin = 0.0f;

    void Update()
    {
        if (transform.position.y < FallPositionMin)
        {
            jengaManager.EndGame();
            Debug.Log("End");
        }
    }
}
