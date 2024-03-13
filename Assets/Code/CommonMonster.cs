using System.Collections.Generic;
using UnityEngine;

public class CommonMonster : Monster {

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb2d;

    void Start() {
        fullHealth();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
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
    }

    public override void activate(Vector3 position) {
        gameObject.transform.position = position;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void deactivate() {
        GameManager.instance.killedMonsters++;
        gameObject.transform.position = basePosition;
        rb2d.bodyType = RigidbodyType2D.Static;
    }

    void Update() {
        if(healthPoints<=0) {
            deactivate();
        }
    }

}