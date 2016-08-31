using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class Straight : MonoBehaviour {

    public float speed;
    public Vector3 direction;

    private Rigidbody rigid;
    private Transform theTransform;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        theTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        rigid.velocity = direction.normalized * speed;
        theTransform.forward = rigid.velocity.normalized;
	}
}
