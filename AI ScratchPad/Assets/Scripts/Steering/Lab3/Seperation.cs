using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody))]
public class Seperation : MonoBehaviour {

    public float maxSpeed;
    public float neighborhoodRadius;
    public Vector3 initialVelocity;

    private Transform theTransform;
    private Rigidbody rigid;
    private Seperation[] others;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = initialVelocity;
        others = GameObject.FindObjectsOfType<Seperation>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 steering = seperate();
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

        if (rigid.velocity != Vector3.zero)
        {
            theTransform.forward = rigid.velocity.normalized;
        }
    }

    private Vector3 seperate()
    {
        Vector3[] nearby = neighbors();
        Vector3 totalVector = new Vector3();
        for (uint i = 0; i < nearby.Length; ++i)
        {
            totalVector += theTransform.position - nearby[i];
        }
        return totalVector;
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
