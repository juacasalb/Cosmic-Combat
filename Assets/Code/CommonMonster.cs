using System.Collections.Generic;
using UnityEngine;

public class CommonMonster : Monster {

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void shoot() {
        Missile missile = transform.Find("Missile").GetComponent<Missile>();
        Vector3 actualPosition = transform.position;
        float randomX = UnityEngine.Random.Range(0f, 1f);
        float randomY = UnityEngine.Random.Range(0f, 1f);
        int isXNegative = (UnityEngine.Random.Range(0,2) * 2) - 1;
        int isYNegative = (UnityEngine.Random.Range(0,2) * 2) - 1;
        Vector3 angleOfShoot = new Vector3(randomX*isXNegative, randomY*isYNegative, 0);

        missile.gameObject.transform.position = new Vector3(actualPosition.x + angleOfShoot.x, 
            actualPosition.y + angleOfShoot.y);
        missile.gameObject.SetActive(true);
        missile._direction = angleOfShoot;
    }

    public override void looseHealthPoints(int damage) {
        healthPoints-=damage;
    }

    public override void fullHealth() {
        healthPoints=100;
        isAlive=true;
    }

    void Update() {
        
    }

}