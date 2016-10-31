using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {
	public GameObject target;
	private int camHeightOffset = 10;
	public int distanceOffset = 10;
	public float lookSpeed = .1f;

	// Use this for initialization
	void Start () {
		Vector3 tar = target.transform.position;
		transform.position = new Vector3 (tar.x, tar.y + camHeightOffset, tar.z - distanceOffset);
		transform.LookAt (tar);	
	}
	
	// Update is called once per frame
	void Update () {
		float lX = Input.GetAxis ("LookX");
		float lY = Input.GetAxis ("LookY");

		Vector3 tar = target.transform.position;
		transform.LookAt (tar);

		if (Mathf.Abs (lX) < .1f && Mathf.Abs (lY) < .1f)
			transform.position = new Vector3 (tar.x, tar.y + camHeightOffset, tar.z - distanceOffset);
		else {
			transform.RotateAround (tar, Vector3.up, lX * lookSpeed * Time.deltaTime);
			Vector3 relVec = tar - transform.position;
			if (relVec.magnitude > distanceOffset)
				transform.Translate (Vector3.forward * 10 * Time.deltaTime);
			else if(relVec.magnitude < distanceOffset)
				transform.Translate (Vector3.back * 10 * Time.deltaTime);
		}



//		transform.LookAt (tar);
	}
}
