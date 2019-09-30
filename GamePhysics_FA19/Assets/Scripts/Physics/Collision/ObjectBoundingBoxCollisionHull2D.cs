using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxCollisionHull2D : CollisionHull2D
{
    public ObjectBoundingBoxCollisionHull2D() : base(CollisionHullType2D.hull_obb) { }

    // Variables needed
    // 1. Half extents
    // 2. Min x/y
    // 3. Max x/y
    // 4. Center
    public Vector2 center;
    public Vector2 halfExtents;
    public Vector2 minExtent;
    public Vector2 maxExtent;
    public Vector2 minExtent_Rotated;
    public Vector2 maxExtent_Rotated;

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
        Matrix4x4 objectToWorldMatrix = transform.localToWorldMatrix;
        Quaternion storedRotation = transform.rotation;

        // Determine extents
        halfExtents = new Vector2(0.5f * particle.width, 0.5f * particle.height);
        minExtent = new Vector2(center.x - halfExtents.x, center.y - halfExtents.y);
        maxExtent = new Vector2(center.x + halfExtents.x, center.x + halfExtents.y);

        // Set rotation to identity
        transform.rotation = Quaternion.identity;
        // Transform points by world matrix
        Vector2 corner1 = objectToWorldMatrix.MultiplyPoint3x4(new Vector2(halfExtents.x, halfExtents.y));
        Vector2 corner2 = objectToWorldMatrix.MultiplyPoint3x4(new Vector2(halfExtents.x, -halfExtents.y));
        Vector2 corner3 = objectToWorldMatrix.MultiplyPoint3x4(new Vector2(-halfExtents.x, halfExtents.y));
        Vector2 corner4 = objectToWorldMatrix.MultiplyPoint3x4(new Vector2(-halfExtents.x, -halfExtents.y));
        // Calculate min/max rotated points
        minExtent_Rotated.x = Mathf.Min(corner1.x, Mathf.Min(corner2.x, Mathf.Min(corner3.x, corner4.x)));
        minExtent_Rotated.y = Mathf.Min(corner1.y, Mathf.Min(corner2.y, Mathf.Min(corner3.y, corner4.y)));
        maxExtent_Rotated.x = Mathf.Max(corner1.x, Mathf.Max(corner2.x, Mathf.Max(corner3.x, corner4.x)));
        maxExtent_Rotated.y = Mathf.Max(corner1.y, Mathf.Max(corner2.y, Mathf.Max(corner3.y, corner4.y)));

        // Reset rotation
        transform.rotation = storedRotation;
    }

    public override bool isColliding(CollisionHull2D other)
    {
        switch (other.type)
        {
            // If other object is a circle hull
            case CollisionHullType2D.hull_circle:
                if (TestCollisionVsCircle((CircleCollisionHull2D)other))
                {
                    Debug.Log(gameObject.name + " Colliding with " + other.name);
                    colliding = true;
                }
                break;
            // If other object is a aabb hull
            case CollisionHullType2D.hull_aabb:
                if (TestCollisionVsAABB((AxisAlignBoundingBoxCollisionHull2D)other))
                {
                    Debug.Log(gameObject.name + " Colliding with " + other.name);
                    colliding = true;
                }
                break;
            // If other object is a obb hull
            case CollisionHullType2D.hull_obb:
                if (TestCollisionVsOBB((ObjectBoundingBoxCollisionHull2D)other))
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

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // same as above, but first...
        // multiply circle center by box world matrix inverse
        // 1. Get world matrix of OBB
        // 2. Multiply inverse of matrix by center point of circle
        // 3. Same as collision vs AABB

        Vector2 circleCenter = transform.InverseTransformPoint(other.center);

        circleCenter += center;

        float clampX = Mathf.Clamp(circleCenter.x, minExtent.x, maxExtent.x);
        float clampY = Mathf.Clamp(circleCenter.y, minExtent.y, maxExtent.y);

        Vector2 closestPoint = new Vector2(clampX, clampY);

        if ((closestPoint - circleCenter).sqrMagnitude < other.radius * other.radius)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull2D other)
    {
        // same as above twice...
        //  first, find max extents of OBB, do AABB vs this box
        //  then, transform this box into OBB's space, find the max extents, repeat
        // 1. Get OBB max/min extents in rotation (done in object script)
        // 2. Get AABB max/min extents from world matrix inv of OBB
        // 3. use same AABB vs AABB test for both scenarios using the others normal max/min extents

        Vector2 aabb_maxExtent_transInv = other.transform.localToWorldMatrix.inverse.MultiplyPoint3x4(other.maxExtent);
        Vector2 aabb_minExtent_transInv = other.transform.localToWorldMatrix.inverse.MultiplyPoint3x4(other.minExtent);

        aabb_maxExtent_transInv += center;
        aabb_minExtent_transInv += center;

        if (other.maxExtent.x > minExtent_Rotated.x &&
            other.minExtent.x < maxExtent_Rotated.x &&
            other.maxExtent.y > minExtent_Rotated.y &&
            other.minExtent.y < maxExtent_Rotated.y)
        {
            if (aabb_maxExtent_transInv.x > minExtent.x &&
                aabb_minExtent_transInv.x < maxExtent.x &&
                aabb_maxExtent_transInv.y > minExtent.y &&
                aabb_minExtent_transInv.y < maxExtent.y)
                return true;
        }
        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull2D other)
    {
        // same as AABB-OBB part 2, twice
        // 1. ......

        return false;
    }
}
