using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class PathFollowing : MonoBehaviour {

    public GameObject[] destinations;
    public float maxSpeed;
    public Vector3 initialVelocity;

    private Transform theTransform;
    private Rigidbody rigid;

    private uint currentIndex = 0;
    private Transform currentDestTransform;
    private bool nextDestination = false;

    // Use this for initialization
    void Start () {
        currentDestTransform = destinations[0].GetComponent<Transform>();
        theTransform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = initialVelocity.normalized * maxSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 steering = seek();
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

        theTransform.forward = rigid.velocity.normalized;
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
            currentDestTransform = destinations[currentIndex].GetComponent<Transform>();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        nextDestination = collision.gameObject == destinations[currentIndex];
    }

    private Vector3 seek()
    {
        Vector3 fromDestination = currentDestTransform.position - theTransform.position;
        fromDestination = fromDestination.normalized * maxSpeed;
        return fromDestination - rigid.velocity;
    }
}
