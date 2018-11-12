using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {

    private CapsuleCollider col;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        col = GetComponentInChildren<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * 1500, ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
