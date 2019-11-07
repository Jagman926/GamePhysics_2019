using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollisionHull3D : CollisionHull3D
{
    public CircleCollisionHull3D() : base(CollisionHullType3D.hull_circle) { }

    // Variables needed
    // 1. Radius
    // 2. Center (can be found using position)
    [Range(0.0f, 100f)]
    public float radius;
    public float restitution;
    public Vector3 center;

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
        colliding = false;
        center = particle.position;
        radius = particle.radiusOuter;
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
                        c.status = true;
                        //Does the correct populate
                        //PopulateCollisionClassCircleVsCirlce(this, (CircleCollisionHull3D)other, ref c);
                        ////Resolves the collisions
                        //ResolveCollisions(ref c);
                        //
                        //clearContacts(ref c); //Clears the information used after contacts have been resolved
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
                    if (c.status)
                    {
                        c.status = false;
                    }
                    else
                    {
                        //PopulateCollisionClassAABBVSCircle(this, (AxisAlignBoundingBoxCollisionHull3D)other, ref c);
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
            // If other object is a obb hull
            case CollisionHullType3D.hull_obb:
                if (TestCollisionVsOBB((ObjectBoundingBoxCollisionHull3D)other, ref c))
                {
                    Debug.Log(gameObject.name + " Colliding with " + other.name);
                    if (c.status)
                    {
                        c.status = false;
                    }
                    else
                    {
                        //PopulateCollisionClassOBBVSCircle(this, (ObjectBoundingBoxCollisionHull3D)other, ref c);
                        ////Resolves the collisions
                        //ResolveCollisions(ref c);
                        //clearContacts(ref c); //Clears the information used after contacts have been resolved
                        c.status = true;
                    }
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

    //Circle vs Circle
    public override bool TestCollisionVsCircle(CircleCollisionHull3D other, ref Collision c)
    {
        // collision if distance between centers is <= sum of radii
        // optimized collision if (distance is between centers) squared <= (sum of radii) squared
        // 1. get the two centers
        // 2. take the difference
        // 3. distance squared = dot(diff, diff) or sqrMagnitude
        // 4. add the radii
        // 5. square the sum
        // 6. DO THE TEST: pass if distanceSq <= sumSq
        float distanceSqr = (other.center - center).sqrMagnitude;
        float radiiSqr = (other.radius + radius) * (other.radius + radius);

        if (distanceSqr <= radiiSqr)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull3D other, ref Collision c)
    {
        /*
                            
                     ---------------- <max
                    |                |         
                    |                |
                    |                |
                    |                |
               min> ----------------

         */

        // find closest point to the circle on the box
        // (done by clamping center of circle to be within box dimensions)
        // if closest point is within circle, pass (do point vs circle test)

        float clampX = Mathf.Clamp(center.x, other.minExtent.x, other.maxExtent.x);
        float clampY = Mathf.Clamp(center.y, other.minExtent.y, other.maxExtent.y);
        float clampZ = Mathf.Clamp(center.z, other.minExtent.z, other.maxExtent.z);

        Vector3 closestPoint = new Vector3(clampX, clampY, clampZ);

        if ((closestPoint - center).sqrMagnitude < radius * radius)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull3D other, ref Collision c)
    {
        // same as above, but first...
        // multiply circle center by box world matrix inverse
        // 1. Get world matrix of OBB
        // 2. Multiply inverse of matrix by center point of circle
        // 3. Same as collision vs AABB

        Vector3 circleCenter = other.transform.InverseTransformPoint(center);

        circleCenter += other.center;

        float clampX = Mathf.Clamp(circleCenter.x, other.minExtent.x, other.maxExtent.x);
        float clampY = Mathf.Clamp(circleCenter.y, other.minExtent.y, other.maxExtent.y);
        float clampZ = Mathf.Clamp(center.z, other.minExtent.z, other.maxExtent.z);

        Vector3 closestPoint = new Vector3(clampX, clampY, clampZ);

        if ((closestPoint - circleCenter).sqrMagnitude < radius * radius)
            return true;
        else
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
