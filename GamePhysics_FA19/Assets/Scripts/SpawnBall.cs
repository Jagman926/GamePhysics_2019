using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    [SerializeField]
    GameObject ballPrefab;

    public void CreateBall()
    {
        GameObject newBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        newBall.GetComponent<J_ForceThrow>().followTransform = this.transform;
    }
}
