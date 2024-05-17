using System.Collections.Generic;
using UnityEngine;

public class Mine : Weapon {

    void Start() {
        damage = 100;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        GameManager.instance.playSound("explode");

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
