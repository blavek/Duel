using UnityEngine;
using System.Collections;

public class enemyAttack : MonoBehaviour {
    public int damage = 5;

    private Collider c;
    private enemyController cont;

    void Start () {
        c = GetComponent<Collider> ();
        cont = GetComponentInParent<enemyController> ();
        if (cont == null) {
            Debug.Log ("NO ENEMY CONTROLLER HAVE A DAY");
        }
    }

    void Update () {
        c.transform.rotation = transform.rotation;
//        Debug.Log ("attacking " + cont.getAttackState());
    }

    void OnCollisionEnter (Collision col) {
//        Debug.Log ("Hit " + col.collider.name);
        if (cont.getAttackState()) {
            if (col.gameObject.GetComponent<Controller> () != null)
                col.gameObject.GetComponent<Controller> ().damage (damage);
        }
    }
}