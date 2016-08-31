using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrival : MonoBehaviour {

    public GameObject seeking;
    public float maxSpeed;
    public float slowingRadius;
    public Vector3 initialVelocity;

    private Transform seekingTransform;
    private Transform theTransform;
    private Rigidbody rigid;

    // Use this for initialization
    void Start () {
        theTransform = GetComponent<Transform>();
        seekingTransform = seeking.GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = initialVelocity.normalized * maxSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 steering = arrival();
        Debug.DrawRay(theTransform.position, steering, Color.red);
        if (steering.magnitude > slowingRadius / 50.0f)
        {
            steering /= rigid.mass;
        }        
        rigid.velocity += steering;

        theTransform.forward = rigid.velocity.normalized;
    }

    private Vector3 arrival()
    {
        Vector3 fromDestination = seekingTransform.position - theTransform.position;
        float speed;
        if (fromDestination.magnitude < slowingRadius)
        {
            speed = maxSpeed * (fromDestination.magnitude / slowingRadius);
        }
        else
        {
            speed =  maxSpeed;
        }
        Vector3 desiredVelocity = (speed / fromDestination.magnitude) * fromDestination;
        Debug.DrawRay(theTransform.position, desiredVelocity);
        return desiredVelocity - rigid.velocity;
    }
}
