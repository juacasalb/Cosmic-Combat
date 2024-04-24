using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
    private static Transform mytransform;
    public static GameObject[] munitions;

    public List<Transform> getMobiles() {
        List<Transform> mobiles = new List<Transform>();
        for(int i = 0; i<transform.childCount;i++) {
            mobiles.Add(transform.GetChild(i));
        }
        return mobiles;
    }

    public static void deleteMobile(GameObject mobile) {
        mobile.transform.SetParent(null);
    }

    public void resetMobility() {
        for(int i = 0; i<transform.childCount;i++) {
            Rigidbody2D rb2d = transform.GetChild(i).GetComponent<Rigidbody2D>();
            rb2d.bodyType = RigidbodyType2D.Static;
            rb2d.bodyType = RigidbodyType2D.Dynamic;
        }
        foreach(GameObject munition in munitions) {
            Rigidbody2D rb2d = munition.transform.GetComponent<Rigidbody2D>();
            if(rb2d.bodyType == RigidbodyType2D.Dynamic) {
                rb2d.bodyType = RigidbodyType2D.Static;
                rb2d.bodyType = RigidbodyType2D.Dynamic;
            }
        }

    }

    void choosePlanetTexture(PlanetType texture) {

    }

    void Start() {
        //Aqui se va a setear el PNG respecto al TipoPlaneta correspondiente
        mytransform = transform;
        munitions = GameObject.FindGameObjectsWithTag("Munition");
    }

    void Update() {
        
    }
}
