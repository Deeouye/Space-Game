using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalShipController : MonoBehaviour {

    public float speed = 200;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate () {
        rb.velocity = transform.forward * speed;
    }
}
