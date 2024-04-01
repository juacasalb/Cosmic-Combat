using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    public List<Transform> getMobiles() {
        List<Transform> mobiles = new List<Transform>();
        for(int i = 0; i<transform.childCount;i++) {
            mobiles.Add(transform.GetChild(i));
        }
        return mobiles;
    }

    public void deleteMobile(GameObject mobile) {
        string mobileName = mobile.name;
        foreach(GameObject child in transform) {
            if(child.name==mobileName)
                child.transform.SetParent(null);
        }
    }

    public void resetMobility() {
        for(int i = 0; i<transform.childCount;i++) {
            Rigidbody2D rb2d = transform.GetChild(i).GetComponent<Rigidbody2D>();
            rb2d.bodyType = RigidbodyType2D.Static;
            rb2d.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    void choosePlanetTexture(PlanetType texture) {

    }

    void Start() {
        //Aqui se va a setear el PNG respecto al TipoPlaneta correspondiente
    }

    void Update() {
        
    }
}
