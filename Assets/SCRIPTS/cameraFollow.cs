using UnityEngine;
using System.Collections;
//using Controller.cs;

public class cameraFollow : MonoBehaviour {
	public Controller target;
	public int camHeightOffset = 5;
	public float distanceOffset = 10f;
    public float maxDistanceOffset = 10f;
    public float minDistanceOffset = 2f;
    public float lowerHeightBound = .5f;
    public float upperHeightBound = 9.5f;
    public float rotSpeed = .5f;
	public float damping = 1;
    public float catchUpThresh = .3f;

    private bool inCatchUp = false;
    private float lastVel = 0;
    private float latRotMult = 4f;
    private Vector3 offset;
    private Controller pc;

	// Use this for initialization
	void Start () {
		transform.LookAt (target.transform.position);
		transform.rotation = target.transform.rotation;
		transform.position = target.transform.position;

        transform.position = transform.position + new Vector3 (0, camHeightOffset, -Mathf.Sqrt (Mathf.Pow (distanceOffset, 2) - Mathf.Pow (camHeightOffset, 2)));
  		offset = target.transform.position - transform.position;// new Vector3(0, 5, 0);
        transform.LookAt (target.GetComponent<Collider>().bounds.center);
		//		updateCam ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        /* If (stick Moves) Look
         * If (velocity) = 0
         */
        float vel = target.getVelocity ();

        if (lastVel > 0 && vel == 0)
            inCatchUp = true;

        lastVel = vel;

        if (Mathf.Abs (Input.GetAxis ("LookX")) > .1f || Mathf.Abs (Input.GetAxis ("LookY")) > .1f) {
            lookCam ();
            inCatchUp = false;
        } else if (inCatchUp || vel > 0) {
            followCam ();
        }
	}

	void followCam() {
		float curAngle = transform.eulerAngles.y;
		float desAngle = target.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle (curAngle, desAngle, Time.deltaTime * damping);
        float height = Mathf.Lerp (transform.position.y, -offset.y + target.transform.position.y, Time.deltaTime * damping);

//        Debug.Log (height + " " + -offset.y + " " + transform.position.y);

        if (Mathf.Abs(transform.rotation.eulerAngles.y - angle) < catchUpThresh)
            inCatchUp = false;
//        Debug.Log(height + " pos " + transform.position + " off " + -offset.y);

        if (distanceOffset < maxDistanceOffset)
            distanceOffset += (Time.deltaTime * rotSpeed);

		Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target.transform.position - (rotation * Vector3.forward * (distanceOffset/* / maxDistanceOffset*/));
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
//        Debug.Log ("Pos " + transform.position);
        transform.LookAt (target.GetComponent<Collider>().bounds.center);
	}

    void lookCam() {
        float lx = Input.GetAxis ("LookX");
        float ly = Input.GetAxis ("LookY");

        float x = 0;
        float y = transform.position.y - target.transform.position.y; // account for player being above or below sea level
        float z = 0;
        float yr = 0;

        // latitudinalk orbit
        if (Mathf.Abs (lx) > .1f) {
            yr = lx * rotSpeed * Time.deltaTime * latRotMult;
            transform.RotateAround (target.transform.position, Vector3.up, yr);
        }

        if (Mathf.Abs (ly) > .1f) {
            ly = ly * rotSpeed * Time.deltaTime;
            if (ly > 0) {                                   // Raising the Camera
                                                            // Camera is pushed in we'll need to move it out first
                if (y <= lowerHeightBound && distanceOffset < maxDistanceOffset) {                
                    if (distanceOffset + ly <= maxDistanceOffset) {
                        distanceOffset += ly;
                    } else {
                        distanceOffset = maxDistanceOffset;
                    }
                } else if (y + ly <= upperHeightBound) {   // The camera is far enough out we can begin raising it but not higher than the farthest away the camera can be                    
                    y += ly;
                } else {
                    y = upperHeightBound;
                }
            } else {                                        // Lowering the Camera
                if (y + ly >= lowerHeightBound) {
                    y += ly;
                } else {                                    // Camera is too low move it closer to the character
                    y = lowerHeightBound;

                    if (distanceOffset + ly >= minDistanceOffset) {  
                        distanceOffset += ly;
                    } else {                                // Too close stop moving it in
                        distanceOffset = minDistanceOffset;
                    }
                }
            }
        }

        if (distanceOffset < maxDistanceOffset) {           // while the camera is in close to the playter y must be minimum height
            y = lowerHeightBound;
        }

        float a = Mathf.Sqrt (Mathf.Pow (distanceOffset, 2) - Mathf.Pow (y, 2));    // Remove character position from y to get 2d distance from character

        yr = (((transform.rotation.eulerAngles.y - 180) % 360) + 360) % 360;        // Normalize yr to be the angle of the orbit not the angle of the camera.
        z = a * Mathf.Cos (yr * Mathf.Deg2Rad) + target.transform.position.z;
        x = a * Mathf.Sin (yr * Mathf.Deg2Rad) + target.transform.position.x;

        Vector3 pos = new Vector3 (x, y + target.transform.position.y, z);          // Add player Height back into the calculation

//        Debug.Log ("Pos " + pos);
//        Debug.Log ("Distance " + Vector3.Distance(pos, target.transform.position));

        transform.position = pos;
        transform.LookAt (target.GetComponent<Collider>().bounds.center);
    }
}
   