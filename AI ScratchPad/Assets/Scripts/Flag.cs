using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {

    public bool captured = false;
    public Transform theTransform;

    void Start()
    {
        theTransform = GetComponent<Transform>();
    }
    
}
