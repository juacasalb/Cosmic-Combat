using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : Weapon {
    private Animator animator;
    private float selfCounter = 0.6f;

    void Start() {
        damage = 60;
        areaOfEffect = 1.25f;
    }

    private void applyDamage() {
        if(selfCounter<=0.0f) {
            Vector2 actualPosition = transform.position;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(actualPosition, areaOfEffect);

            foreach (Collider2D collider in colliders) {
                CommonMonster CommonMonsterScript = collider.GetComponent<CommonMonster>();
                Boss BossScript = collider.GetComponent<Boss>();

                if (CommonMonsterScript != null)
                    CommonMonsterScript.looseHealthPoints(damage);
                    
                if (BossScript != null)
                    BossScript.looseHealthPoints(damage);
                    
            }

            selfCounter = 0.6f;
            gameObject.SetActive(false);
        }
    }

    void Update() {
        if(!gameObject.activeSelf && animator==null) animator = GetComponent<Animator>();
        if(gameObject.activeSelf) {
            selfCounter-=Time.deltaTime;
            applyDamage();
        }
    }

}
