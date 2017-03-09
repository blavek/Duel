﻿using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	Animator anim;
//	public Animation attackClip;

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
    public float jumpForce = 100f;

//    private bool grounded = true;

	float attackLen = .467f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		Debug.Log (anim.name /*["Attack"].length*/);
	}
	
	// Update is called once per frame
	void Update () {
		move ();
//		attack ();
        playerInput ();
//        stopJump ();
		Debug.Log("Grounded: " + anim.GetBool("Grounded"));
	}

    void playerInput() {
		if (Input.GetButtonDown ("Attack")) {
			attack ();
		}
            
        if (Input.GetButtonDown ("Jump")) {
            jump ();
        }
    }

    void move() {
		float x = Input.GetAxis ("Horizontal");
		float y = Input.GetAxis ("Vertical");

//		if (Mathf.Abs(x) < .1f)
//			x = 0;

//		if (Mathf.Abs(y) < .1f)
//			y = 0;

		float translation = speed * (Mathf.Sqrt((x*x) + (y*y)));
        float rot = Mathf.Atan2 (x, y) * Mathf.Rad2Deg;

//        Debug.Log (rot + " x:" + x + " y:" + y);

        if (x != 0 || y != 0)
            transform.rotation = Quaternion.Euler (0, rot, 0);

		translation *= Time.deltaTime;
		transform.Translate(new Vector3(0, 0, translation));

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
		anim.SetBool ("Idle", false);
        anim.SetBool ("Attack", true);
		Invoke ("stopAttack", attackLen);

	}

    void jump () {
        if (anim.GetBool("Grounded")) {
            GetComponent<Rigidbody> ().AddForce (Vector3.up * jumpForce);
			anim.SetBool ("Jump", true);
            anim.SetBool ("Idle", false);
//            anim.SetBool ("Run", false);
            anim.SetBool ("Grounded", false);
        }

    }

	void stopAttack() {
		anim.SetBool ("Attack", false);
		anim.SetBool ("Idle", true);
	}

    void OnCollisionEnter(Collision col) {
        foreach (ContactPoint con in col.contacts) {
            if (con.otherCollider.tag == "Ground") {
                anim.SetBool ("Grounded", true);
				anim.SetBool ("Jump", false);
            }
        }
    }

    void OnCollisionExit(Collision col) {
        foreach (ContactPoint con in col.contacts) {
            if (con.otherCollider.tag == "Ground") {
                anim.SetBool("Grounded",false);
            }
        }
    }
}
