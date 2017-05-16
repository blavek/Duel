using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
	Animator anim;
//	public Animation attackClip;

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
    public float jumpForce = 100f;
    public Reticle reticle;
	public float attackDistance = 2;
    public AudioClip[] slashes;
    public float maxHp = 200;
    public Canvas gameOverCan;


    public Slider healthBar;
    private GameObject lockOnTarget = null;
    private bool lockedOn = false;
    private Rigidbody rBody;
    private GameObject cam = null;
    private bool attacking = false;
    private AudioSource move;
    private AudioSource attackAs;
    private float velocity = 0;
    private float hp;
    private CharacterController charCont;
    private float gravity = 0;
    private Vector3 jumpHeight = Vector3.zero;
//    private bool grounded = true;

	float attackLen = .467f;

	// Use this for initialization
	void Start () {
        charCont = GetComponent<CharacterController> ();
		anim = GetComponent<Animator> ();
        rBody = GetComponent<Rigidbody> ();
        Debug.Log ("Gravity " + Physics.gravity);
        gravity = Physics.gravity.y;
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		Debug.Log (anim.name /*["Attack"].length*/);
        AudioSource[] audSource = GetComponents<AudioSource> ();
        if (healthBar == null)
            healthBar = GetComponentInChildren<Slider> ();

        if (gameOverCan == null) {
            Canvas [] cans = GameObject.FindObjectsOfType<Canvas> ();
            foreach(Canvas c in cans) {
                if (c.tag == "GameOver") {
                    gameOverCan = c;
                    break;
                }
            }
        }

        hp = maxHp;

        healthBar.maxValue = maxHp;
        healthBar.value = hp;
        healthBar.minValue = 0;

//        Debug.Log ("sources " + audSource [1].clip.name);
        if (audSource.Length >= 2) {
            if (audSource [0].clip != null) {
                if (audSource [0].clip.name == "FootSteps Sound (Play in Loop)") {
                    move = audSource [0];
                    attackAs = audSource [1];
                }
            } else {
                move = audSource [1];
                attackAs = audSource [0];
            }
        } else {
            move = audSource [0];
            attackAs = audSource [0];
        }

        move.pitch = velocity;
        move.Play ();
    }

	// Update is called once per frame
	void Update () {
		if (lockOnTarget == null) {
			lockOff ();
        } else if (lockOnTarget.GetComponent<enemyController>() != null) {
            if (!lockOnTarget.GetComponent<enemyController> ().isAlive ()) {
                lockOff ();
            }
        }

		if (!lockedOn)
            moveNoTarget ();
        else
            moveTarget ();
//		attack ();
        playerInput ();
        moveSound ();

        if ((!anim.GetBool ("Grounded") || anim.GetFloat ("Height") > 0) && jumpHeight.y > 0 )
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
		float x = Input.GetAxis ("MoveX");
		float y = Input.GetAxis ("MoveY");
        float mag = Mathf.Sqrt((x*x) + (y*y));
		float translation = speed * (mag);
		float joyRot = ((Mathf.Atan2 (x, y) * Mathf.Rad2Deg) + 360) % 360;
		float camRot = cam.transform.eulerAngles.y;
		float rot = (joyRot + camRot) % 360;

        anim.SetFloat ("Velocity", 1 + mag);
        velocity = mag;

        if (x != 0 || y != 0)
            transform.rotation = Quaternion.Euler (0, rot, 0);

        charCont.SimpleMove (transform.TransformDirection(Vector3.forward * translation));

        animate (translation);
	}

    void moveTarget() {
        float lx = Input.GetAxis ("MoveX");
        float ly = Input.GetAxis ("MoveY");

        velocity = Mathf.Sqrt ((lx * lx) + (ly * ly));
        face (lockOnTarget);
		float yTranslation = speed * ly * Time.deltaTime;
        float xTranslation = speed * lx * Time.deltaTime;

		if (Vector3.Distance (transform.position, lockOnTarget.transform.position) < attackDistance && yTranslation > 0)
			yTranslation = 0;

        charCont.SimpleMove (transform.TransformDirection(Vector3.forward * speed * ly) + transform.TransformDirection(Vector3.right * speed * lx));
		float translation = xTranslation > yTranslation ? xTranslation : yTranslation;

        animate (translation);
    }

    void animate (float translation) {
		if (anim != null) {
			anim.SetFloat ("Moving", translation);
//            Animation anim["Running"].speed = translation

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
        anim.SetTrigger ("Attack");
        attacking = true;
		Invoke ("stopAttack", attackLen);

        int slashIndex = Random.Range (0, slashes.Length);
        attackAs.PlayOneShot (slashes [slashIndex]);
	}

    void jump () {
        if (anim.GetBool("Grounded")) {
            jumpHeight.y = jumpForce;
            charCont.Move (jumpHeight * Time.deltaTime);

            anim.SetBool ("Jump", true);
            anim.SetBool ("Idle", false);
            anim.SetBool ("Grounded", false);
        }
    }

    void jumpUpdate() {
        jumpHeight.y += gravity * Time.deltaTime;
        charCont.Move (jumpHeight * Time.deltaTime);
        anim.SetFloat ("AirVel", charCont.velocity.y);
        float height = rBody.position.y;

        RaycastHit hit;
        if (Physics.Raycast (transform.position, Vector3.down, out hit)) {
            Debug.Log ("Ground be " + hit.distance);
            height = hit.distance;
        } else {
            Debug.Log ("No Ground");
            height = 0;
        }

        anim.SetFloat ("Height", height);
    }

    void lockOn() {
        lockedOn = true;

        List<GameObject> targets = new List<GameObject>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject g in enemies) {
            if (g.GetComponent<enemyController> () != null) {
                if (g.GetComponent<enemyController> ().isAlive ()) {
                    targets.Add (g);
                }
            } else {
                targets.Add (g);
            }
            
        }

        if (targets.Count == 0)
            return;

        targets = targets.OrderBy (x => Vector3.Distance (this.transform.position, x.transform.position)).ToList ();

        if (targets.Count > 0) {
            lockOnTarget = targets [0];
        }

        reticle.setTarget (lockOnTarget);
        reticle.gameObject.SetActive(true);
        face (lockOnTarget);
    }

    void lockOff() {
        lockedOn = false;
        reticle.gameObject.SetActive(false);
    }

	void stopAttack() {
		anim.SetBool ("Idle", true);
        attacking = false;
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

    void moveSound() {
        if (velocity > 0 && anim.GetBool("Grounded"))
            move.pitch = 1 + velocity;
        else
            move.pitch = 0;
    }

    public float getVelocity() {
        return velocity;
    }

    public void heal(int health) {
        hp += health;

        if (hp > maxHp)
            hp = maxHp;

        healthBar.value = hp;
    }

    public void damage(float dmg) {
        hp -= dmg;

        if (hp <= 0)
            gameOver ();

        healthBar.value = hp;
    }

    public void gameOver() {
        healthBar.gameObject.SetActive(false);
        gameOverCan.gameObject.SetActive(true);
        GetComponent<CharacterController> ().enabled = false;
        GetComponent<Rigidbody> ().freezeRotation = false;
        GetComponent<Controller>().enabled = false;
        // Do game over shit
    }

    void OnCollisionEnter(Collision col) {
        //        Debug.Log ("THE GROUND I AM ON IT");
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
