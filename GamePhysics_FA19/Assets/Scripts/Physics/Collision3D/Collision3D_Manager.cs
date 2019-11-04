using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision3D_Manager : MonoBehaviour
{
    // Collision Objects
    private CollisionHull3D[] collisionObjects;

    private CollisionHull3D.Collision collision;

    [SerializeField]
    private GameManager gameManager;

    public float restitution;

    void Start()
    {
        collisionObjects = FindObjectsOfType(typeof(CollisionHull3D)) as CollisionHull3D[];
        collision = new CollisionHull3D.Collision(); //Creates the collision class
        collision.restitution = restitution;
    }

    private void FixedUpdate()
    {
        collisionObjects = FindObjectsOfType(typeof(CollisionHull3D)) as CollisionHull3D[];
        //UpdateObjectTransforms();
        CheckObjectCollisions();
    }

    void UpdateObjectTransforms()
    {
        int i = 0;
        for (i = 0; i < collisionObjects.Length; i++)
        {
            collisionObjects[i].UpdateTransform();
        }
    }

    void CheckObjectCollisions()
    {
        int i = 0, j = 0;
        for (i = 0; i < collisionObjects.Length - 1; i++)
            for (j = i + 1; j < collisionObjects.Length; j++)
            {
                // Declare hull being used
                CollisionHull3D thisHull = collisionObjects[i];
                CollisionHull3D otherHull = collisionObjects[j];
                // Check for collision
                thisHull.isColliding(otherHull, ref collision);

            }
    }
}
