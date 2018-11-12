using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBehavior : MonoBehaviour {

    // Public fields
    public Transform modelTransform;
    public float speed = 100;
    public float pitchSpeed = 1;
    public float turnSpeed = 1;
    public float fireRate = 4;

    [HideInInspector] public bool boosting, braking;
    [HideInInspector] public float pitchAmt, yawAmt;

    // Private fields
    private Rigidbody rb;
    private MeshCollider coll;
    private bool moving;
    private float boost;
    private float secsSinceLastFire;
    private Quaternion modelTargetRot;


    // Use this for initialization
    void Start ()
    {
        // Move this to class shared with player? probably...
        rb = GetComponent<Rigidbody>();
        coll = GetComponentInChildren<MeshCollider>();
    }

    // Should be called after Update in controller script
    public void FighterUpdate ()
    {

    }

    // Should be called after FixedUpdate in controller script
    public void FighterFixedUpdate () {

        // Boost and brake
        boost = 1;
        if (boosting)
        {
            boost *= 2;
        }
        if (braking)
        {
            boost /= 2;
        }


        // Constantly move forward. In space, if you stop moving, you die.
        rb.velocity = transform.forward * speed * boost;


        modelTargetRot = Quaternion.Euler(0, 0, -30 * yawAmt);

    }
}
