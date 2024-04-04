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
        for(int i = 0; i<transform.childCount;i++) {
            if(transform.GetChild(i).gameObject.name==mobile.name)
                transform.GetChild(i).transform.SetParent(null);
        }
    }

    public void resetMobility() {
        for(int i = 0; i<transform.childCount;i++) {
            Rigidbody2D rb2d = transform.GetChild(i).GetComponent<Rigidbody2D>();
            rb2d.bodyType = RigidbodyType2D.Static;
            rb2d.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void discardMobiles(ref int shiftCounter) {
        for(int i = 0; i<transform.childCount;i++) {
            Character character = transform.GetChild(i).GetComponent<Character>();
            CommonMonster commonMonster = transform.GetChild(i).GetComponent<CommonMonster>();
            Boss boss = transform.GetChild(i).GetComponent<Boss>();

            bool noHealth = (character!=null && character.getHealthPoints() <= 0) ||
                      (commonMonster!=null && commonMonster.getHealthPoints() <= 0) ||
                      (boss!=null && boss.getHealthPoints() <= 0);

            if(noHealth) {
                deleteMobile(transform.GetChild(i).gameObject);
                shiftCounter--;
            }
        }
        if (shiftCounter<0) shiftCounter = 0;
    }

    void choosePlanetTexture(PlanetType texture) {

    }

    void Start() {
        //Aqui se va a setear el PNG respecto al TipoPlaneta correspondiente
    }

    void Update() {
        
    }
}
