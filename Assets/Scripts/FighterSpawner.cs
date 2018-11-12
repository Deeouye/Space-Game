using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSpawner : MonoBehaviour {

    public GameObject fighter;

    private float cooldown;
    private int counter;

	// Use this for initialization
	void Start () {
        cooldown = 0;
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        cooldown -= Time.deltaTime;
		if (cooldown <= 0 && counter < 1500)
        {
            GameObject newobj = Instantiate(fighter, transform);
            newobj.transform.parent = null;
            newobj.GetComponent<FighterAI>().dest = (Random.insideUnitSphere * 2000) + transform.position;
            newobj.GetComponent<FighterAI>().dest.z += 25000;
            //cooldown = .001f;
            counter++;
            Debug.Log(counter);
        }
	}
}
