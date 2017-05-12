using UnityEngine;
using System.Collections;

public class TextureScroll : MonoBehaviour {

	public float ScrollX = 0.5f;
	public float ScrollY = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		float OffsetX = Time.time * ScrollX;
		float OffsetY = Time.time * ScrollY;
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
	}
}
