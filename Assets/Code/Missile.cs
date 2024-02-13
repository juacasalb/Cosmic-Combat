using System.Collections.Generic;
using UnityEngine;

public class Missile : Weapon {

    public Vector3 _direction {
        get { return direction; }
        set { direction = value; }
    }
    private Vector3 direction = Vector3.zero;
    private float speed;

    private void OnCollisionEnter2D(Collision2D other) {
        Explotion explotion = GameObject.FindWithTag("Explotion").GetComponent<Explotion>();
        Vector2 actualPosition = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(actualPosition, areaOfEffect);

        foreach (Collider2D collider in colliders) {
            //Mine mineScript = collider.GetComponent<Mine>();
            Character characterScript = collider.GetComponent<Character>();
            CommonMonster CommonMonsterScript = collider.GetComponent<CommonMonster>();
            Boss BossScript = collider.GetComponent<Boss>();

            /*if (mineScript != null)
                mineScript.SetActive(false);*/
            if (characterScript != null)
                characterScript.looseHealthPoints(damage);
            if (CommonMonsterScript != null)
                CommonMonsterScript.looseHealthPoints(damage);
            if (BossScript != null)
                BossScript.looseHealthPoints(damage);
        }

        explotion.gameObject.transform.position = actualPosition;
        gameObject.SetActive(false);
    }

    void Start() {
        damage = 40;
        areaOfEffect = 0.75f;
        speed = 1.1f;
    }

    void movement() {
        gameObject.transform.Translate(direction * speed * Time.deltaTime);
    }

    void Update() {
        if(gameObject.activeSelf) {
            movement();
        }
    }

}
