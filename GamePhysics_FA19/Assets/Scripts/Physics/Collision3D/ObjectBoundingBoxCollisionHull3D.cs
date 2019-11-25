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
    Vector3[] cornersLocal;
    Vector3[] cornersWorld;

    void Awake()
    {
        cornersLocal = new Vector3[8];
        cornersWorld = new Vector3[8];
        renderer = gameObject.GetComponent<Renderer>();
        cornersLocal[0]= new Vector3(halfExtents.x, halfExtents.y, halfExtents.z);
        cornersLocal[1]= new Vector3(halfExtents.x, -halfExtents.y, halfExtents.z);
        cornersLocal[2]= new Vector3(-halfExtents.x, halfExtents.y, halfExtents.z);
        cornersLocal[3]= new Vector3(-halfExtents.x, -halfExtents.y, halfExtents.z);
        cornersLocal[4]= new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
        cornersLocal[5]= new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);
        cornersLocal[6]= new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
        cornersLocal[7]= new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
        halfExtents = new Vector3(0.5f * particle.width, 0.5f * particle.height, 0.5f * particle.length);


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
            cornersWorld[i] = J_Physics.LocalToWorldDirection(cornersLocal[i], particle.GetTransformationMatrix());
        }

        // Get min & max extents in world space
        minExtent_World = GetMinExtent(cornersWorld);
        maxExtent_World = GetMaxExtent(cornersWorld);


    }

    Vector3 GetMinExtent(Vector3[] vecArray)
    {
        Vector3 currentMinExtent = Vector3.zero;

        for(int i = 0; i < vecArray.Length; i++)
        {
            if (i == 0)
                currentMinExtent = vecArray[i];
            else
            {
                if (vecArray[i].x == Mathf.Min(currentMinExtent.x, vecArray[i].x))
                    currentMinExtent.x = vecArray[i].x;
                if (vecArray[i].y == Mathf.Min(currentMinExtent.y, vecArray[i].y))
                    currentMinExtent.y = vecArray[i].y;
                if (vecArray[i].z == Mathf.Min(currentMinExtent.z, vecArray[i].z))
                    currentMinExtent.z = vecArray[i].z;
            }
        }

        return currentMinExtent;
    }

    Vector3 GetMaxExtent(Vector3[] vecArray)
    {
        Vector3 currentMaxExtent = Vector3.zero;

        for (int i = 0; i < vecArray.Length; i++)
        {
            if (i == 0)
                currentMaxExtent = vecArray[i];
            else
            {
                if (vecArray[i].x == Mathf.Max(currentMaxExtent.x, vecArray[i].x))
                    currentMaxExtent.x = vecArray[i].x;
                if (vecArray[i].y == Mathf.Max(currentMaxExtent.y, vecArray[i].y))
                    currentMaxExtent.y = vecArray[i].y;
                if (vecArray[i].z == Mathf.Max(currentMaxExtent.z, vecArray[i].z))
                    currentMaxExtent.z = vecArray[i].z;
            }
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

        if (obbThis_maxExtent_transInv != Vector3.zero || obbThis_minExtent_transInv != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(obbThis_maxExtent_transInv, .05f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(obbThis_minExtent_transInv, .05f);
        }


        if (obbOther_maxExtent_transInv != Vector3.zero || obbOther_minExtent_transInv != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(obbOther_maxExtent_transInv, .05f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(obbOther_minExtent_transInv, .05f);
        }
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull3D other, ref Collision c)
    {
        // same as AABB-OBB part 2, twice
        // 1. Get OBB1 max/min extents from world matrix inv of OBB2
        // 2. Get OBB2 max/min extents from world matrix inv of OBB1
        // 3. For both test, and if both true, pass

        // Other object multiplied by inverse world matrix
        obbThis_maxExtent_transInv = J_Physics.WorldToLocalDirection(maxExtent_Local, other.particle.GetTransformationMatrix());
        obbThis_minExtent_transInv = J_Physics.WorldToLocalDirection(minExtent_Local, other.particle.GetTransformationMatrix());
        // This object multiplied by inverse world matrix
        obbOther_maxExtent_transInv = J_Physics.WorldToLocalDirection(other.maxExtent_Local, particle.GetTransformationMatrix());
        obbOther_minExtent_transInv = J_Physics.WorldToLocalDirection(other.minExtent_Local, particle.GetTransformationMatrix());

    if (obbThis_maxExtent_transInv.x > other.minExtent_Local.x &&
        obbThis_minExtent_transInv.x < other.maxExtent_Local.x &&
        obbThis_maxExtent_transInv.y > other.minExtent_Local.y &&
        obbThis_minExtent_transInv.y < other.maxExtent_Local.y &&
        obbThis_maxExtent_transInv.z > other.minExtent_Local.z &&
        obbThis_minExtent_transInv.z < other.maxExtent_Local.z)
    {
        if (obbOther_maxExtent_transInv.x > minExtent_Local.x &&
            obbOther_minExtent_transInv.x < maxExtent_Local.x &&
            obbOther_maxExtent_transInv.y > minExtent_Local.y &&
            obbOther_minExtent_transInv.y < maxExtent_Local.y &&
            obbOther_maxExtent_transInv.z > minExtent_Local.z &&
            obbOther_minExtent_transInv.z < maxExtent_Local.z)
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
