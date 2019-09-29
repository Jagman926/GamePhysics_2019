using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxCollisionHull2D : CollisionHull2D
{
    public AxisAlignBoundingBoxCollisionHull2D() : base(CollisionHullType2D.hull_aabb) { }

    // Variables needed
    // 1. Min x/y
    // 2. Max x/y
    public Vector2 center = Vector2.zero;
    public Vector2 halfExtents = Vector2.zero;
    public Vector2 minExtent = Vector2.zero;
    public Vector2 maxExtent = Vector2.zero;

    public override void UpdateTransform()
    {
        center = particle.position;

        halfExtents = new Vector2(0.5f * particle.width, 0.5f * particle.height);

        minExtent = new Vector2(center.x - halfExtents.x, center.y - halfExtents.y);
        maxExtent = new Vector2(center.x + halfExtents.x, center.y + halfExtents.y);
    }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // see circle
        // find closest point to the circle on the box
        // (done by clamping center of circle to be within box dimensions)
        // if closest point is within circle, pass (do point vs circle test)

        Vector2 difference = other.center - center;

        float clampX = Mathf.Clamp(difference.x, -halfExtents.x, halfExtents.x);
        float clampY = Mathf.Clamp(difference.y, -halfExtents.y, halfExtents.y);

        Vector2 closestPoint = new Vector2(center.x + clampX, center.y + clampY);

        if ((closestPoint - other.center).magnitude < other.radius)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull2D other)
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

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull2D other)
    {
        // same as above twice...
        //  first, find max extents of OBB, do AABB vs this box
        //  then, transform this box into OBB's space, find the max extents, repeat
        // 1. ......

        return false;
    }
}
