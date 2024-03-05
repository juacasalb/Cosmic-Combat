using System.Collections.Generic;
using UnityEngine;

public class Mine : Weapon {

    private Vector3 basePosition = new Vector3(25f,0f,0f);

    void Start() {
        damage = 100;
    }

    private void deactivate() {
        transform.position = new Vector3(25f,0f,0f);
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.GetComponent<CommonMonster>() != null) {
            CommonMonster commonMonsterScript = other.gameObject.GetComponent<CommonMonster>();
            commonMonsterScript.looseHealthPoints(damage);
            deactivate();
        }

        else if (other.gameObject.GetComponent<Boss>() != null) {
            Boss bossScript = other.gameObject.GetComponent<Boss>();
            bossScript.looseHealthPoints(damage);
            deactivate();
        }

        else if (other.gameObject.GetComponent<Missile>() != null) {
            deactivate();
        }

    }

}
