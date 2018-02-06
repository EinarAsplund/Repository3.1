using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOffPlat : MonoBehaviour {

    public GameObject ThePlayer;

    void OnTriggerEnter()
    {
        ThePlayer.transform.parent = null;
    }
}
