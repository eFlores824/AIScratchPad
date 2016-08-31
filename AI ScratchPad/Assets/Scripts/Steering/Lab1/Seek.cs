using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Seek : MonoBehaviour {

    public GameObject seeking;
    public float maxSpeed;
    public Vector3 initialVelocity;

    private Transform seekingTransform;
    private Transform theTransform;
    private Rigidbody rigid;


    // Use this for initialization
    void Start()
    {
        seekingTransform = seeking.GetComponent<Transform>();
        theTransform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = initialVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 steering = seek();
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

        theTransform.forward = rigid.velocity.normalized;
    }

    private Vector3 seek()
    {
        Vector3 fromDestination = seekingTransform.position - theTransform.position;
        fromDestination = fromDestination.normalized * maxSpeed;
        return fromDestination - rigid.velocity;
    }
}
