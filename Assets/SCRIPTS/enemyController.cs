using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {
	public float speed = 5f;
	public GameObject pc;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 relVec = pc.transform.position - transform.position;
		float rot = Mathf.Atan2 (relVec.x, relVec.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3 (0, rot, 0));
	}
}
