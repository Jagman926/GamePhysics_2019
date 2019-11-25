using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxCollisionHull3D : CollisionHull3D
{
    public AxisAlignBoundingBoxCollisionHull3D() : base(CollisionHullType3D.hull_aabb) { }

    // Variables needed
    // 1. Min x/y
    // 2. Max x/y
    // 3. Center
    public Vector3 center;
    public Vector3 minExtent;
    public Vector3 maxExtent;

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

        Vector3 halfExtents = new Vector3(0.5f * particle.width, 0.5f * particle.height, 0.5f * particle.length);

        minExtent = new Vector3(center.x - halfExtents.x, center.y - halfExtents.y, center.z - halfExtents.z);
        maxExtent = new Vector3(center.x + halfExtents.x, center.y + halfExtents.y, center.z + halfExtents.z);
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
                        //PopulateCollisionClassAABBVSCircle((CircleCollisionHull3D)other, this, ref c);
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

        if (other.TestCollisionVsAABB(this, ref c))
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull3D other, ref Collision c)
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
            minExtent.y < other.maxExtent.y &&
            maxExtent.z > other.minExtent.z &&
            minExtent.z < other.maxExtent.z)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull3D other, ref Collision c)
    {
        // same as above twice...
        //  first, find max extents of OBB, do AABB vs this box
        //  then, transform this box into OBB's space, find the max extents, repeat
        // 1. Get OBB max/min extents in rotation (done in object script)
        // 2. Get AABB max/min extents from world matrix inv of OBB
        // 3. use same AABB vs AABB test for both scenarios using the others normal max/min extents

        Vector3 aabb_maxExtent_transInv = other.transform.worldToLocalMatrix.MultiplyPoint(maxExtent);
        Vector3 aabb_minExtent_transInv = other.transform.worldToLocalMatrix.MultiplyPoint(minExtent);
        //Vector3 obb_maxExtent_transInv = transform.worldToLocalMatrix.MultiplyPoint(maxExtent);
        //Vector3 obb_minExtent_transInv = transform.worldToLocalMatrix.MultiplyPoint(minExtent);
        //aabb_maxExtent_transInv += other.center;
        //aabb_minExtent_transInv += other.center;

        if (maxExtent.x > other.minExtent_Local.x &&
            minExtent.x < other.maxExtent_Local.x &&
            maxExtent.y > other.minExtent_Local.y &&
            minExtent.y < other.maxExtent_Local.y &&
            maxExtent.z > other.minExtent_Local.z &&
            minExtent.z < other.maxExtent_Local.z)
        {
            if (aabb_maxExtent_transInv.x > other.minExtent_Local.x &&
                aabb_minExtent_transInv.x < other.maxExtent_Local.x &&
                aabb_maxExtent_transInv.y > other.minExtent_Local.y &&
                aabb_minExtent_transInv.y < other.maxExtent_Local.y &&
                aabb_maxExtent_transInv.z > other.minExtent_Local.z &&
                aabb_minExtent_transInv.z < other.maxExtent_Local.z)
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
