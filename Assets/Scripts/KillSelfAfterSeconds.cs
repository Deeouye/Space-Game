using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelfAfterSeconds : MonoBehaviour {

    public float seconds;

    private float elapsed;

	// Use this for initialization
	void Start () {
        elapsed = 0;
	}
	
	// Update is called once per frame
	void Update () {
        elapsed += Time.deltaTime;
        if (elapsed >= seconds)
        {
            Destroy(this.gameObject);
        }
	}
}
