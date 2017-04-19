using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {
	public GameObject target;
	public int camHeightOffset = 5;
	public int distanceOffset = 10;

/*
 * public float rotSpeed = 1000f;
	public float defRot = 180;
	public float damping = 1;
	private float curRot;
*/
	public float damping = 1;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		transform.LookAt (target.transform.position);
		transform.rotation = target.transform.rotation;
		transform.position = target.transform.position;
		transform.position = transform.position + new Vector3 (0, camHeightOffset, -distanceOffset);
  		offset = target.transform.position - transform.position;// new Vector3(0, 5, 0);

		//		updateCam ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		followCam ();		
//		rotateCam ();
	}

	void followCam() {
		float curAngle = transform.eulerAngles.y;
		float desAngle = target.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle (curAngle, desAngle, Time.deltaTime * damping);


		Quaternion rotation = Quaternion.Euler(0, angle, 0);
		transform.position = target.transform.position - (rotation * offset);
		transform.LookAt (target.transform.position);
	}
/*
	/// </summary>
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
*/}
