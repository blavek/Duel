using UnityEngine;
using System.Collections;

public class SpawnItem : MonoBehaviour {

	public float timer = 30.0f;
	float resetTime = 30.0f;
	public GameObject spawnable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			Instantiate (spawnable, gameObject.transform.position, gameObject.transform.rotation);
			timer = resetTime;
		}
	}
}
