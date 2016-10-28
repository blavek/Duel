using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		move ();
	}

	void move() {
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		float translation = speed * (Mathf.Sqrt((x*x) + (y*y)));
		float rotation = 0;

		rotation = Mathf.Atan2 (x, y) * Mathf.Rad2Deg;
		translation *= Time.deltaTime;

		transform.rotation = Quaternion.Euler(0, rotation, 0);
		transform.Translate(new Vector3(0, 0, translation));
	}
}
