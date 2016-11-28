using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {
	public GameObject target;
	public int camHeightOffset = 10;
	public int distanceOffset = 10;
	public float rotSpeed = 1000f;
	public float defRot = 180;
	private float curRot;
	private float rotOff;

	// Use this for initialization
	void Start () {
			defRot = defRot % 360;

		if (defRot < 0)
			defRot += 360;

		rotOff = 180 - defRot;
		curRot = defRot;
		updateCam ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		rotateCam ();
	}

	void rotateCam() {
		float lX = Input.GetAxis ("LookX");
		curRot = curRot % 360;

		if (curRot < 0)
			curRot += 360;

//		float lY = Input.GetAxis ("LookY");
		/* && Mathf.Abs (lY) < .1f)*/ 

		if (Mathf.Abs (lX) > .1f) {
			curRot -= lX * rotSpeed * Time.deltaTime;
		} else {
//			Debug.Log ("curRot: " + Mathf.Abs(curRot) + "\n" + "defRot: " + defRot + "\n(Mathf.Abs(curRot) < defRot): " + (Mathf.Abs(curRot) < defRot));

//			Debug.Log ("(rotOff): " + rotOff);

			if (curRot + rotOff < defRot + rotOff) {
//				Debug.Log ("LESS THAN");
				curRot += rotSpeed * Time.deltaTime;
			} 

			if (curRot + rotOff > defRot + rotOff) {
//				Debug.Log ("GREATER THAN");
				curRot -= rotSpeed * Time.deltaTime;
			}
/*
			if (Mathf.Abs (defRot - curRot) <= 2) {
				Debug.Log ("offset: " + Mathf.Abs (defRot - curRot));
				curRot = defRot;
			}
*/
		}

//		curRot = Mathf.FloorToInt (curRot);
		updateCam ();
	}

	void updateCam() {
		float x = Mathf.Sin (curRot * Mathf.Deg2Rad) * distanceOffset;
		float z = Mathf.Cos (curRot * Mathf.Deg2Rad) * distanceOffset;

		transform.position = new Vector3 (x + target.transform.position.x, camHeightOffset, z + target.transform.position.z);

//		transform.position = new Vector3 (Mathf.Lerp(x, x + target.transform.position.x, rotSpeed * Time.deltaTime), camHeightOffset, Mathf.Lerp(z,z + target.transform.position.z, rotSpeed * Time.deltaTime));

		transform.LookAt (target.transform.position);	
	}
}
