using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {
	public GameObject target;
	private int camHeightOffset = 5;
	public int distanceOffset = 10;

	// Use this for initialization
	void Start () {
		Vector3 tar = target.transform.position;
		transform.position = new Vector3 (tar.x, tar.y + camHeightOffset, tar.z - distanceOffset);
		transform.LookAt (tar);	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tar = target.transform.position;
		transform.position = new Vector3 (tar.x, tar.y + camHeightOffset, tar.z - distanceOffset);
//		transform.LookAt (tar);

	}
}
