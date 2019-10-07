using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxCollisionHull2D : CollisionHull2D
{
    public AxisAlignBoundingBoxCollisionHull2D() : base(CollisionHullType2D.hull_aabb) { }

    // Variables needed
    // 1. Min x/y
    // 2. Max x/y
    // 3. Center
    public Vector2 center;
    public Vector2 minExtent;
    public Vector2 maxExtent;

    [Header("Debug Material")]
    bool colliding;
    private Renderer renderer;
    public Material mat_red;
    public Material mat_green;

    void Awake()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }

    public override void UpdateTransform()
    {
        center = particle.position;

        Vector2 halfExtents = new Vector2(0.5f * particle.width, 0.5f * particle.height);

        minExtent = new Vector2(center.x - halfExtents.x, center.y - halfExtents.y);
        maxExtent = new Vector2(center.x + halfExtents.x, center.y + halfExtents.y);
    }

    public override bool isColliding(CollisionHull2D other, ref Collision c)
    {
        switch (other.type)
        {
            // If other object is a circle hull
            case CollisionHullType2D.hull_circle:
                if (TestCollisionVsCircle((CircleCollisionHull2D)other, ref c))
                {
                    Debug.Log(gameObject.name + " Colliding with " + other.name);
                    colliding = true;
                }
                break;
            // If other object is a aabb hull
            case CollisionHullType2D.hull_aabb:
                if (TestCollisionVsAABB((AxisAlignBoundingBoxCollisionHull2D)other, ref c))
                {
                    Debug.Log(gameObject.name + " Colliding with " + other.name);
                    colliding = true;
                }
                break;
            // If other object is a obb hull
            case CollisionHullType2D.hull_obb:
                if (TestCollisionVsOBB((ObjectBoundingBoxCollisionHull2D)other, ref c))
                {
                    Debug.Log(gameObject.name + " Colliding with " + other.name);
                    colliding = true;
                }
                break;
            default:
                break;
        }
        if (colliding)
            renderer.material = mat_red;
        else
            renderer.material = mat_green;

        return colliding;
    }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c)
    {
        // See CircleCollisionHull2D
        
        if (other.TestCollisionVsAABB(this, ref c))
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull2D other, ref Collision c)
    {
        /*

                               ---------------- <maxB
                              |                |
                              |                |
                     ---------+------ <maxA    |
                    |         |      |         |
                    |    minB> ------+---------
                    |                |
                    |                |
               minA> ----------------

         */

        // pass if, for all axes, max extent of A is greater than min extent of B
        // 1. maxA.x > minB.x && minA.x < maxB.x && maxA.y > minB.y && minA.y < maxB.y
        // 2. pass if all cases are true

        if (maxExtent.x > other.minExtent.x &&
           minExtent.x < other.maxExtent.x &&
           maxExtent.y > other.minExtent.y &&
           minExtent.y < other.maxExtent.y)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull2D other, ref Collision c)
    {
        // same as above twice...
        //  first, find max extents of OBB, do AABB vs this box
        //  then, transform this box into OBB's space, find the max extents, repeat
        // 1. Get OBB max/min extents in rotation (done in object script)
        // 2. Get AABB max/min extents from world matrix inv of OBB
        // 3. use same AABB vs AABB test for both scenarios using the others normal max/min extents

        Vector2 aabb_maxExtent_transInv = transform.localToWorldMatrix.inverse.MultiplyPoint3x4(maxExtent);
        Vector2 aabb_minExtent_transInv = transform.localToWorldMatrix.inverse.MultiplyPoint3x4(minExtent);

        aabb_maxExtent_transInv += other.center;
        aabb_minExtent_transInv += other.center;

        if (maxExtent.x > other.minExtent_Rotated.x &&
            minExtent.x < other.maxExtent_Rotated.x &&
            maxExtent.y > other.minExtent_Rotated.y &&
            minExtent.y < other.maxExtent_Rotated.y)
        {
            if (aabb_maxExtent_transInv.x > other.minExtent.x &&
                aabb_minExtent_transInv.x < other.maxExtent.x &&
                aabb_maxExtent_transInv.y > other.minExtent.y &&
                aabb_minExtent_transInv.y < other.maxExtent.y)
                return true;
        }
        return false;
    }
}
