using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

    public GameObject[] hubPoints;
    public float secondsToTeleport;

    private Transform theTransform;
    private float timePassed = 0.0f;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        timePassed += Time.deltaTime;
        if (timePassed >= secondsToTeleport)
        {
            timePassed = 0.0f;
            randomTeleport();
        }
	}

    private void randomTeleport()
    {
        uint randomIndex = (uint)Random.Range(0.0f, hubPoints.Length);
        theTransform.position = hubPoints[randomIndex].transform.position;
    }
}
