using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public fields
    public GameObject explosion;
    public Transform cameraTransform;
    public Transform modelTransform;

    // Private fields
    private Rigidbody rb;
    private MeshCollider coll;
    private FighterBehavior fb;

    private float vertRaw;
    private float horizRaw;
    private bool moving;
    private float secsSinceLastFire;
    private Quaternion targetRot, modelTargetRot;
    private Vector3 cameraTarget;
    private float targetFOV;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponentInChildren<MeshCollider>();
        fb = GetComponent<FighterBehavior>();
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

        fb.firing = Input.GetButton("Fire1");

        fb.FighterUpdate();
    }
    void FixedUpdate()
    {
        fb.pitchAmt = Input.GetAxis("Vertical");
        fb.yawAmt = Input.GetAxis("Horizontal");
        vertRaw = Input.GetAxisRaw("Vertical");
        horizRaw = Input.GetAxisRaw("Horizontal");

        // Boost and brake
        fb.boost = 1;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            fb.boost *= 2f;
        }
        if (Input.GetKey(KeyCode.U))
        {
            fb.boost = 0;
        }


        // Camera and model movement
        cameraTarget = Vector3.zero;
        modelTargetRot = Quaternion.identity;
        if (horizRaw > 0)
        {
            cameraTarget += Vector3.right * 10 * fb.yawAmt;
            modelTargetRot = Quaternion.Euler(0, 0, -30 * fb.yawAmt);
        }
        else if (horizRaw < 0)
        {
            cameraTarget += Vector3.left * 10 * -fb.yawAmt;
            modelTargetRot = Quaternion.Euler(0, 0, 30 * -fb.yawAmt);
        }

        if (vertRaw > 0)
        {
            cameraTarget += Vector3.down * 6 * fb.pitchAmt;
        }
        else if (vertRaw < 0)
        {
            cameraTarget += Vector3.up * 6 * -fb.pitchAmt;
        }

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraTarget, Time.deltaTime * 2);
        modelTransform.localRotation = Quaternion.Lerp(modelTransform.localRotation, modelTargetRot, Time.deltaTime * 3);

        // Camera FOV based on boost
        targetFOV = 60;
        if (fb.boost > 1)
        {
            targetFOV = 80;
        }
        else if (fb.boost < 1)
        {
            targetFOV = 45;
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime * 2);

        fb.FighterFixedUpdate();
    }
}
