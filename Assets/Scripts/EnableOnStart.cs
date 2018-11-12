using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class lets me have extremely large objects inactive in the editor but active in the game.
/// </summary>
public class EnableOnStart : MonoBehaviour
{

    void Start()
    {
        this.gameObject.SetActive(true);
    }
}
