using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Controller : MonoBehaviour {
	Animator anim;
//	public Animation attackClip;

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
    public float jumpForce = 100f;
    public Reticle reticle;
	public float attackDistance = 2;

    private GameObject lockOnTarget = null;
    private bool lockedOn = false;
    private Rigidbody rBody;
    private GameObject cam = null;
    private bool attacking = false;
//    private bool grounded = true;

	float attackLen = .467f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
        rBody = GetComponent<Rigidbody> ();
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		Debug.Log (anim.name /*["Attack"].length*/);
	}
	
	// Update is called once per frame
	void Update () {
		if (lockOnTarget == null) {
			lockOff ();
		}

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
			Debug.Log ("JUMP");
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
		float joyRot = ((Mathf.Atan2 (x, y) * Mathf.Rad2Deg) + 360) % 360;
		float camRot = cam.transform.eulerAngles.y;
		float rot = (joyRot + camRot) % 360;

//		Vector3 rot = y * cam.transform.forward + x * cam.transform.right;


//		rot = Mathf.LerpAngle (transform.eulerAngles.y, rot, rotationSpeed * Time.deltaTime);
//		rot = Camera.main.transform.TransformDirection(new Vector3 (0, rot, 0));

//        Debug.Log ("CamRot: " + camRot + "JoyRot: " + joyRot + " x:" + x + " y:" + y);

		translation *= Time.deltaTime;
		transform.Translate(new Vector3(0, 0, translation));

        if (x != 0 || y != 0)
            transform.rotation = Quaternion.Euler (0, rot, 0);

		animate (translation);
	}

    void moveTarget() {
        float lx = Input.GetAxis ("Horizontal");
        float ly = Input.GetAxis ("Vertical");

/*
        Vector3 targetDir = lockOnTarget.transform.position - transform.position;
        targetDir.Set (targetDir.x, 0, targetDir.z);
        float angle = Vector3.Angle(targetDir, transform.forward);
        transform.Rotate (new Vector3 (0, angle, 0));
*/
        face (lockOnTarget);
//		transform.LookAt (lockOnTarget.transform);
//		transform.rotation = new Quaternion (0, transform.rotation.y, 0, 0);

		float yTranslation = speed * ly * Time.deltaTime;

		if (Vector3.Distance (transform.position, lockOnTarget.transform.position) < attackDistance && yTranslation > 0)
			yTranslation = 0;
		
		float xTranslation = speed * lx * Time.deltaTime;

        transform.Translate(new Vector3(xTranslation, 0, yTranslation));

		float translation = xTranslation > yTranslation ? xTranslation : yTranslation;

        animate (translation);
//        face (lockOnTarget);
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
		anim.SetBool ("Idle", false);
        anim.SetBool ("Attack", true);
        attacking = true;
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

        List<GameObject> targets = new List<GameObject>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject g in enemies) {
            targets.Add (g);
        }

        if (targets.Count == 0)
            return;

        targets = targets.OrderBy (x => Vector3.Distance (this.transform.position, x.transform.position)).ToList ();

        if (targets.Count > 0) {
            lockOnTarget = targets [0];
        }

        face (lockOnTarget);

        reticle.gameObject.SetActive(true);
        reticle.setTarget (lockOnTarget);
    }

    void lockOff() {
        lockedOn = false;
        reticle.gameObject.SetActive(false);
    }

	void stopAttack() {
		anim.SetBool ("Attack", false);
		anim.SetBool ("Idle", true);
        attacking = false;
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

    public bool getAttackState() {
        return (attacking);
    }

    void face(GameObject tar) {
        Vector3 lookPos = tar.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation; //Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }
}
