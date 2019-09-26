using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxCollisionHull2D : CollisionHull2D
{
    public AxisAlignBoundingBoxCollisionHull2D() : base(CollisionHullType2D.hull_aabb) { }

    // Variables needed
    // 1. Min x/y
    // 2. Max x/y
    public Vector2 minExtent;
    public Vector2 maxExtent;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // see circle

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

        if(maxExtent.x > other.minExtent.x &&
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

    public override void OnDrawGizmosSelected()
    {
        // Draw debug square of AABB collision hull
        Gizmos.color = Color.green;
    }
}
