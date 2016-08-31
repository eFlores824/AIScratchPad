using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class UnalignedCollisionAvoidance : MonoBehaviour {

    public float maxSpeed;
    public float avoidanceRadius;
    public float secondsToPredict;
    public Vector3 initialVelocity;
    public GameObject[] objectsAvoiding;
    public GameObject[] destinations;

    private Transform theTransform;
    private Rigidbody rigid;
    private Rigidbody[] objectsRigid;
    private Transform[] objectsTransform;
    private Vector3 currentDestination;
    private int currentIndex = 0;
    private bool nextDestination = false;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();

        setRigids();
        setTransforms();

        rigid.velocity = initialVelocity.normalized * maxSpeed;
        currentDestination = destinations[0].transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 steering = steer();
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

        theTransform.forward = rigid.velocity.normalized;
    }

    private void setRigids()
    {
        objectsRigid = new Rigidbody[objectsAvoiding.Length];
        for (uint i = 0; i < objectsRigid.Length; ++i)
        {
            objectsRigid[i] = objectsAvoiding[i].GetComponent<Rigidbody>();
        }
    }

    private void setTransforms()
    {
        objectsTransform = new Transform[objectsAvoiding.Length];
        for (uint i = 0; i < objectsTransform.Length; ++i)
        {
            objectsTransform[i] = objectsAvoiding[i].GetComponent<Transform>();
        }
    }

    private Vector3 steer()
    {
        Vector3 steering = new Vector3();
        bool avoided = false;
        for (uint i = 0; i < objectsAvoiding.Length; ++i)
        {
            Vector3 futurePosition;
            if (willInterset(i, out futurePosition))
            {
                steering = avoid(i, futurePosition);
                avoided = true;
                break;
            }
        }
        if (!avoided)
        {
            steering = seek();
        }
        return steering;
    }

    void LateUpdate()
    {
        if (nextDestination)
        {
            nextDestination = false;
            ++currentIndex;
            if (currentIndex == destinations.Length)
            {
                currentIndex = 0;
            }
            currentDestination = destinations[currentIndex].GetComponent<Transform>().position;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        nextDestination = collision.gameObject == destinations[currentIndex];
    }

    private Vector3 seek()
    {
        Vector3 fromDestination = currentDestination - theTransform.position;
        fromDestination = fromDestination.normalized * maxSpeed;
        return fromDestination - rigid.velocity;
    }

    private Vector3 avoid(uint avoidingIndex, Vector3 collisionPosition)
    {
        float avoidingSpeed = objectsRigid[avoidingIndex].velocity.magnitude;
        float avoidingDistanceToCollision = (collisionPosition - objectsTransform[avoidingIndex].position).magnitude;
        float currentSpeed = rigid.velocity.magnitude;
        float currentDistanceToCollision = (collisionPosition - theTransform.position).magnitude;

        float timeForAvoidingReach = avoidingDistanceToCollision / avoidingSpeed;
        float timeForCurrentReach = currentDistanceToCollision / currentSpeed;
        Vector3 forward = timeForAvoidingReach < timeForCurrentReach ? theTransform.forward * -1.0f: theTransform.forward;

        Vector3 desiredVelocity = theTransform.right * -1.0f;
        Vector3 centerPosition = Vector3.Lerp(theTransform.position + desiredVelocity, theTransform.position + forward, 0.5f);
        desiredVelocity = (centerPosition - theTransform.position).normalized * maxSpeed;
        return desiredVelocity - rigid.velocity;
    }

    private bool willInterset(uint objectIndex, out Vector3 futurePosition)
    {
        futurePosition = theTransform.position + rigid.velocity * secondsToPredict;
        Vector3 nextObjectFuturePosition = objectsTransform[objectIndex].position + objectsRigid[objectIndex].velocity * secondsToPredict;
        Debug.DrawRay(futurePosition, nextObjectFuturePosition - futurePosition);
        float distanceBetween = (futurePosition - nextObjectFuturePosition).magnitude;
        return distanceBetween <= avoidanceRadius;
    }
}
