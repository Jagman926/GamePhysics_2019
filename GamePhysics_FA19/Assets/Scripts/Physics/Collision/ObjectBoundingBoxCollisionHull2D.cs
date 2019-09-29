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
    public Vector2 halfExtents = Vector2.zero;
    public Vector2 minExtent = Vector2.zero;
    public Vector2 maxExtent = Vector2.zero;

    public override void UpdateTransform()
    {
        center = particle.position;
        Matrix4x4 objectToWorldMatrix = transform.localToWorldMatrix;
        Quaternion storedRotation = transform.rotation;

        // Determine extents
        halfExtents = new Vector2(0.5f * particle.width, 0.5f * particle.height);
        minExtent = new Vector2(-halfExtents.x, -halfExtents.y);
        maxExtent = new Vector2(halfExtents.x, halfExtents.y);

        // Set rotation to identity
        transform.rotation = Quaternion.identity;

        // Transform points by world matrix
        minExtent = objectToWorldMatrix.MultiplyPoint3x4(minExtent);
        maxExtent = objectToWorldMatrix.MultiplyPoint3x4(maxExtent);

        // Reset rotation
        transform.rotation = storedRotation;
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(minExtent, .04f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(maxExtent, .04f);
    }
}
