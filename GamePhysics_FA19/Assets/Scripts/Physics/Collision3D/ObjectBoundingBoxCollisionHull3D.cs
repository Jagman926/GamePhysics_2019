using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxCollisionHull3D : CollisionHull3D
{
    public ObjectBoundingBoxCollisionHull3D() : base(CollisionHullType3D.hull_obb) { }

    // Variables needed
    // 1. Half extents
    // 2. Min x/y
    // 3. Max x/y
    // 4. Center
    public Vector3 center;
    public Vector3 halfExtents;
    public Vector3 minExtent;
    public Vector3 maxExtent;
    public Vector3 minExtent_Rotated;
    public Vector3 maxExtent_Rotated;

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
        halfExtents = new Vector3(0.5f * particle.width, 0.5f * particle.height, 0.5f * particle.length);
        minExtent = new Vector3(center.x - halfExtents.x, center.y - halfExtents.y, center.z - halfExtents.z);
        maxExtent = new Vector3(center.x + halfExtents.x, center.x + halfExtents.y, center.z + halfExtents.z);

        // Set rotation to identity
        transform.rotation = Quaternion.identity;
        // Transform points by world matrix
        Vector3 corner1 = objectToWorldMatrix.MultiplyPoint3x4(new Vector3(halfExtents.x, halfExtents.y, halfExtents.z));
        Vector3 corner2 = objectToWorldMatrix.MultiplyPoint3x4(new Vector3(halfExtents.x, -halfExtents.y, halfExtents.z));
        Vector3 corner3 = objectToWorldMatrix.MultiplyPoint3x4(new Vector3(-halfExtents.x, halfExtents.y, halfExtents.z));
        Vector3 corner4 = objectToWorldMatrix.MultiplyPoint3x4(new Vector3(-halfExtents.x, -halfExtents.y, halfExtents.z));
        Vector3 corner5 = objectToWorldMatrix.MultiplyPoint3x4(new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z));
        Vector3 corner6 = objectToWorldMatrix.MultiplyPoint3x4(new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z));
        Vector3 corner7 = objectToWorldMatrix.MultiplyPoint3x4(new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z));
        Vector3 corner8 = objectToWorldMatrix.MultiplyPoint3x4(new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z));
        // Calculate min/max rotated points 
        minExtent_Rotated.x = Mathf.Min(corner1.x, Mathf.Min(corner2.x, Mathf.Min(corner3.x, Mathf.Min(corner4.x, Mathf.Min(corner5.x, Mathf.Min(corner6.x, Mathf.Min(corner7.x, corner8.x))))))); 
        minExtent_Rotated.y = Mathf.Min(corner1.y, Mathf.Min(corner2.y, Mathf.Min(corner3.y, Mathf.Min(corner4.y, Mathf.Min(corner5.y, Mathf.Min(corner6.y, Mathf.Min(corner7.y, corner8.y))))))); 
        minExtent_Rotated.z = Mathf.Min(corner1.z, Mathf.Min(corner2.z, Mathf.Min(corner3.z, Mathf.Min(corner4.z, Mathf.Min(corner5.z, Mathf.Min(corner6.z, Mathf.Min(corner7.z, corner8.z))))))); 
        maxExtent_Rotated.x = Mathf.Max(corner1.x, Mathf.Max(corner2.x, Mathf.Max(corner3.x, Mathf.Max(corner4.x, Mathf.Max(corner5.x, Mathf.Max(corner6.x, Mathf.Max(corner7.x, corner8.x))))))); 
        maxExtent_Rotated.y = Mathf.Max(corner1.y, Mathf.Max(corner2.y, Mathf.Max(corner3.y, Mathf.Max(corner4.y, Mathf.Max(corner5.y, Mathf.Max(corner6.y, Mathf.Max(corner7.y, corner8.y))))))); 
        maxExtent_Rotated.z = Mathf.Max(corner1.z, Mathf.Max(corner2.z, Mathf.Max(corner3.z, Mathf.Max(corner4.z, Mathf.Max(corner5.z, Mathf.Max(corner6.z, Mathf.Max(corner7.z, corner8.z))))))); 

        // Reset rotation
        transform.rotation = storedRotation;
    }

    public override bool isColliding(CollisionHull3D other, ref Collision c)
    {
        switch (other.type)
        {
            // If other object is a circle hull
            case CollisionHullType3D.hull_circle:
                if (TestCollisionVsCircle((CircleCollisionHull3D)other, ref c))
                {
                    if (c.status)
                    {
                        c.status = false;
                    }
                    else
                    {
                        //PopulateCollisionClassOBBVSCircle((CircleCollisionHull3D)other, this, ref c);
                        ////Resolves the collisions
                        //ResolveCollisions(ref c);
                        //clearContacts(ref c); //Clears the information used after contacts have been resolved
                        c.status = true;
                    }
                    Debug.Log(gameObject.name + " Colliding with " + other.name);

                    colliding = true;
                }
                else
                    colliding = false;
                break;
            // If other object is a aabb hull
            case CollisionHullType3D.hull_aabb:
                if (TestCollisionVsAABB((AxisAlignBoundingBoxCollisionHull3D)other, ref c))
                {
                    Debug.Log(gameObject.name + " Colliding with " + other.name);
                    colliding = true;
                }
                else
                    colliding = false;
                break;
            // If other object is a obb hull
            case CollisionHullType3D.hull_obb:
                if (TestCollisionVsOBB((ObjectBoundingBoxCollisionHull3D)other, ref c))
                {
                    Debug.Log(gameObject.name + " Colliding with " + other.name);

                    colliding = true;
                }
                else
                    colliding = false;
                break;
            default:
                break;
        }

        return colliding;
    }

    public override bool TestCollisionVsCircle(CircleCollisionHull3D other, ref Collision c)
    {
        // See CircleCollisionHull3D

        if (other.TestCollisionVsOBB(this, ref c))
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull3D other, ref Collision c)
    {
        // See AxisAlignBoundingBoxCollisionHull3D

        if (other.TestCollisionVsOBB(this, ref c))
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull3D other, ref Collision c)
    {
        // same as AABB-OBB part 2, twice
        // 1. Get OBB1 max/min extents from world matrix inv of OBB2
        // 2. Get OBB2 max/min extents from world matrix inv of OBB1
        // 3. For both test, and if both true, pass

        // Other object multiplied by inverse world matrix
        Vector3 obb1_maxExtent_transInv = other.transform.worldToLocalMatrix.MultiplyPoint(other.maxExtent);
        Vector3 obb1_minExtent_transInv = other.transform.worldToLocalMatrix.MultiplyPoint(other.minExtent);
        // This object multiplied by inverse world matrix
        Vector3 obb2_maxExtent_transInv = transform.worldToLocalMatrix.MultiplyPoint(maxExtent);
        Vector3 obb2_minExtent_transInv = transform.worldToLocalMatrix.MultiplyPoint(minExtent);

       //obb1_maxExtent_transInv += center;
       //obb1_minExtent_transInv += center;
       //obb2_maxExtent_transInv += other.center;
       //obb2_minExtent_transInv += other.center;

        if (obb1_maxExtent_transInv.x > minExtent.x &&
            obb1_minExtent_transInv.x < maxExtent.x &&
            obb1_maxExtent_transInv.y > minExtent.y &&
            obb1_minExtent_transInv.y < maxExtent.y &&
            obb1_maxExtent_transInv.z > minExtent.z &&
            obb1_minExtent_transInv.z < maxExtent.z)
        {
            if (obb2_maxExtent_transInv.x > other.minExtent.x &&
                obb2_minExtent_transInv.x < other.maxExtent.x &&
                obb2_maxExtent_transInv.y > other.minExtent.y &&
                obb2_minExtent_transInv.y < other.maxExtent.y &&
                obb2_maxExtent_transInv.z > other.minExtent.z &&
                obb2_minExtent_transInv.z < other.maxExtent.z)
                return true;
        }

        return false;
    }

    public override void ChangeMaterialBasedOnCollsion(bool collisionTest)
    {
        if (collisionTest || collisionDetededThisFrame)
        {
            renderer.material = mat_red;
            collisionDetededThisFrame = true;
        }
        else
            renderer.material = mat_green;
    }
}
