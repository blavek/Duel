using UnityEngine;
using System.Collections;

public class particalAttack : MonoBehaviour {
    public float fireBreathDPS = 5f;
    public float fireShotDamage = 5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticleCollision (GameObject col) {
        Debug.Log (gameObject.name);
        if (col.gameObject.GetComponent<Controller> () != null) {
            switch (gameObject.name) {
                case "FireShotParticle":
                    col.gameObject.GetComponent<Controller> ().damage (fireShotDamage); 
                    break;

                case "FireBreathParticle":
                    col.gameObject.GetComponent<Controller> ().damage (fireBreathDPS * Time.deltaTime); 
                    break;
            }
        }
    }
}
