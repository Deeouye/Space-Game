using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public fields
    public float speed = 100;
    public float pitchSpeed = 1;
    public float turnSpeed = 1;
    public float fireRate = 4;
    public GameObject explosion;
    public GameObject bullet;
    public Transform cameraTransform;
    public Transform modelTransform;

    // Private fields
    private Rigidbody rb;
    private MeshCollider coll;

    private float vert;
    private float horiz;
    private float vertRaw;
    private float horizRaw;
    private bool moving;
    private float boost;
    private float secsSinceLastFire;
    private Quaternion targetRot, modelTargetRot;
    private Vector3 cameraTarget;
    private float targetFOV;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponentInChildren<MeshCollider>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Explosion testing
        
        if (Input.GetButtonDown("Fire1")) {
            GameObject temp;
            temp = Instantiate(explosion);
            temp.transform.parent = null;
            temp.transform.position = transform.position + (Random.insideUnitSphere * 20);
        }
        
        
        if (Input.GetButton("Fire1") && secsSinceLastFire > 1 / fireRate)
        {
            GameObject temp;
            temp = Instantiate(bullet, transform.position, transform.rotation);
            temp.transform.parent = null;
            secsSinceLastFire = 0;
        }

        secsSinceLastFire += Time.deltaTime;
    }
    void FixedUpdate()
    {
        vert = Input.GetAxis("Vertical");
        horiz = Input.GetAxis("Horizontal");
        vertRaw = Input.GetAxisRaw("Vertical");
        horizRaw = Input.GetAxisRaw("Horizontal");

        moving = horizRaw != 0 || vertRaw != 0;

        // Boost and brake
        boost = 1;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            boost *= 2f;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            boost /= 2;
        }


        // Constantly move forward. In space, if you stop moving, you die.
        rb.velocity = transform.forward * speed * boost;

        // Rotate base on controls
        transform.Rotate(vert * pitchSpeed, horiz * turnSpeed, 0);

        targetRot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime / (moving ? 5 : 2));


        // Camera and model movement
        cameraTarget = Vector3.zero;
        modelTargetRot = Quaternion.identity;
        if (horizRaw > 0)
        {
            cameraTarget += Vector3.right * 10 * horiz;
            modelTargetRot = Quaternion.Euler(0, 0, -30 * horiz);
        }
        else if (horizRaw < 0)
        {
            cameraTarget += Vector3.left * 10 * -horiz;
            modelTargetRot = Quaternion.Euler(0, 0, 30 * -horiz);
        }

        if (vertRaw > 0)
        {
            cameraTarget += Vector3.down * 6 * vert;
        }
        else if (vertRaw < 0)
        {
            cameraTarget += Vector3.up * 6 * -vert;
        }

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraTarget, Time.deltaTime * 2);
        modelTransform.localRotation = Quaternion.Lerp(modelTransform.localRotation, modelTargetRot, Time.deltaTime * 3);

        // Camera FOV based on boost
        targetFOV = 60;
        if (boost > 1)
        {
            targetFOV = 80;
        }
        else if (boost < 1)
        {
            targetFOV = 45;
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime * 2);
    }
}
