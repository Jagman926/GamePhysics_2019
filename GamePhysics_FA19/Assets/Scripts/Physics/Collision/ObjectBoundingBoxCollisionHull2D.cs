using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxCollisionHull2D : CollisionHull2D
{
    public ObjectBoundingBoxCollisionHull2D() : base(CollisionHullType2D.hull_obb) { }

    // Variables needed
    // 1. Min x/y
    // 2. Max x/y
    public Vector2 center = Vector2.zero;
    public Vector2 minExtent = Vector2.zero;
    public Vector2 maxExtent = Vector2.zero;

    public override void UpdateTransform()
    {
        center = particle.position;

        float xExtent = 0.5f * particle.width;
        float yExtent = 0.5f * particle.height;

        float xRotatedExtent = (xExtent * Mathf.Cos(particle.rotation.z) - (yExtent * Mathf.Sin(particle.rotation.z)));
        float yRotatedExtent = (xExtent * Mathf.Sin(particle.rotation.z) + (yExtent * Mathf.Cos(particle.rotation.z)));

        minExtent = new Vector2(center.x + xRotatedExtent, center.y - yRotatedExtent);
        maxExtent = new Vector2(center.x - xRotatedExtent, center.y + yRotatedExtent);
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
}
