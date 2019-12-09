using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    [SerializeField]
    GameObject objectToDestroy;
    [SerializeField]
    string tagForWin = "Goal";
    CircleCollisionHull3D ccHull;

    // Update is called once per frame
    void Update()
    {
 
    }
}
