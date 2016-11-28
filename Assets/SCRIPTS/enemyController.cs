using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {
	public float speed = 5f;
	public float chaseDist = 25f;
	public float attackDist = 2f;
	public int startingHp = 20;
	public GameObject self;
	int HP;

	Animator anim;
	GameObject pc;

	Vector3 relVec;
	float attackLen = .7f;
	float attacking = 0;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag ("pc");

		relVec = pc.transform.position - transform.position;
		anim = GetComponent<Animator> ();
		HP = startingHp;
	}

	// Update is called once per frame
	void Update () {
		relVec = pc.transform.position - transform.position;
		Debug.Log (": " + relVec.magnitude);
		if (attacking <= 0) {
			rotate ();
			move ();
			attack ();
		} else {
			attacking -= Time.deltaTime;
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
			float translation = speed * Time.deltaTime;
			transform.Translate (new Vector3 (0, 0, translation));
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
		Debug.Log ("Attacking");
		if (relVec.magnitude <= attackDist) {
			anim.SetBool ("Idle", false);
			anim.SetBool ("Attack", true);
			attacking = attackLen;
			Invoke ("stopAttack", attackLen);
		}
	}

	void stopAttack() {
		Debug.Log ("Stop Attacking");
		anim.SetBool ("Attack", false);
		anim.SetBool ("Idle", true);
	}

	public void damage (int dmg) {
		HP -= dmg;

		if (HP <= 0) {
			Instantiate (self, new Vector3 (0, 0, 0), Quaternion.identity);
			Destroy (gameObject);
		}
	}
}
