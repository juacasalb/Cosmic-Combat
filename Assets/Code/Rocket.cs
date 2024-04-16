using System;
using UnityEngine;
using System.Collections.Generic;

public class Rocket : MonoBehaviour {
    private static Rigidbody2D rb2d;
    private static GameObject self;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<Character>() != null) {
            GameManager.instance.isGameFinished = true;
        }
    }

    public static void generateRocketOnPlanetSurface() {
        self.transform.position = RandomClass.getRandomPointOnPlanetSurface();
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    void Start() {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        self = gameObject;
    }

}
