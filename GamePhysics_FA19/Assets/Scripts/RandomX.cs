using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomX : MonoBehaviour
{
    float min = 27;
    float max = 108;

    public void randomX()
    {
        transform.position = new Vector3(Random.Range(min, max), transform.position.y, transform.position.z);
    }
}
