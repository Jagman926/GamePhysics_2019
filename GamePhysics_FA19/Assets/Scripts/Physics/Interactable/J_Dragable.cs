using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Dragable : MonoBehaviour
{
    /*
    References:
    Jayanam - Unity Tutorial: Drag Gameobject with Mouse
     */

    // Main Camera
    Camera mainCamera = null;

    // RigidBody
    Rigidbody rb = null;

    // Drag Coords
    RaycastHit hit;
    private Vector3 dragOffset;
    private float zCoord;

    void Awake()
    {
        mainCamera = Camera.main;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // Cast ray and get first gameObject hit
        Physics.Raycast(ray, out hit);
        Debug.Log("This hit at " + hit.point);
        //zCoord = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;
        //dragOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    /// <summary>
    /// OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.
    /// </summary>
    void OnMouseDrag()
    {
        //transform.position = GetMouseWorldPos() + dragOffset;
        // Determine force
        Vector3 pullForce;
        pullForce = (GetMouseWorldPos() - hit.point);
        rb.AddForce(pullForce.normalized * pullForce.magnitude);
    }

    Vector3 GetMouseWorldPos()
    {
        // pixel coords (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coords of game object on screen
        mousePoint.z = zCoord;

        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}
