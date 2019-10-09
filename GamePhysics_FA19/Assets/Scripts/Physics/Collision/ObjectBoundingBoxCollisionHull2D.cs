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

    public override bool isColliding(CollisionHull2D other, ref Collision c)
    {
        switch (other.type)
        {
            // If other object is a circle hull
            case CollisionHullType2D.hull_circle:
                if (TestCollisionVsCircle((CircleCollisionHull2D)other, ref c))
                {
                    Debug.Log(gameObject.name + " Colliding with " + other.name);
                    PopulateCollisionClassOBBVSCircle((CircleCollisionHull2D)other, this, ref c);
                    //Resolves the collisions
                    ResolveCollisions(ref c);
                    clearContacts(ref c); //Clears the information used after contacts have been resolved
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

        if (other.TestCollisionVsOBB(this, ref c))
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull2D other, ref Collision c)
    {
        // See AxisAlignBoundingBoxCollisionHull2D

        if (other.TestCollisionVsOBB(this, ref c))
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull2D other, ref Collision c)
    {
        // same as AABB-OBB part 2, twice
        // 1. Get OBB1 max/min extents from world matrix inv of OBB2
        // 2. Get OBB2 max/min extents from world matrix inv of OBB1
        // 3. For both test, and if both true, pass

        // Other object multiplied by inverse world matrix
        Vector2 obb1_maxExtent_transInv = other.transform.worldToLocalMatrix.MultiplyPoint(other.maxExtent);
        Vector2 obb1_minExtent_transInv = other.transform.worldToLocalMatrix.MultiplyPoint(other.minExtent);
        // This object multiplied by inverse world matrix
        Vector2 obb2_maxExtent_transInv = transform.worldToLocalMatrix.MultiplyPoint(maxExtent);
        Vector2 obb2_minExtent_transInv = transform.worldToLocalMatrix.MultiplyPoint(minExtent);

        obb1_maxExtent_transInv += center;
        obb1_minExtent_transInv += center;
        obb2_maxExtent_transInv += other.center;
        obb2_minExtent_transInv += other.center;

        if (obb1_maxExtent_transInv.x > minExtent.x &&
            obb1_minExtent_transInv.x < maxExtent.x &&
            obb1_maxExtent_transInv.y > minExtent.y &&
            obb1_minExtent_transInv.y < maxExtent.y)
        {
            if (obb2_maxExtent_transInv.x > other.minExtent.x &&
                obb2_minExtent_transInv.x < other.maxExtent.x &&
                obb2_maxExtent_transInv.y > other.minExtent.y &&
                obb2_minExtent_transInv.y < other.maxExtent.y)
                return true;
        }

        return false;
    }
}
