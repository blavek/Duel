using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BirdController : MonoBehaviour {
    public float maxHeight = 5f;
    public float climbVel = 1f;
    public float swapSecs = 30f;
    public int maxHp = 20;
    private int hp;
    public Slider healthBar;

    private float height = 0f;
    private float timeSinceSwap = 0f;
    private bool flying = true;

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator> ();
        height = maxHeight;

        hp = maxHp;

        healthBar.maxValue = maxHp;
        healthBar.value = hp;

        if (anim != null)
            anim.SetBool ("Flying", true);
        else
            Debug.Log ("No Animator");
	}
	
	// Update is called once per frame
	void Update () {
        flightManager ();
	}

    void flightManager() {
        timeSinceSwap += Time.deltaTime;

        if (flying)
            height = maxHeight + Mathf.Sin (Time.time);

        if (timeSinceSwap > swapSecs) {
            timeSinceSwap = 0f;

            if (flying)
                flying = false;
            else {
                height = maxHeight;
                flying = true;
            }
        }            

        if (flying)
            fly ();
        else
            land ();
    }

    void land () {
        RaycastHit hit;

        if (Physics.Raycast (transform.position, Vector3.down, out hit)) {
            if (hit.distance > 0) {
                transform.position = new Vector3 (transform.position.x, transform.position.y - climbVel * Time.deltaTime, transform.position.z);
            }
        } else {
            anim.SetBool ("Flying", false);
        }
    }

    void fly () {
        anim.SetBool ("Flying", true);
        RaycastHit hit;
        if (Physics.Raycast (transform.position, Vector3.down, out hit)) {
            if (hit.distance < height) {
                float y = transform.position.y + (climbVel * Time.deltaTime);
                transform.position = new Vector3 (transform.position.x, y, transform.position.z);
            } else if (hit.distance > height) {
                float y = transform.position.y - (climbVel * Time.deltaTime);
                transform.position = new Vector3 (transform.position.x, y, transform.position.z);
            }
        } else {
            transform.position = new Vector3 (transform.position.x, transform.position.y + climbVel * Time.deltaTime, transform.position.z);
        }
    }

    public void damage (int dmg) {
        hp -= dmg;
        healthBar.value = hp;

        if (hp <= 0) {
            //Instantiate (self, new Vector3 (0, 0, 0), Quaternion.identity);
            Destroy (gameObject);
        }
    }
}