using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {
	public GameObject target;
	public int camHeightOffset = 1;
	public int distanceOffset = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tar = target.transform.position;
		transform.position = new Vector3 (tar.x, tar.y + camHeightOffset, tar.z - distanceOffset);
		transform.LookAt (tar);

	}
}
