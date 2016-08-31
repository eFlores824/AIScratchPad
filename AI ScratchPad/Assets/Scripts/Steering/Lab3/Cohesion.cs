using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody))]
public class Cohesion : MonoBehaviour {

    public float maxSpeed;
    public float neighborhoodRadius;
    public Vector3 initialVelocity;

    private Transform theTransform;
    private Rigidbody rigid;
    private Cohesion[] others;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = initialVelocity;
        others = GameObject.FindObjectsOfType<Cohesion>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 steering = cohese();
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

        if (rigid.velocity != Vector3.zero)
        {
            theTransform.forward = rigid.velocity.normalized;
        }
    }

    private Vector3 cohese()
    {
        Vector3[] near = neighbors();
        Vector3 averagePosition = new Vector3();
        for (uint i = 0; i < near.Length; ++i)
        {
            averagePosition += near[i];
        }
        averagePosition /= near.Length;
        return seek(averagePosition);
    }

    private Vector3 seek(Vector3 targetPosition)
    {
        Vector3 fromDestination = targetPosition - theTransform.position;
        fromDestination = fromDestination.normalized * maxSpeed;
        return fromDestination - rigid.velocity;
    }

    private Vector3[] neighbors()
    {
        List<Vector3> near = new List<Vector3>();
        for (uint i = 0; i < others.Length; ++i)
        {
            if (others[i] == this) continue;

            Vector3 targetPosition = others[i].transform.position;
            if ((targetPosition - theTransform.position).magnitude <= neighborhoodRadius)
            {
                near.Add(targetPosition);
            }
        }
        return near.ToArray();
    }
}
