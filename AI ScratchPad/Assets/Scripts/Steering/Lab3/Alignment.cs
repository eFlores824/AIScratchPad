using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody))]
public class Alignment : MonoBehaviour {

    public float maxSpeed;
    public float neighborhoodRadius;
    public Vector3 initialVelocity;

    private Transform theTransform;
    private Rigidbody rigid;
    private Alignment[] others;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        others = GameObject.FindObjectsOfType<Alignment>();
        rigid.velocity = initialVelocity;
        theTransform.forward = initialVelocity.normalized;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 steering = align();
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

        if (rigid.velocity != Vector3.zero)
        {
            theTransform.forward = rigid.velocity.normalized;
        }
    }

    private Vector3 align()
    {
        Transform[] near = neighbors();
        Vector3 averageForward = new Vector3();
        for (uint i = 0; i < near.Length; ++i)
        {
            averageForward += near[i].forward;
        }
        averageForward = averageForward.normalized * maxSpeed;
        return averageForward - rigid.velocity;
    }

    private Transform[] neighbors()
    {
        List<Transform> near = new List<Transform>();
        for (uint i = 0; i < others.Length; ++i)
        {
            if (others[i] == this) continue;

            Vector3 targetPosition = others[i].transform.position;
            if ((targetPosition - theTransform.position).magnitude <= neighborhoodRadius)
            {
                near.Add(others[i].GetComponent<Transform>());
            }
        }
        return near.ToArray();
    }
}
