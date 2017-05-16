using UnityEngine;
using System.Collections;

public class attack : MonoBehaviour {
    public Controller player;

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
	}

	void OnCollisionEnter (Collision col) {
//		Debug.Log (col.collider);
        bool attacking = player.getAttackState ();

        if (attacking) {
            if (col.gameObject.GetComponent<enemyController> () != null)
                col.gameObject.GetComponent<enemyController> ().damage (10);

            if (col.gameObject.GetComponent<BirdController> () != null)
                col.gameObject.GetComponent<BirdController> ().damage (10);
		}
	}
}
