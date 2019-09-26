using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxCollisionHull2D : CollisionHull2D
{
    public ObjectBoundingBoxCollisionHull2D() : base(CollisionHullType2D.hull_obb) { }

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
        // see AABB

        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull2D other)
    {
        // same as AABB-OBB part 2, twice
        // 1. ......

        return false;
    }

    public override void OnDrawGizmosSelected()
    {
        // Draw debug square of OOB collision hull
        Gizmos.color = Color.green;
    }
}
