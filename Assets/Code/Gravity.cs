using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
    private float gravity = 3f;
    private float relativeGravityModifier = 1.0f;
    Rigidbody2D rb;

    void applyGravity() {
        Vector2 direction = (Vector3.zero - gameObject.transform.position).normalized;
        float distance = Vector2.Distance(Vector2.zero, gameObject.transform.position);

        float force = gravity / (distance * distance);

        rb.AddForce(direction * force);
    }

    void applyRotation() {
        float lag = 90f;
        Vector3 planetCenter = Vector3.zero;
        Vector3 MonsterPosition = transform.position;

        float angle = Mathf.Atan2(MonsterPosition.y - planetCenter.y, MonsterPosition.x - planetCenter.x) * Mathf.Rad2Deg - lag;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void applyMissileGravity() {
        if(gameObject.activeSelf) {
            gravity += (Time.deltaTime/5.0f)*relativeGravityModifier;
            Debug.Log("Gravedad: " + gravity);
        }
        if(gravity <= 3f) {
            relativeGravityModifier = 1.0f;
        } else if (gravity >= 4f) {
            relativeGravityModifier = -1.0f;
        }
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        applyGravity();
        applyRotation();
        if(gameObject.GetComponent<Missile>()!=null) {
            applyMissileGravity();
        }
    }
}
