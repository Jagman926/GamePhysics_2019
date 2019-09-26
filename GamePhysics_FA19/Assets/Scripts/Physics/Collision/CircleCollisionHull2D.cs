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
    public float radius = 0.0f;
    public Vector2 center = Vector2.zero;

    void Start()
    {

    }

    void Update()
    {

    }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
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

    public override bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull2D other)
    {
        /*
                            
                     ---------------- <max
                    |                |         
                    |                |
                    |                |
                    |                |
               min> ----------------

            Top = max.y
            Bot = min.y
            Left = min.x
            Right = max.x
         */

        // find closest point to the circle on the box
        // (done by clamping center of circle to be within box dimensions)
        // if closest point is within circle, pass (do point vs circle test)

        Vector2 point = center;

        if (point.x > other.maxExtent.x) 
            point.x = other.maxExtent.x;
        if (point.x < other.minExtent.x) 
            point.x = other.minExtent.x;
        if (point.y > other.minExtent.y)
            point.y = other.minExtent.y;
        if (point.y < other.maxExtent.y)
            point.y = other.maxExtent.y;

        if((point - center).sqrMagnitude < radius)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull2D other)
    {
        // same as above, but first...
        // multiply circle center by box world matrix inverse -> ? https://docs.unity3d.com/ScriptReference/Transform-localToWorldMatrix.html

        return false;
    }

    public override void OnDrawGizmosSelected()
    {
        // Draw debug sphere of circle collision hull
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, radius);
    }
}
