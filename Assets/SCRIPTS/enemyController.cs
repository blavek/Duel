using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class enemyController : MonoBehaviour {
	public float speed = 5f;
	GameObject pc;
	public float chaseDist = 25f;
	public float attackDist = 2f;
	public int startingHp = 20;
	public GameObject self;
	int HP;
    public Slider healthBar;
    bool alive;
    public float deathAnimLength = 3f;
    public HealthPickup vial;
    public float dropPercentage = .2f;

	Animator anim;

	Vector3 relVec;
	float attackLen = .533f;
	bool attacking = false;
    private bool drops = true;

    private CharacterController charCont;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag ("pc");
		relVec = pc.transform.position - transform.position;
		anim = GetComponent<Animator> ();
		HP = startingHp;
        healthBar.maxValue = startingHp;
        healthBar.minValue = 0;
        healthBar.value = HP;
        alive = true;
        charCont = GetComponent<CharacterController> ();
        if (dropPercentage > 1 || dropPercentage < 0)
            dropPercentage = 0;            

        if (vial == null) {
            drops = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isAlive ()) {
            relVec = pc.transform.position - transform.position;
            if (!attacking) {
                rotate ();
                move ();
                attack ();
            }
        }
	}

	void rotate() {
		if (relVec.magnitude <= chaseDist) {
			float rot = Mathf.Atan2 (relVec.x, relVec.z) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (new Vector3 (0, rot, 0));
		}
	}

	void move() {
		if (relVec.magnitude <= chaseDist && relVec.magnitude > attackDist) {
            float translation = speed;// * Time.deltaTime;
            charCont.SimpleMove (transform.TransformDirection(Vector3.forward * translation));
			//transform.Translate (new Vector3 (0, 0, translation));
			anim.SetBool ("Idle", false);
			anim.SetFloat ("Move", translation);
		} else if (relVec.magnitude > chaseDist && relVec.magnitude > attackDist) {
			anim.SetBool ("Idle", true);
			anim.SetFloat ("Move", 0);
		} else {
			anim.SetFloat ("Move", 0);
		}
	}

	void attack() {
		if (relVec.magnitude <= attackDist) {
			anim.SetBool ("Idle", false);
			anim.SetBool ("Attack", true);
            attacking = true; //attackLen;
			Invoke ("stopAttack", attackLen);
		}
	}

	void stopAttack() {
        attacking = false;
		anim.SetBool ("Attack", false);
		anim.SetBool ("Idle", true);
	}

	public void damage (int dmg) {
		HP -= dmg;
        healthBar.value = HP;
        anim.SetTrigger ("Damaged");

		if (HP <= 0) {
            gameObject.GetComponent<Animator>().enabled = false;

//            healthBar.enabled = false;
            if (healthBar.GetComponentInParent<Canvas> () != null)
                healthBar.GetComponentInParent<Canvas> ().gameObject.SetActive (false);

            Invoke ("stopRD", deathAnimLength);
            gameObject.GetComponent<CapsuleCollider> ().enabled = false;
            charCont.enabled = false;

            if (drops && Random.value < dropPercentage)
                Instantiate (vial, gameObject.transform.position, Random.rotation);

            alive = false;
		}
	}

    private void stopRD() {
        Rigidbody[] rb = gameObject.GetComponentsInChildren<Rigidbody> ();

        foreach (Rigidbody r in rb) {
            r.isKinematic = true;
        }
    }

    public bool getAttackState() {
        return (attacking);
    }

    public bool isAlive() {
        return (alive);
    }
}
