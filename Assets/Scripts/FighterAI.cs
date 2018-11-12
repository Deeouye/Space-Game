using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAI : MonoBehaviour {
    
    public Vector3 dest;
    private FighterBehavior fb;
    private Quaternion targetRotation;

	// Use this for initialization
	void Start () {
        fb = GetComponent<FighterBehavior>();
    }
	
	// Update is called once per frame
	void Update () {


        fb.FighterUpdate();
	}

    private void FixedUpdate()
    {
        Vector3 currEulerAngles, targetEulerAngles;
        
        currEulerAngles = transform.rotation.eulerAngles;
        fb.yawAmt = 0;
        fb.pitchAmt = 0;
         

        // Fly to point
        Vector3 direction = (dest - transform.position).normalized;
        targetRotation = Quaternion.LookRotation(direction);
        targetEulerAngles = targetRotation.eulerAngles;
        //targetEulerAngles = new Vector3(-15, 45, 0);
        float xdiff = Mathf.DeltaAngle(currEulerAngles.x, targetEulerAngles.x);
        if (Mathf.Abs(xdiff) > fb.pitchSpeed)
        {
            fb.pitchAmt = (xdiff > 0 ? 1 : -1);
        }

        float ydiff = Mathf.DeltaAngle(currEulerAngles.y, targetEulerAngles.y);
        if (Mathf.Abs(ydiff) > fb.turnSpeed)
        {
            fb.yawAmt = (ydiff > 0 ? 1 : -1);
        }

        fb.FighterFixedUpdate();
    }
}
