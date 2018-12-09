using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAI : MonoBehaviour {

    public GameObject formationLayout;
    public FormationState formation_state;

    private FighterBehavior fb;
    private FormationLayout fl;

    private Quaternion targetRotation;
    private GameObject target;
    private Vector3 dest;
    private AiState ai_state;
    private bool facingTarget;
    private int followerPos;
    private float timer;

    enum AiState { CHARGING, FLEEING, ROAMING };
    public enum FormationState { LEADER, FOLLOWER, SEARCHING };

	// Use this for initialization
	void Start () {
        fb = GetComponent<FighterBehavior>();
        fl = formationLayout.GetComponent<FormationLayout>();
        ai_state = AiState.CHARGING;
        formation_state = FormationState.SEARCHING;
        FindTarget(float.MaxValue);
        if (target == null)
        {
            ai_state = AiState.ROAMING;
        }
    }
	
	// Update is called once per frame
	void Update () {
        fb.FighterUpdate();
        timer += Time.deltaTime;
	}

    private void FixedUpdate()
    {
        fb.firing = false;

        switch (ai_state)
        {
            case AiState.CHARGING:
                Charge();
                break;
            case AiState.FLEEING:
                Flee();
                break;
        }
        

        FlyToPoint();

        fb.FighterFixedUpdate();
    }

    public void SetDest (Vector3 value)
    {
        dest = value;
    }

    /// <summary>
    /// This will point the fighter towards 'dest'
    /// </summary>
    void FlyToPoint()
    {
        Vector3 currEulerAngles, targetEulerAngles;

        currEulerAngles = transform.rotation.eulerAngles;
        fb.yawAmt = 0;
        fb.pitchAmt = 0;
        facingTarget = true;

        Vector3 direction = (dest - transform.position).normalized;
        targetRotation = Quaternion.LookRotation(direction);
        targetEulerAngles = targetRotation.eulerAngles;
        float xdiff = Mathf.DeltaAngle(currEulerAngles.x, targetEulerAngles.x);
        if (Mathf.Abs(xdiff) > fb.pitchSpeed)
        {
            fb.pitchAmt = (xdiff > 0 ? 1 : -1);
            facingTarget = false;
        }

        float ydiff = Mathf.DeltaAngle(currEulerAngles.y, targetEulerAngles.y);
        if (Mathf.Abs(ydiff) > fb.turnSpeed)
        {
            fb.yawAmt = (ydiff > 0 ? 1 : -1);
            facingTarget = false;
        }
    }

    /// <summary>
    /// Charge at a chosen enemy fighter and fire when in range
    /// 'dest' will move with that fighter
    /// </summary>
    void Charge()
    {
        if (Vector3.Distance(transform.position, dest) < 50 || target == null || timer > 7f)
        {
            timer = 0;
            ai_state = AiState.FLEEING;
            dest = (Random.insideUnitSphere * 500) + transform.position;
            return;
        }

        SetDest(target.transform.position);
        fb.firing = facingTarget && Vector3.Distance(transform.position, dest) < 500;

    }

    /// <summary>
    /// Move to a given point
    /// </summary>
    void Flee()
    {
        if (Vector3.Distance(transform.position, dest) < 50 || timer > 5f)
        {
            timer = 0;
            FindTarget(float.MaxValue);
            if (target == null)
            {
                ai_state = AiState.ROAMING;
            }
            else
            {
                SetDest(target.transform.position);
                ai_state = AiState.CHARGING;
            }
        }
    }

    /// <summary>
    /// Fly to random points
    /// </summary>
    void Roam()
    {

    }

    /// <summary>
    /// Find a fighter within the given max distance
    /// </summary>
    /// <param name="maxDistance"></param>
    void FindTarget(float maxDistance)
    {
        // If target is null and FindTarget was called, we couldn't find any targets
        target = null;

        GameObject[] fighters;
        fighters = GameObject.FindGameObjectsWithTag("Fighter");
        float closest = maxDistance;
        foreach (GameObject fighter in fighters)
        {
            // Skip fighters on our team
            if (fighter.GetComponent<FighterBehavior>().team == fb.team) continue;

            // Get the closest
            float distance = Vector3.Distance(transform.position, fighter.transform.position);
            if (distance < closest)
            {
                closest = distance;
                target = fighter;
            }
        }
    }

    void FindFormationLeader(float maxDistance)
    {
        GameObject[] fighters;
        FighterAI leaderAi = null;
        fighters = GameObject.FindGameObjectsWithTag("Fighter");
        float closest = maxDistance;
        foreach (GameObject fighter in fighters)
        {
            FighterBehavior theirfb = fighter.GetComponent<FighterBehavior>();
            FighterAI fai = fighter.GetComponent<FighterAI>();
            // Skip fighters not on our team
            if (theirfb.team != fb.team) continue;

            // Get the closest
            float distance = Vector3.Distance(transform.position, fighter.transform.position);
            if (distance < closest && fai.formation_state == FormationState.SEARCHING)
            {
                closest = distance;
                leaderAi = fai;
            }
        }

        if (leaderAi != null)
        {
            leaderAi.formation_state = FormationState.LEADER;
            formation_state = FormationState.FOLLOWER;

            for (int i = 0; i < leaderAi.fl.positions.Length; i++)
            {
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (other.gameObject.GetComponentInParent<BulletBehavior>().team == fb.team)
            {
                return;
            }
            //Spawn an explosion and deparent it so it doens't die with us
            GameObject temp = Instantiate(fb.explosion, transform);
            temp.transform.parent = null;
            Destroy(this.gameObject);
        }
    }
}
