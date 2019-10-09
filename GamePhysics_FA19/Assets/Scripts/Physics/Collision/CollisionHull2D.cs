using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
    public class Collision
    {
        public struct Contact{
            public Vector2 point;
            public Vector2 normal;
            public float penetration;

        }

        public float restitution;
        public CollisionHull2D a = null, b = null;
        public Contact[] contact = new Contact[4];
        public int contactCount = 0;
        public bool status = false;

        public int itterations = 3; //Max itterations allowed
        public int iterationsUsed = 0;

        Vector2 closingVelocity;
    }

    public enum CollisionHullType2D
    {
        hull_circle,
        hull_aabb,
        hull_obb,

    }

    public CollisionHullType2D type { get; }

    protected CollisionHull2D(CollisionHullType2D type_set)
    {
        type = type_set;
    }

    public Particle2D particle;

    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    public void ResolveCollisions(ref Collision collisionData)
    {
        //TODO: Calculate contact normals on collision
        //TODO: Calculate penetration on collisions
        //TODO: Fill the collision class on collison lol

        //Gets the particles
        Particle2D particleA = collisionData.a.particle;
        Particle2D particleB = collisionData.b.particle;

        collisionData.iterationsUsed = 0;
        int i = 0;

        while(collisionData.iterationsUsed < collisionData.itterations)
        {
            float max = float.MaxValue;
            int maxIndex = collisionData.contactCount;

            for(i = 0; i < collisionData.contactCount; i++)
            {
                Vector2 contactNormal = collisionData.contact[i].normal;
                float seperatingVelocity = (particleA.GetVelocity() - particleB.GetVelocity()).magnitude * contactNormal.magnitude; //NOTE Might need to be a float instead of a vector 3

                if(seperatingVelocity < max &&
                    seperatingVelocity < 0 && collisionData.contact[i].penetration > 0)
                {
                    max = seperatingVelocity;
                    maxIndex = i;
                }
            }

            if (maxIndex == collisionData.contactCount)
                break;

            Resolve(ref collisionData, maxIndex);

            collisionData.iterationsUsed++;
        }
    }

    void Resolve(ref Collision collisionData, int contactBeingCalculated)
    {
        ResolveContact(ref collisionData, contactBeingCalculated);
        ResolveInterpenetration(ref collisionData, contactBeingCalculated);
    }

    void ResolveContact(ref Collision collisionData, int contactBeingCalculated)
    {
        //1. Calculate inital seperating velocity
        // - if Pb != null, return (Velocitya - Velocityb) * collisionNormal
        //2. Check if impuls is required
        // - if SVInital >0, return
        //3. Calculate new seperating velocity
        // - newSV = -SVinitial * restitution
        // - DeltaSV = newSV - SVInitial
        //4. Caclulate total inverse mass
        // - totalInvMass = InvMassA + invMassB
        //5. calculate impulse
        // - impulse = deltaSV / totalInvMass
        // - Vector3 inpulsePerIMass = collisionNormal * impulse
        //6. Apply inpulse to velocity

        //Sets up needed variables

        //Gets the particles
        Particle2D particleA = collisionData.a.particle;
        Particle2D particleB = collisionData.b.particle;

        Vector2 contactNormal = collisionData.contact[contactBeingCalculated].normal;

        //I think this is calculated on collision for effeciancy

        //Calculate the collision normal
        //n = (Pa - Pb) / |(Pa - Pb)}|
        //contactNormal = particleA.position - particleB.position; 
        //collisionData.contact[contactBeingCalculated].normal = collisionData.contact[contactBeingCalculated].normal / collisionData.contact[contactBeingCalculated].normal.magnitude; //NOTE: this is probably inefficant (using magnitude & division) rework later.

        //1. Calculate inital seperating velocity
        float initalSeperatingVelocity = (particleA.GetVelocity() - particleB.GetVelocity()).magnitude * contactNormal.magnitude; //NOTE Might need to be a float instead of a vector 3

        //2. Check if impuls is required
        if (initalSeperatingVelocity > 0f)
            return;

        //3. Calculate new seperating velocity
        float newSeperatingVelocity = -initalSeperatingVelocity * collisionData.contact[contactBeingCalculated].restitution;
        float deltaSeperatingVelocity = newSeperatingVelocity - initalSeperatingVelocity;


        //4. Caclulate total inverse mass
        float totalInvMass = particleA.GetInvMass() + particleB.GetInvMass();

        //5. calculate impulse
        float impulse = deltaSeperatingVelocity / totalInvMass;
        Vector2 impulsePerIMass = collisionData.contact[contactBeingCalculated].normal * impulse;

        //6. Apply inpulse to velocity
        //Two potentail ways to handle this, either A: Set the velocity directly, or B: Add the impulse as a force.
        //Method A
        particleA.SetVelocity(particleA.GetVelocity() + impulsePerIMass * particleA.GetInvMass());
        particleB.SetVelocity(particleA.GetVelocity() + impulsePerIMass * -particleA.GetInvMass());

        //Method B
        //particleA.AddForce(particleA.GetVelocity() + impulsePerIMass * particleA.GetInvMass());
        //particleB.AddForce(particleA.GetVelocity() + impulsePerIMass * -particleA.GetInvMass());

    }

    void ResolveInterpenetration(ref Collision collisionData, int contactBeingCalculated)
    {
        //1. Get penetration (though collision)
        //2. Check for penetration
        // - ifPenetraction <= 0, return
        //3. Caculate total inverse mass
        //4. find amount of penitration per inverse mass
        // - collisionNormal * (penitration / totalInverseMass)
        //5. Calculate movement amounts
        // - particleMovementA = movePerInverseMass * particleAInverseMass
        // - particleMovementB = movePerInverseMass * -particleBInverseMass
        //6. Apply to positions
        // - particleAPosition += particleMovementA
        // - particleBPosition += -particleMovementB

        //Sets up needed variables

        //Gets the particles
        Particle2D particleA = collisionData.a.particle;
        Particle2D particleB = collisionData.b.particle;

        //I think this is calculated on collision for effeciancy

        //Calculate the collision normal
        //n = (Pa - Pb) / |(Pa - Pb)}|
        //collisionData.contactNormal = particleA.position - particleB.position;
        //collisionData.contactNormal = collisionData.contactNormal / collisionData.contactNormal.magnitude; //NOTE: this is probably inefficant (using magnitude & division) rework later.

        //1. Get penetration (though collision) 
        //This is done on collision detection, so ignore here

        //2. Check for penetration
        if (collisionData.contact[contactBeingCalculated].penetration <= 0)
            return;

        //3. Caculate total inverse mass
        float totalInvMass = particleA.GetInvMass() + particleB.GetInvMass();

        //4. find amount of penitration per inverse mass
        Vector2 movmentPerInvMass = -collisionData.contact[contactBeingCalculated].normal * (collisionData.contact[contactBeingCalculated].penetration / totalInvMass);

        //5. Calculate movement amounts
        // - particleMovementA = movePerInverseMass * particleAInverseMass
        // - particleMovementB = movePerInverseMass * -particleBInverseMass
        Vector2 particleMovmentA = movmentPerInvMass * particleA.GetInvMass();
        Vector2 particleMovmentB = movmentPerInvMass * -particleB.GetInvMass();

        //6. Apply to positions
        // - particleAPosition += particleMovementA
        // - particleBPosition += -particleMovementB
        particleA.SetPosition(particleA.GetPosition() + particleMovmentA);
        particleB.SetPosition(particleB.GetPosition() - particleMovmentB);

    }

    public bool PopulateCollisionClassCircleVsCirlce(CircleCollisionHull2D a, CircleCollisionHull2D b, ref Collision c)
    {
        // 1. Finds the vector between objects
        // 2. Calculate the size (magnitude of midline)
        // 3. test to see if the size is large enough
        // 4. Creature the normal
        // 5. Calculate contact info (normal, point, and penetration)


        // 1. Finds the vector between objects
        Vector2 midline = a.particle.position - b.particle.position;

        // 2. Calculate the size (magnitude of midline)
        float size = midline.magnitude;

        // 3. test to see if the size is large enough
        if (size <= 0.0f || size >= a.radius + b.radius)
        {
            return false;
        }

        // 4. Creature the normal
        Vector2 normal = midline * (1.0f / size); //Potential optimization here, cut division?

        // 5. Calculate contact info
        c.contact[0].normal = normal;
        c.contact[0].point = a.particle.position + midline * 0.5f;
        c.contact[0].penetration = (a.radius + b.radius - size);


        return true;

    }

    public bool PopulateCollisionClassAABBVSCircle(CircleCollisionHull2D cirlce, AxisAlignBoundingBoxCollisionHull2D aabb, ref Collision c)
    {
        // 1. Transform the center of teh sphere into box coordinates
        // 2. Check to see if it's valid based on center
        // 3. Clamp each coordinate to the box 
        // 4. Check for contact
        // 5. Compile the contact & generate the values

        // 1. Transform the center of the sphere into box coordinates
        Vector2 ceneter = cirlce.particle.position;
        Vector2 relCenter = aabb.particle.transform.InverseTransformPoint(ceneter);

        // 2. Check to see if it's valid based on center
        if (Mathf.Abs(relCenter.x) - cirlce.radius > aabb.maxExtent.x ||
            Mathf.Abs(relCenter.y) - cirlce.radius > aabb.maxExtent.y)
            return false;

        // 3. Clamp each coordinate to the box 
        float distance;
        Vector2 closestPoint = new Vector2(0,0);

        distance = relCenter.x;
        if (distance > aabb.maxExtent.x) distance = aabb.maxExtent.x;
        if (distance < -aabb.maxExtent.x) distance = -aabb.maxExtent.x;
        closestPoint.x = distance;

        distance = relCenter.y;
        if (distance > aabb.maxExtent.y) distance = aabb.maxExtent.y;
        if (distance < -aabb.maxExtent.y) distance = -aabb.maxExtent.y;
        closestPoint.y = distance;

        distance = (closestPoint - relCenter).sqrMagnitude;
        if (distance > cirlce.radius * cirlce.radius)
            return false;

        Vector2 closestPtWorld = aabb.transform.TransformVector(closestPoint);

        c.contact[0].normal = closestPtWorld - ceneter;
        c.contact[0].normal = Vector3.Normalize(c.contact[0].normal);
        c.contact[0].point = closestPtWorld;
        c.contact[0].penetration = cirlce.radius - Mathf.Sqrt(distance);

        return true;

    }

    public void clearContacts(ref Collision c)
    {
        for(int i = 0; i < c.contactCount; i++)
        {
            c.contact[i].normal = Vector2.zero;
            c.contact[i].point = Vector2.zero;
            c.contact[i].penetration = 0;
           

        }
    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision c)
    {

        return false;
    }

    public abstract void UpdateTransform();

    public abstract bool isColliding(CollisionHull2D other, ref Collision c);

    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c);

    public abstract bool TestCollisionVsAABB(AxisAlignBoundingBoxCollisionHull2D other, ref Collision c);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxCollisionHull2D other, ref Collision c);
}
