using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Hide : MonoBehaviour {

    public GameObject[] obstacles;
    public GameObject hidingFrom;
    public float distanceFromBoundary;
    public float maxSpeed;
    public float slowingRadius;

    private Transform theTransform;
    private Transform hidingTransform;
    private Rigidbody rigid;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        hidingTransform = hidingFrom.GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 steering = hide();
        if (steering.magnitude > slowingRadius / 50.0f)
        {
            steering /= rigid.mass;
        }
        rigid.velocity += steering;
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

        theTransform.forward = rigid.velocity.normalized;
	}

    private Vector3 hide()
    {
        Vector3 closestPosition = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3[] spots = hidingSpots();
        for (uint i = 0; i < spots.Length; ++i)
        {
            Vector3 position = spots[i];
            Debug.DrawRay(theTransform.position, position - theTransform.position, Color.red);
            closestPosition = (position - hidingTransform.position).magnitude > closestPosition.magnitude ? position : closestPosition;
        }
        return seek(closestPosition);
    }

    private Vector3 arrival(Vector3 arrivalPosition)
    {
        Vector3 fromDestination = arrivalPosition - theTransform.position;
        float speed;
        if (fromDestination.magnitude < slowingRadius)
        {
            speed = maxSpeed * (fromDestination.magnitude / slowingRadius);
        }
        else
        {
            speed = maxSpeed;
        }
        Vector3 desiredVelocity = (speed / fromDestination.magnitude) * fromDestination;
        Debug.DrawRay(theTransform.position, desiredVelocity);
        return desiredVelocity - rigid.velocity;
    }

    private Vector3 seek(Vector3 targetPosition)
    {
        Vector3 fromDestination = targetPosition - theTransform.position;
        fromDestination = fromDestination.normalized * maxSpeed;
        return fromDestination - rigid.velocity;
    }

    private Vector3[] hidingSpots()
    {
        Vector3[] spots = new Vector3[obstacles.Length];
        for (uint i = 0; i < obstacles.Length; ++i)
        {
            spots[i] = hidingSpot(obstacles[i].transform);
        }
        return spots;
    }

    private Vector3 hidingSpot(Transform obstacle)
    {
        float distanceAway = distanceFromBoundary + obstacle.lossyScale.z;
        Vector3 toObstacle = obstacle.position - hidingTransform.position;
        return (toObstacle.normalized * (distanceAway + toObstacle.magnitude)) + hidingTransform.position;
    }
}
