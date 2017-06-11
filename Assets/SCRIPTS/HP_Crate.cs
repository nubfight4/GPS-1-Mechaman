using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Crate : MonoBehaviour {

    public int hpHeal;

    private void OnTriggerEnter2D(Collider2D other){

        if (other.GetComponents<LifeObject>() == null)
            return;

        Destroy(gameObject);
    }
}