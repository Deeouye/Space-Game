using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAI : MonoBehaviour {

    private FighterBehavior fb;

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


        fb.FighterFixedUpdate();
    }
}
