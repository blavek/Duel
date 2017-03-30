using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	Animator anim;
//	public Animation attackClip;

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
    public float jumpForce = 100f;
    public Reticle reticle;

    private GameObject lockOnTarget = null;
    private bool lockedOn = false;
    private Rigidbody rBody;
//    private bool grounded = true;

	float attackLen = .467f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
        rBody = GetComponent<Rigidbody> ();
		Debug.Log (anim.name /*["Attack"].length*/);
	}
	
	// Update is called once per frame
	void Update () {
        if (!lockedOn)
            moveNoTarget ();
        else
            moveTarget ();
//		attack ();
        playerInput ();

        if (!anim.GetBool ("Grounded"))
            jumpUpdate ();

//        stopJump ();
//		Debug.Log("Grounded: " + anim.GetBool("Grounded"));
	}

    void playerInput() {
		if (Input.GetButtonDown ("Attack")) {
			attack ();
		}
            
        if (Input.GetButtonDown ("Jump")) {
            jump ();
        }

        if (Input.GetButtonDown ("LockOn")) {
            if (!lockedOn)
                lockOn ();
            else
                lockOff ();
        }
    }

    void moveNoTarget() {
		float x = Input.GetAxis ("Horizontal");
		float y = Input.GetAxis ("Vertical");

//		if (Mathf.Abs(x) < .1f)
//			x = 0;

//		if (Mathf.Abs(y) < .1f)
//			y = 0;

		float translation = speed * (Mathf.Sqrt((x*x) + (y*y)));
		float rot = (Mathf.Atan2 (x, y) * Mathf.Rad2Deg) + transform.eulerAngles.y;
		rot = Mathf.LerpAngle (transform.eulerAngles.y, rot, rotationSpeed * Time.deltaTime);
//		rot = Camera.main.transform.TransformDirection(new Vector3 (0, rot, 0));

//        Debug.Log (rot + " x:" + x + " y:" + y);
		translation *= Time.deltaTime;
		transform.Translate(new Vector3(0, 0, translation));

        if (x != 0 || y != 0)
            transform.rotation = Quaternion.Euler (0, rot, 0);

		animate (translation);
	}

    void moveTarget() {
        float lx = Input.GetAxis ("Horizontal");
        float ly = Input.GetAxis ("Vertical");
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
            rBody.AddForce (Vector3.up * jumpForce);
			anim.SetBool ("Jump", true);
            anim.SetBool ("Idle", false);
//            anim.SetBool ("Run", false);
            anim.SetBool ("Grounded", false);
        }

    }

    void jumpUpdate() {
        anim.SetFloat ("AirVel", rBody.velocity.y);
        anim.SetFloat ("Height", rBody.position.y);            
    }

    void lockOn() {
        lockedOn = true;
        Debug.Log ("LOCKON");
        GameObject [] targets = GameObject.FindGameObjectsWithTag("Enemy");

        if (targets.Length == 0)
            return;

        if (lockOnTarget == null && targets.Length > 0) {
            lockOnTarget = targets [0];
        }

        transform.LookAt (lockOnTarget.transform);
        reticle.gameObject.SetActive(true);
        reticle.setTarget (lockOnTarget);

        //else if (LockOnTarget != null && targets.Length > 0)
        // if no target
        //     Get Closest enemy within some range
        // else
        //     get Next Closest enemy

        // face enemy
        // place target on enemy
    }

    void lockOff() {
        lockedOn = false;
        reticle.gameObject.SetActive(false);
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
