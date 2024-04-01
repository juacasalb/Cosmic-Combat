using System.Collections.Generic;
using UnityEngine;

public class CommonMonster : Monster {

    public bool isMyTurn;
    private float movementTimer;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb2d;

    protected override void shoot() {
        Missile missile = transform.Find("Missile").GetComponent<Missile>();
        Vector3 actualPosition = transform.position;
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomY = UnityEngine.Random.Range(-1f, 1f);
        Vector3 angleOfShoot = new Vector3(randomX, randomY, 0);

        missile.gameObject.transform.position = new Vector3(actualPosition.x + angleOfShoot.x, 
            actualPosition.y + angleOfShoot.y);

        missile.gameObject.SetActive(true);
        missile._direction = angleOfShoot;
    }

    protected void move() {
        float xDirection = 1-(2*UnityEngine.Random.value);
        Vector3 direction = new Vector3(xDirection,0f,0f);
        while(movementTimer >= 0f) {
            movement(direction);
            movementTimer-= Time.deltaTime;
        }
        movementTimer = 10f;
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

    void Start() {
        fullHealth();
        isMyTurn = false;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if(healthPoints<=0) {
            deactivate();
        }
        if(isMyTurn) {
            movementTimer = 10f;
            move();
            shoot();
            isMyTurn=false;
        }
    }

}