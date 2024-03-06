using System.Collections.Generic;
using UnityEngine;

public class LaserRay : Weapon {

    public Vector3 _direction {
        get { return direction; }
        set { direction = value; }
    }
    Rigidbody2D rb2d;
    private float selfCounter;
    private Vector3 direction = Vector3.zero;
    private float speed;

    [HideInInspector]
    public string shooterName;

    void Start() {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        speed = 7f;
        damage = 100;
        selfCounter = 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.name!=shooterName) {
            if(other.gameObject.GetComponent<Character>() != null) {
                Character characterScript = other.gameObject.GetComponent<Character>();
                characterScript.looseHealthPoints(damage);
            }

            else if (other.gameObject.GetComponent<CommonMonster>() != null) {
                CommonMonster commonMonsterScript = other.gameObject.GetComponent<CommonMonster>();
                commonMonsterScript.looseHealthPoints(damage);
            }

            else if (other.gameObject.GetComponent<Boss>() != null) {
                Boss bossScript = other.gameObject.GetComponent<Boss>();
                bossScript.looseHealthPoints(damage);
            }
        }
    }


    void movement() {
        rb2d.velocity = direction * speed;
    }


    void Update() {
        if(gameObject.activeSelf) {
            movement();
            selfCounter-=Time.deltaTime;
            if(selfCounter<=0.0f) {
                direction = Vector3.zero;
                gameObject.SetActive(false);
                selfCounter = 1.5f;
            }
        }
    }

}
