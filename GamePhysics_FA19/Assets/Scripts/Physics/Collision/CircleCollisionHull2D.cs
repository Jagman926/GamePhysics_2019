using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollisionHull2D : CollisionHull2D
{
    public CircleCollisionHull2D() : base(CollisionHullType2D.hull_circle) { }

    [Range(0.0f, 100f)]
    public float radius;

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

        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull2D other)
    {
        // find closest point to the circle on the box
        // (done by clamping center of circle to be within box dimensions)
        // if closest point is within circle, pass (do point vs circle test)

        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull2D other)
    {
        // same as above, but first...
        // multiply circle center by box world matrix inverse

        return false;
    }
}
