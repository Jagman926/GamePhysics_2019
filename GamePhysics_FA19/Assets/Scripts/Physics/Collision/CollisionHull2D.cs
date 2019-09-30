using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
    public enum CollisionHullType2D
    {
        hull_circle,
        hull_aabb,
        hull_obb,

    }

    public CollisionHullType2D type { get; }

    protected CollisionHull2D(CollisionHullType2D type_set)
    {
        type = type_set;
    }

    protected Particle2D particle;

    protected CollisionHull2D[] collisionObjects;

    void Start()
    {
        particle = GetComponent<Particle2D>();
        collisionObjects = FindObjectsOfType(typeof(CollisionHull2D)) as CollisionHull2D[];
    }

    void Update()
    {
        // Updates transform variables
        UpdateTransform();

        foreach (CollisionHull2D other in collisionObjects)
        {
            // Prevents checking collision on itself
            if (other.gameObject != gameObject)
            {
                isColliding(other);
            }
        }
    }

    public abstract void UpdateTransform();

    public abstract bool isColliding(CollisionHull2D other);

    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other);

    public abstract bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull2D other);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull2D other);
}
