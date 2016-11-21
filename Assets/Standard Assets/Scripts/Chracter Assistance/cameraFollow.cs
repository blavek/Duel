﻿using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {
	public GameObject target;
	public int camHeightOffset = 10;
	public int distanceOffset = 10;
	public float rotSpeed = 1000f;
	public float defRot = 180;
	private float curRot;

	// Use this for initialization
	void Start () {
		curRot = defRot;
		updateCam ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		rotateCam ();
	}

	void rotateCam() {
		float lX = Input.GetAxis ("LookX");
		float lY = Input.GetAxis ("LookY");

		if (Mathf.Abs (lX) < .1f && Mathf.Abs (lY) < .1f) {
			if (curRot < defRot)
				curRot += rotSpeed * Time.deltaTime;
			else if (curRot > defRot)
				curRot -= rotSpeed * Time.deltaTime;

		} else {
			curRot += lX * rotSpeed * Time.deltaTime;
		}

		curRot = Mathf.FloorToInt (curRot);
		updateCam ();
	}
	//		transform.LookAt (tar);
	void updateCam() {
		float x = Mathf.Sin (curRot * Mathf.Deg2Rad) * distanceOffset;
		float z = Mathf.Cos (curRot * Mathf.Deg2Rad) * distanceOffset;

		transform.position = new Vector3 (Mathf.Lerp(x, x + target.transform.position.x, rotSpeed * Time.deltaTime), camHeightOffset, Mathf.Lerp(z,z + target.transform.position.z, rotSpeed * Time.deltaTime));

		transform.LookAt (target.transform.position);	
	}
}
