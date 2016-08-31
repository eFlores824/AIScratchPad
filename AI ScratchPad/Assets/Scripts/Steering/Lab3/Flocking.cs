using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody))]
public class Flocking : MonoBehaviour {

    public float maxSpeed;
    public float neighborhoodRadius;
    public Vector3 initialVelocity;
    public GameObject destination;

    public float alignmentWeight;
    public float cohesionWeight;
    public float seperationWeight;

    private Transform theTransform;
    private Rigidbody rigid;
    private Flocking[] others;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        others = GameObject.FindObjectsOfType<Flocking>();
        rigid.velocity = initialVelocity.normalized * maxSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 steering = flock();
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity += seek(destination.transform.position) / rigid.mass;
        rigid.velocity = rigid.velocity.normalized * maxSpeed;//Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

        if (rigid.velocity != Vector3.zero)
        {
            theTransform.forward = rigid.velocity.normalized;
        }
    }

    private Vector3 flock()
    {
        Transform[] near = neighbors();
        Vector3 alignment = align(near).normalized * alignmentWeight;
        Vector3 cohesion = cohese(near).normalized * cohesionWeight;
        Vector3 seperation = seperate(near).normalized * seperationWeight;
        Vector3 addition = (alignment + cohesion + seperation).normalized * maxSpeed;
        return addition;
    }

    private Vector3 align(Transform[] near)
    {
        Vector3 averageForward = new Vector3();
        for (uint i = 0; i < near.Length; ++i)
        {
            averageForward += near[i].forward;
        }
        averageForward = averageForward.normalized * maxSpeed;
        return averageForward - rigid.velocity;
    }

    private Vector3 cohese(Transform[] near)
    {
        Vector3 averagePosition = new Vector3();
        for (uint i = 0; i < near.Length; ++i)
        {
            averagePosition += near[i].position;
        }
        averagePosition /= near.Length;
        return seek(averagePosition);
    }

    private Vector3 seperate(Transform[] near)
    {
        Vector3 totalVector = new Vector3();
        for (uint i = 0; i < near.Length; ++i)
        {
            totalVector += theTransform.position - near[i].position;
        }
        return totalVector;
    }

    private Vector3 seek(Vector3 targetPosition)
    {
        Vector3 fromDestination = targetPosition - theTransform.position;
        fromDestination = fromDestination.normalized * maxSpeed;
        return fromDestination - rigid.velocity;
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
