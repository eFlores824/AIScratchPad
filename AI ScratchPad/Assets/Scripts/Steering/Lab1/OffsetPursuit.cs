using UnityEngine;
using System.Collections;

public class OffsetPursuit : MonoBehaviour {

    public GameObject pursuing;
    public float speed;
    public float enemyMaxSpeed;
    public Vector3 initialVelocity;
    public Vector3 offset;
    public float secondsToPredict;

    private Rigidbody rigid;
    private Rigidbody pursuingRigid;
    private Transform theTransform;
    private Transform pursueTransform;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        pursuingRigid = pursuing.GetComponent<Rigidbody>();
        theTransform = GetComponent<Transform>();
        pursueTransform = pursuing.GetComponent<Transform>();
        rigid.velocity = initialVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 steering = offsetPursuit();
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = rigid.velocity.normalized * speed;

        theTransform.forward = rigid.velocity.normalized;
    }

    private Vector3 offsetPursuit()
    {
        Vector3 expectedPosition = (pursueTransform.position + offset) + pursuingRigid.velocity * secondsToPredict;
        Debug.DrawRay(theTransform.position, (expectedPosition - theTransform.position).normalized * 3, Color.green);
        return seek(expectedPosition);
    }

    private Vector3 seek(Vector3 targetPosition)
    {
        Vector3 fromDestination = targetPosition - theTransform.position;
        fromDestination = fromDestination.normalized * speed;
        return fromDestination - rigid.velocity;
    }
}
