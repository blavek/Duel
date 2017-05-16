using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class dragonBoss : MonoBehaviour {
    public int maxHp = 200;
    public Slider healthBar;
    public GameObject player;
    public float engageDist = 20f;
    public float attackDist = 10f;
    public float walkSpeed = 4f;
    public float bossDeathHeight = 10f;
    public float attackDelay = 6f;
    public ParticleSystem fireBreath;
    public ParticleSystem fireShot;
    public float dmg = 10f;
    public float rotSpeed = 100f;
    public Canvas victoryCan;

    private float tSinceAttack = 4f;
    private int hp;
    private Vector3 bossMove = Vector3.zero;
    private CharacterController cont;
    private bool alive = true;
    Animator anim;
    private Vector3 relVec;
    private bool hit = false;
    private bool attacking = false;
    private bool attacked = false;

    // Use this for initialization
    void Start () {
        relVec = new Vector3 (player.transform.position.x, 0, player.transform.position.z) - new Vector3 (transform.position.x, 0, transform.position.z);

        if (player == null)
            player = GameObject.FindGameObjectWithTag ("pc");
        
        cont = GetComponent<CharacterController> ();
        anim = GetComponent<Animator> ();

        healthBar.maxValue = maxHp;
        healthBar.minValue = 0;
        healthBar.value = maxHp;

        hp = maxHp;

        Transform[] bones = GetComponentsInChildren<Transform> ();

        foreach (Transform b in bones) {
            if (b.gameObject.GetComponent<BoxCollider> () != null)
                b.gameObject.AddComponent<boneAttacks> ();
        }
    }

    // Update is called once per frame
    void Update () {
        tSinceAttack += Time.deltaTime;

        if (alive) {
            relVec = new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            anim.SetFloat ("Velocity", cont.velocity.magnitude);

            if (relVec.magnitude <= engageDist && !healthBar.gameObject.activeSelf) {
                healthBar.gameObject.SetActive(true);
            }

            if (relVec.magnitude <= engageDist && relVec.magnitude > attackDist) {
                rotate ();
                move ();
            } else if (relVec.magnitude <= engageDist && relVec.magnitude <= attackDist) {
                dragonAttack ();
            }
        } else {
            cont.Move (bossMove);
            bossMove = bossMove + Physics.gravity * Time.deltaTime;
/*
            if (transform.position.y <= .1f) {
                bossDeathHeight *= .5f;
                bossMove = new Vector3 (bossDeathHeight * .1f, bossDeathHeight, bossDeathHeight * .1f);
            }
*/
        }
    }

    void move() {
        cont.SimpleMove (relVec.normalized * walkSpeed);
    }

    void rotate() {
        if (relVec.magnitude <= engageDist) {
            float rot = Mathf.Atan2 (relVec.x, relVec.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler (new Vector3 (0, rot, 0));
        }
    }

    void dragonAttack() {
        if (tSinceAttack >= attackDelay) {
            tSinceAttack = 0;
            switch (Random.Range (0, 4)) {
                case 0:
                    anim.SetTrigger ("BreathAttack");
                    fireBreath.Play ();
                    break;

                case 1:
                    anim.SetTrigger ("TailAttack");
                    attacking = true;
                    Invoke ("stopAttack", 1.567f);
                    break;

                case 2:
                    anim.SetTrigger ("ShotAttack");
                    fireBreath.Play ();
                    break;

                case 3:
                    anim.SetTrigger ("ClawAttack");
                    Invoke ("stopAttack", 1.16f);
                    attacking = true;
                    break;
            }
        }
    }

    void stopAttack() {
        attacking = false;
        attacked = false;
        anim.SetBool("Idle", true);
    }

    public bool getAttackState() {
        return (attacking);
    }

    public void damage(int damage) {
        anim.SetTrigger ("Damaged");
        hp -= damage;
        if (hp > 0)
            healthBar.value = hp;

        if (hp <= 0) {
            hp = 0;
            healthBar.value = 0;
            alive = false;
            bossMove = new Vector3 (bossDeathHeight * .1f, bossDeathHeight, bossDeathHeight * .1f);
            gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            Camera.main.GetComponent<cameraFollow> ().enabled = false;
            Camera.main.GetComponent<cameraDeath> ().enabled = true;
            anim.SetTrigger ("death");
            Invoke ("victory", 5);
        }
    }

    void victory() {
        victoryCan.gameObject.SetActive (true);
    }

    void OnCollisionEnter (Collision col) {
        if (getAttackState() && !attacked) {
            if (col.gameObject.GetComponent<Controller> () != null) {
                col.gameObject.GetComponent<Controller> ().damage (dmg);
                attacked = true;
            }
        }
    }

    public bool getAttacked() {
        return(attacked);
    }

    public void setAttacked() {
        attacked = true;
    }
}
