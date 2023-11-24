using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float gravity = 9.8f;

    void onTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Missile") || other.CompareTag("Character") || other.CompareTag("Monster"))
        {
            applyGravity(other.transform);
        }
    }
    
    void applyGravity(Transform mobile) {
        Vector2 direction = (transform.position - mobile.position).normalized;
        float distance = Vector2.Distance(transform.position, mobile.position);

        float force = gravity / (distance * distance);

        mobile.GetComponent<Rigidbody2D>().AddForce(direction * force);
    }

    void choosePlanetTexture(PlanetType texture) {

    }

    void Start() {
        //Aqui se va a setear el PNG respecto al TipoPlaneta correspondiente
    }

    void Update() {
        
    }
}
