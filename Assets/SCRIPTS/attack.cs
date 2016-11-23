using UnityEngine;
using System.Collections;

public class attack : MonoBehaviour {
	Collider c;
	public Animation anim;
	// Use this for initialization
	void Start () {
		c = GetComponent<Collider> ();
		anim = GameObject.FindGameObjectWithTag("pc").GetComponent<Animation> ();
	}
	
	// Update is called once per frame
	void Update () {
		c.transform.rotation = transform.rotation;
	}

	void OnCollisionEnter (Collision col) {
		Debug.Log (col.gameObject.name);

		//if (GameObject.FindGameObjectWithTag("pc").GetComponent<Animator> ().GetCurrentAnimatorClipInfo(0))//. IsPlaying("Attack"))

		AnimatorClipInfo [] clipInfo = GameObject.FindGameObjectWithTag ("pc").GetComponent<Animator> ().GetCurrentAnimatorClipInfo (0);

		foreach (AnimatorClipInfo c in clipInfo) {
			if (c.clip.name == "Attack") {
				col.gameObject.GetComponent<enemyController> ().damage (10);
				break;
			}
		}
	}

}
