using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorOnStart : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV(0.15f, 0.16f, .1f, .2f, 0.7f, 1f);
    }
}
