using UnityEngine;
using System.Collections;

public class boneAttacks : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter (Collision col) {
        dragonBoss db = GetComponentInParent<dragonBoss> ();
        if (db == null) {
            Debug.Log ("no boss");
            return;
        }
        
        if (db.getAttackState() && !db.getAttacked()) {
            if (col.gameObject.GetComponent<Controller> () != null) {
                col.gameObject.GetComponent<Controller> ().damage (db.dmg);
                db.setAttacked ();
            }
        }
    }
}
