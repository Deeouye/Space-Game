using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBehavior : MonoBehaviour {

    // Public fields
    //public Transform modelTransform;
    public float speed = 100;
    public float pitchSpeed = 1;
    public float turnSpeed = 1;
    public float fireRate = 4;
    public int team = 1;
    public GameObject bullet;
    public GameObject explosion;

    [HideInInspector] public float pitchAmt, yawAmt;
    [HideInInspector] public float boost;
    [HideInInspector] public bool firing;

    // Private fields
    private Rigidbody rb;
    private MeshCollider coll;
    private bool moving;
    private float secsSinceLastFire;
    private Quaternion targetRot, modelTargetRot;


    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponentInChildren<MeshCollider>();
        boost = 1;
    }

    // Should be called after Update in controller script
    public void FighterUpdate ()
    {

        if (firing && secsSinceLastFire > 1 / fireRate)
        {
            GameObject temp;
            temp = Instantiate(bullet, transform.position + (transform.forward * 10), transform.rotation);
            temp.GetComponent<BulletBehavior>().team = team;
            temp.transform.parent = null;
            secsSinceLastFire = 0;
        }

        secsSinceLastFire += Time.deltaTime;
    }

    // Should be called after FixedUpdate in controller script
    public void FighterFixedUpdate () {

        // Constantly move forward. In space, if you stop moving, you die.
        rb.velocity = transform.forward * speed * boost;

        // Rotate base on controls
        transform.Rotate(pitchAmt * pitchSpeed, yawAmt * turnSpeed, 0);

        moving = Mathf.Abs(pitchAmt + yawAmt) > .2f;

        targetRot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime / (moving ? 5 : 2));


        modelTargetRot = Quaternion.Euler(0, 0, -30 * yawAmt);

    }
}
