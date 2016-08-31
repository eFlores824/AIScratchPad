using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class Evade : MonoBehaviour {

    public GameObject evading;
    public float speed;
    public float secondsToPredict;
    public float enemyMaxSpeed;
    public Vector3 initialVelocity;

    private Rigidbody rigid;
    private Rigidbody evadingRigid;
    private Transform theTransform;
    private Transform evadingTransform;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        evadingRigid = evading.GetComponent<Rigidbody>();
        theTransform = GetComponent<Transform>();
        evadingTransform = evading.GetComponent<Transform>();
        rigid.velocity = initialVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 steering = evade();
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = rigid.velocity.normalized * speed;

        theTransform.forward = rigid.velocity.normalized;
    }

    private Vector3 evade()
    {
        Vector3 prediction = evadingTransform.position + evadingRigid.velocity * secondsToPredict;
        Debug.DrawRay(theTransform.position, (prediction - theTransform.position).normalized * 3, Color.green);
        return flee(prediction);
    }

    private Vector3 flee(Vector3 targetPosition)
    {
        Vector3 fromDestination = theTransform.position - targetPosition;
        fromDestination = fromDestination.normalized * speed;
        return fromDestination - rigid.velocity;
    }
}
