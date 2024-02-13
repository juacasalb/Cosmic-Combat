using System.Collections.Generic;
using UnityEngine;

public class Explotion : MonoBehaviour {
    private Animator animator;
    private float selfCounter = 0.38f;

    private void deactivate() {
        if(selfCounter<=0.0f) {
            selfCounter = 0.38f;
            transform.position = new Vector3(20,0,0);
            //gameObject.SetActive(false);
        } 
    }

    void Update() {
        if(!gameObject.activeSelf && animator==null) animator = GetComponent<Animator>();
        if(gameObject.activeSelf) {
            selfCounter-=Time.deltaTime;
            deactivate();
        }
    }

}
