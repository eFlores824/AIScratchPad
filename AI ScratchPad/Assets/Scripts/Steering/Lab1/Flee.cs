using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Flee : MonoBehaviour {

    public GameObject fleeingFrom;
    public float speed;
    public Vector3 initialVelocity;

    private Transform fleeingTransform;
    private Transform theTransform;
    private Rigidbody rigid;

	// Use this for initialization
	void Start () {
        fleeingTransform = fleeingFrom.GetComponent<Transform>();
        theTransform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = initialVelocity;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 steering = flee();
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = rigid.velocity.normalized * speed;

        theTransform.forward = rigid.velocity.normalized;
    }

    private Vector3 flee()
    {
        Vector3 fromDestination = theTransform.position - fleeingTransform.position;
        fromDestination = fromDestination.normalized * speed;
        return fromDestination - rigid.velocity;
    }
}
