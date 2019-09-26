using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxCollisionHull2D : CollisionHull2D
{
    public AxisAlignBoundingBoxCollisionHull2D() : base(CollisionHullType2D.hull_aabb) { }

    // Variables needed
    // 1. Min x/y
    // 2. Max x/y

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
        // pass if, for all axes, max extent of A is greater than min extent of B
        // 1. ......

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
