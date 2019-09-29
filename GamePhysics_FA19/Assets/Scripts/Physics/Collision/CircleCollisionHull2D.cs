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

    public override void UpdateTransform()
    {
        center = particle.position;
        radius = particle.radiusOuter;
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

         */

        // find closest point to the circle on the box
        // (done by clamping center of circle to be within box dimensions)
        // if closest point is within circle, pass (do point vs circle test)

        Vector2 difference = center - other.center;

        float clampX = Mathf.Clamp(difference.x, -other.halfExtents.x, other.halfExtents.x);
        float clampY = Mathf.Clamp(difference.y, -other.halfExtents.y, other.halfExtents.y);

        Vector2 closestPoint = new Vector2(other.center.x + clampX, other.center.y + clampY);

        if ((closestPoint-center).magnitude < radius)
            return true;
        else
            return false;
    }

    public Vector2 circleCenter;

    public override bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull2D other)
    {
        // same as above, but first...
        // multiply circle center by box world matrix inverse
        // 1. Get world matrix of OBB
        // 2. Multiply inverse of matrix by center point of circle
        // 3. Same as collision vs AABB

        Matrix4x4 objectWorldMatrix_inv = other.transform.localToWorldMatrix.inverse;

        circleCenter = objectWorldMatrix_inv.MultiplyPoint3x4(center);

        Vector2 difference = circleCenter - other.center;

        float clampX = Mathf.Clamp(difference.x, -other.halfExtents.x, other.halfExtents.x);
        float clampY = Mathf.Clamp(difference.y, -other.halfExtents.y, other.halfExtents.y);

        Vector2 closestPoint = new Vector2(other.center.x + clampX, other.center.y + clampY);

        if ((closestPoint-center).magnitude < radius)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(circleCenter, radius);
    }
}
