using UnityEngine;
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
<<<<<<< HEAD
        curRot = defRot; //target.transform.eulerAngles.y + 180;
		updateCam ();
=======
		transform.LookAt (target.transform.position);
		transform.rotation = target.transform.rotation;
		transform.position = target.transform.position;
		transform.position = transform.position + new Vector3 (0, camHeightOffset, -distanceOffset);
  		offset = target.transform.position - transform.position;// new Vector3(0, 5, 0);

		//		updateCam ();
>>>>>>> refs/remotes/origin/Brian
	}
	
	// Update is called once per frame
	void LateUpdate () {
		rotateCam ();
	}

	void rotateCam() {
        float lX = Input.GetAxis ("LookX");
		float lY = Input.GetAxis ("LookY");

		curRot = curRot % 360;

		if (curRot < 0)
			curRot += 360;
		
		if (Mathf.Abs (lX) > .1f) {
			curRot += lX * rotSpeed * Time.deltaTime;
		} else {
			if (curRot < defRot)
				curRot += rotSpeed * Time.deltaTime;

			if (curRot > defRot)
				curRot -= rotSpeed * Time.deltaTime;

			if (Mathf.Abs (curRot - defRot) < 1) {
				curRot = defRot;
			}
		}

//        curRot = target.transform.eulerAngles.y + 180; //Mathf.FloorToInt (curRot);
		updateCam ();
	}
	//		transform.LookAt (tar);
	void updateCam() {
		float x = Mathf.Sin (curRot * Mathf.Deg2Rad) * distanceOffset;
		float z = Mathf.Cos (curRot * Mathf.Deg2Rad) * distanceOffset;

//		transform.position = new Vector3 (Mathf.Lerp(x, x + target.transform.position.x, rotSpeed * Time.deltaTime), camHeightOffset, Mathf.Lerp(z,z + target.transform.position.z, rotSpeed * Time.deltaTime));
		transform.position = new Vector3 (x + target.transform.position.x, camHeightOffset, z + target.transform.position.z);

		transform.LookAt (target.transform.position);	
	}
}
