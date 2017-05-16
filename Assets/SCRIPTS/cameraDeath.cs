using UnityEngine;
using System.Collections;

public class cameraDeath : MonoBehaviour {
    public GameObject boss;
	// Use this for initialization

    void Update () {
        transform.LookAt (boss.transform);
	}
}
