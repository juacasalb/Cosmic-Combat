using System.Collections.Generic;
using UnityEngine;

public class Mine : Weapon {

    private Vector3 basePosition = new Vector3();

    void Start() {
        damage = 100;
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.GetComponent<CommonMonster>() != null) {
            CommonMonster commonMonsterScript = other.gameObject.GetComponent<CommonMonster>();
            commonMonsterScript.looseHealthPoints(damage);
            gameObject.SetActive(false);
        }

        else if (other.gameObject.GetComponent<Boss>() != null) {
            Boss bossScript = other.gameObject.GetComponent<Boss>();
            bossScript.looseHealthPoints(damage);
            gameObject.SetActive(false);
        }

        else if (other.gameObject.GetComponent<Missile>() != null) {
            gameObject.SetActive(false);
        }

    }

}
