using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollisionHull2D : CollisionHull2D
{
    public CircleCollisionHull2D() : base(CollisionHullType2D.hull_circle) { }

    // Variables needed
    // 1. Radius
    // 2. Center (can be found using position)
    [Range(0.0f, 100f)]
    public float radius;
    public float restitution;
    public Vector2 center;

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

    public override bool isColliding(CollisionHull2D other, ref Collision c)
    {
        switch (other.type)
        {
            // If other object is a circle hull
            case CollisionHullType2D.hull_circle:
                if (TestCollisionVsCircle((CircleCollisionHull2D)other, ref c))
                {
                    //Does the correct populate
                    PopulateCollisionClassCircleVsCirlce(this, (CircleCollisionHull2D)other, ref c);
                    //Resolves the collisions
                    ResolveCollisions(ref c);
                    Debug.Log(gameObject.name + " Colliding with " + other.name);

                    clearContacts(ref c); //Clears the information used after contacts have been resolved

                    colliding = true;


                }
                break;
            // If other object is a aabb hull
            case CollisionHullType2D.hull_aabb:
                if (TestCollisionVsAABB((AxisAlignBoundingBoxCollisionHull2D)other, ref c))
                {
                    PopulateCollisionClassAABBVSCircle(this, (AxisAlignBoundingBoxCollisionHull2D)other, ref c);
                    //Resolves the collisions
                    ResolveCollisions(ref c);
                    clearContacts(ref c); //Clears the information used after contacts have been resolved

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

    //Circle vs Circle
    public override bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c)
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

    public override bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull2D other, ref Collision c)
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

        Vector2 closestPoint = new Vector2(clampX, clampY);

        if ((closestPoint - center).sqrMagnitude < radius * radius)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull2D other, ref Collision c)
    {
        // same as above, but first...
        // multiply circle center by box world matrix inverse
        // 1. Get world matrix of OBB
        // 2. Multiply inverse of matrix by center point of circle
        // 3. Same as collision vs AABB

        Vector2 circleCenter = other.transform.InverseTransformPoint(center);

        circleCenter += other.center;

        float clampX = Mathf.Clamp(circleCenter.x, other.minExtent.x, other.maxExtent.x);
        float clampY = Mathf.Clamp(circleCenter.y, other.minExtent.y, other.maxExtent.y);

        Vector2 closestPoint = new Vector2(clampX, clampY);

        if ((closestPoint - circleCenter).sqrMagnitude < radius * radius)
            return true;
        else
            return false;
    }


}
