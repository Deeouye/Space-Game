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
		if (cooldown <= 0 && counter < 100)
        {
            GameObject newobj = Instantiate(fighter, transform);
            newobj.transform.parent = null;
            Vector3 dest = (Random.insideUnitSphere * 2000) + transform.position;
            dest.z += 25000;
            newobj.GetComponent<FighterAI>().SetDest(dest);
            //cooldown = .001f;
            counter++;
            //Debug.Log(counter);
        }
	}
}
