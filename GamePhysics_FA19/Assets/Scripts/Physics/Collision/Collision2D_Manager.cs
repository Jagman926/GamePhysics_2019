﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision2D_Manager : MonoBehaviour
{
    // Collision Objects
    private CollisionHull2D[] collisionObjects;

    private CollisionHull2D.Collision collision;

    [SerializeField]
    private GameManager gameManager;

    public float restitution;

    void Start()
    {
        collisionObjects = FindObjectsOfType(typeof(CollisionHull2D)) as CollisionHull2D[];
        collision = new CollisionHull2D.Collision(); //Creates the collision class
        collision.restitution = restitution;
    }

    private void FixedUpdate()
    {
        collisionObjects = FindObjectsOfType(typeof(CollisionHull2D)) as CollisionHull2D[];
        UpdateObjectTransforms();
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
                CollisionHull2D thisHull = collisionObjects[i];
                CollisionHull2D otherHull = collisionObjects[j];
                // Check for collision
                if(thisHull.isColliding(otherHull, ref collision))
                {
                    //Checks if player is colliding
                    if ((thisHull.gameObject.tag == "Player" || otherHull.gameObject.tag == "Player"))
                    {
                        //END GAME
                        if(collision.status)
                            gameManager.EndGame();
                    }
                }

            }
    }
}
