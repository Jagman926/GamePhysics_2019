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
    public Vector3 minExtent_Local;
    public Vector3 maxExtent_Local;
    public Vector3 minExtent_World;
    public Vector3 maxExtent_World;

    [Header("Debug Material")]
    bool colliding;
    private Renderer renderer;
    public Material mat_red;
    public Material mat_green;
    Vector3[] cornersLocal = new Vector3[0];
    Vector3[] cornersWorld = new Vector3[0];

    void Awake()
    {
        cornersLocal = new Vector3[8];
        cornersWorld = new Vector3[8];
        renderer = gameObject.GetComponent<Renderer>();
        halfExtents = new Vector3(0.5f * particle.width, 0.5f * particle.height, 0.5f * particle.length);
        cornersLocal[0]= new Vector3(halfExtents.x, halfExtents.y, halfExtents.z);
        cornersLocal[1]= new Vector3(halfExtents.x, -halfExtents.y, halfExtents.z);
        cornersLocal[2]= new Vector3(-halfExtents.x, halfExtents.y, halfExtents.z);
        cornersLocal[3]= new Vector3(-halfExtents.x, -halfExtents.y, halfExtents.z);
        cornersLocal[4]= new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
        cornersLocal[5]= new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);
        cornersLocal[6]= new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
        cornersLocal[7]= new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);



        // Determine extents
        minExtent_Local = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
        maxExtent_Local = new Vector3(halfExtents.x, halfExtents.y, halfExtents.z);
    }

    public override void UpdateTransform()
    {
        center = particle.position;

        //Get corners in world space
        for (int i = 0; i < cornersLocal.Length; i++)
        {
            cornersWorld[i] = J_Physics.LocalToWorldPosition(cornersLocal[i], center, particle.GetRotationMatrix());
        }

        // Get min & max extents in world space
        minExtent_World = GetMinExtent(cornersWorld);
        maxExtent_World = GetMaxExtent(cornersWorld);


    }

    Vector3 GetMinExtent(Vector3[] vecArray)
    {
        Vector3 currentMinExtent = vecArray[0];

        for(int i = 0; i < vecArray.Length; i++)
        {

            currentMinExtent.x = Mathf.Min(currentMinExtent.x, vecArray[i].x);
            currentMinExtent.y = Mathf.Min(currentMinExtent.y, vecArray[i].y);
            currentMinExtent.z = Mathf.Min(currentMinExtent.z, vecArray[i].z);

        }

        return currentMinExtent;
    }

    Vector3 GetMaxExtent(Vector3[] vecArray)
    {
        Vector3 currentMaxExtent = vecArray[0];

        for (int i = 0; i < vecArray.Length; i++)
        {

            currentMaxExtent.x = Mathf.Max(currentMaxExtent.x, vecArray[i].x);
            currentMaxExtent.y = Mathf.Max(currentMaxExtent.y, vecArray[i].y);
            currentMaxExtent.z = Mathf.Max(currentMaxExtent.z, vecArray[i].z);

        }

        return currentMaxExtent;
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

    public Vector3 obbThis_maxExtent_transInv;
    public Vector3 obbThis_minExtent_transInv;
    public Vector3 obbOther_maxExtent_transInv;
    public Vector3 obbOther_minExtent_transInv;

    
    private void OnDrawGizmos()
    {
        foreach(Vector3 point in cornersWorld)
        {
            Gizmos.DrawSphere(point, 0.05f);
        }
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull3D other, ref Collision c)
    {
        // same as AABB-OBB part 2, twice
        // 1. Get OBB1 max/min extents from world matrix inv of OBB2
        // 2. Get OBB2 max/min extents from world matrix inv of OBB1
        // 3. For both test, and if both true, pass

        // Other object multiplied by inverse world matrix
        obbThis_maxExtent_transInv = J_Physics.WorldToLocalPosition(maxExtent_World, other.center, other.particle.GetRotationMatrixInverse());
        obbThis_minExtent_transInv = J_Physics.WorldToLocalPosition(minExtent_World, other.center, other.particle.GetRotationMatrixInverse());
        // This object multiplied by inverse world matrix
        obbOther_maxExtent_transInv = J_Physics.WorldToLocalPosition(other.maxExtent_World, center, particle.GetRotationMatrixInverse());
        obbOther_minExtent_transInv = J_Physics.WorldToLocalPosition(other.minExtent_World, center, particle.GetRotationMatrixInverse());

    if (obbThis_maxExtent_transInv.x > other.minExtent_World.x &&
        obbThis_minExtent_transInv.x < other.maxExtent_World.x &&
        obbThis_maxExtent_transInv.y > other.minExtent_World.y &&
        obbThis_minExtent_transInv.y < other.maxExtent_World.y &&
        obbThis_maxExtent_transInv.z > other.minExtent_World.z &&
        obbThis_minExtent_transInv.z < other.maxExtent_World.z)
    {
        if (obbOther_maxExtent_transInv.x > minExtent_World.x &&
            obbOther_minExtent_transInv.x < maxExtent_World.x &&
            obbOther_maxExtent_transInv.y > minExtent_World.y &&
            obbOther_minExtent_transInv.y < maxExtent_World.y &&
            obbOther_maxExtent_transInv.z > minExtent_World.z &&
            obbOther_minExtent_transInv.z < maxExtent_World.z)
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
