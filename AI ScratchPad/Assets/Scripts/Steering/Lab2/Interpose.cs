using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class Interpose : MonoBehaviour {

    public GameObject firstObject;
    public GameObject secondObject;

    public float maxSpeed;

    private Transform theTransform;
    private Transform firstTransform;
    private Transform secondTransform;
    private Rigidbody rigid;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        firstTransform = firstObject.GetComponent<Transform>();
        secondTransform = secondObject.GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 steering = interpose();
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

        theTransform.forward = rigid.velocity.normalized;
	}

    private Vector3 interpose()
    {
        Vector3 target = Vector3.Lerp(firstTransform.position, secondTransform.position, 0.5f);
        Debug.DrawRay(theTransform.position, target - theTransform.position);
        return seek(target);
    }

    private Vector3 seek(Vector3 targetPosition)
    {
        Vector3 fromDestination = targetPosition - theTransform.position;
        fromDestination = fromDestination.normalized * maxSpeed;
        return fromDestination - rigid.velocity;
    }
}
