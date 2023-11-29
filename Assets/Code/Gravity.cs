using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
    public float gravity = 1.0f;
    Rigidbody2D rb;

    void applyGravity() {
        Vector2 direction = (Vector3.zero - gameObject.transform.position).normalized;
        float distance = Vector2.Distance(Vector2.zero, gameObject.transform.position);

        float force = gravity / (distance * distance);

        rb.AddForce(direction * force);
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        applyGravity();
    }
}
