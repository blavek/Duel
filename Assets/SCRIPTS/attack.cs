using UnityEngine;
using System.Collections;

public class attack : MonoBehaviour {
    public Controller player;
    public int wpnDmg = 10;

    private int frame = 0;
    Collider c;
//	public Animation anim;
	// Use this for initialization
	void Start () {
		c = GetComponent<Collider> ();
/*
        anim = GameObject.FindGameObjectWithTag("pc").GetComponent<Animation> ();
        if (anim == null)
            Debug.Log ("No ANim");
*/
    }
	
	// Update is called once per frame
	void Update () {
		c.transform.rotation = transform.rotation;
        frame++;
	}

    //      Debug.Log (col.collider);
    void weaponHit (Collision col) {
        bool attacking = player.getAttackState ();

        if (attacking) {
            if (col.gameObject.GetComponent<enemyController> () != null && !player.getAttacked ()) {
                col.gameObject.GetComponent<enemyController> ().damage (wpnDmg);
                player.setAttacked ();
            }

            if (col.gameObject.GetComponent<BirdController> () != null && !player.getAttacked ()) {
                col.gameObject.GetComponent<BirdController> ().damage (wpnDmg);
                player.setAttacked ();
            }

            if (col.gameObject.GetComponent<dragonBoss> () != null && !player.getAttacked()) {
                col.gameObject.GetComponent<dragonBoss> ().damage (wpnDmg);
                player.setAttacked ();
            }
        }
    }



	void OnCollisionEnter (Collision col) {
        weaponHit (col);
	}

    void OnCollisionStay (Collision col) {
        weaponHit (col);
    }
}
