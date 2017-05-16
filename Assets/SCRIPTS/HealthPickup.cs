using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour {

	public int healAmount = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col) {
		Controller cont = col.gameObject.GetComponent<Controller> ();
		if (cont != null) {
			cont.heal (healAmount);
			Destroy (gameObject);
		}
	}
}
