using UnityEngine;
using System.Collections;

public class lavaDamage : MonoBehaviour {
    public float damagePerSecond = 2f;

    void OnCollisionStay (Collision col) {
        if (col.gameObject.GetComponent<Controller> () != null)
            col.gameObject.GetComponent<Controller> ().damage (damagePerSecond * Time.deltaTime);
    }
}