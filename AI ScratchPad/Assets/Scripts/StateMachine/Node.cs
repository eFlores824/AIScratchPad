using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

    public Node[] connections;
    public float distance;
    public Node origin;

    public Transform theTransform;

    void Start()
    {
        theTransform = GetComponent<Transform>();
    }
}
