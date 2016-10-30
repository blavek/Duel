using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	Animator anim;
//	public Animation attackClip;

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;

	float attackLen = .467f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		Debug.Log (anim.name /*["Attack"].length*/);
	}
	
	// Update is called once per frame
	void Update () {
		move ();
		attack ();
	}

	void move() {
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		if (Mathf.Abs(x) < .1f)
			x = 0;

		if (Mathf.Abs(y) < .1f)
			y = 0;

		float translation = speed * (Mathf.Sqrt((x*x) + (y*y)));
		float rot = Mathf.Atan2 (x, y) * Mathf.Rad2Deg;

		translation *= Time.deltaTime;
		transform.Translate(new Vector3(0, 0, translation));

		if (Mathf.Abs (rot) > 0.1f)
			transform.rotation = Quaternion.Euler (0, rot, 0);

		animate (translation);
	}

	void animate (float translation) {
		if (anim != null) {
			anim.SetFloat ("Moving", translation);

			if (translation != 0) {
				anim.SetBool ("Idle", false);
			} else {
				anim.SetBool ("Idle", true);
			}

		} else
			Debug.Log("No animator");
	}

	void attack() {
//		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !Input.GetButtonDown ("Fire1")) {
//			anim.SetBool ("Attack", false);
//			anim.SetBool ("Idle", true);
//		} else 
		if (Input.GetButtonDown ("Fire1")) {
			anim.SetBool ("Attack", true);
			anim.SetBool ("Idle", false);
			Invoke ("stopAttack", attackLen);
		}
	}

	void stopAttack() {
		anim.SetBool ("Attack", false);
		anim.SetBool ("Idle", true);
	}
}
