using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour {

    public float speed;
    private Transform theTransform;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        controls();
    }

    private void controls()
    {
        Vector3 forwardVector = theTransform.forward * Time.deltaTime * speed;
        Vector3 rightVector = theTransform.right * Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.W))
        {
            theTransform.position += forwardVector;
        }
        if (Input.GetKey(KeyCode.A))
        {
            theTransform.position += -rightVector;
        }
        if (Input.GetKey(KeyCode.S))
        {
            theTransform.position += -forwardVector;
        }
        if (Input.GetKey(KeyCode.D))
        {
            theTransform.position += rightVector;
        }
    }
}
