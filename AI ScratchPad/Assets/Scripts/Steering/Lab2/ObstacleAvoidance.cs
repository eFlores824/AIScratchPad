using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(BoxCollider))]
public class ObstacleAvoidance : MonoBehaviour {

    public float maxSpeed;
    public Vector3 initialVelocity;
    public GameObject[] destinations;

    private Transform theTransform;
    private Vector3 currentDestination;
    private Rigidbody rigid;
    private BoxCollider box;

    private int currentIndex;
    private bool nextDestination = false;
    private bool steeringApplied = false;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        rigid.velocity = initialVelocity;
        currentDestination = destinations[0].transform.position;
        currentIndex = 0;
	}

    void Update()
    {
        steeringApplied = false;
    }

    void LateUpdate()
    {
        if (!steeringApplied)
        {
            Vector3 steering = seek();
            steering /= rigid.mass;
            rigid.velocity += steering;
            rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

        }
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
        theTransform.forward = rigid.velocity.normalized;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 1)
        {
            nextDestination = collision.gameObject == destinations[currentIndex];
        }
    }

    private Vector3 seek()
    {
        Vector3 fromDestination = currentDestination - theTransform.position;
        fromDestination = fromDestination.normalized * maxSpeed;
        return fromDestination - rigid.velocity;
    }

    void OnTriggerStay(Collider collider)
    {
        Vector3 steering = avoid(collider.gameObject.transform.position);
        steeringApplied = true;
        applySteering(steering);
    }

    private void applySteering(Vector3 steering)
    {
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);
    }

    private Vector3 avoid(Vector3 avoiding)
    {
        Vector3 forwardCollision = theTransform.position + theTransform.forward * box.bounds.extents.z * 2;
        Debug.DrawRay(forwardCollision, forwardCollision - avoiding, Color.green);
        return forwardCollision - avoiding;
    }
}
