using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {
    public float rotSpeed = 200f;
    private GameObject target;


	// Use this for initialization
	void Start () {
        gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null) {
            gameObject.SetActive (false);
        } else if (target.GetComponent<enemyController> () != null) {
            if (target.GetComponent<enemyController> ().isAlive ())
                transform.position = target.transform.position + new Vector3(0, transform.lossyScale.y / 2, 0);
            else
                gameObject.SetActive (false);
        } else {                
            transform.position = target.transform.position;
        }

        transform.Rotate (Vector3.forward, rotSpeed * Time.deltaTime);
        Vector3 temp = transform.rotation.eulerAngles;
        transform.LookAt (Camera.main.transform);
        transform.Rotate(0,0,temp.z);

//        transform.Rotate(new Vector3(0,0,));
	}

    public void setTarget(GameObject tar) {
        target = tar;
    }
}
