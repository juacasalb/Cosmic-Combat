using System.Collections.Generic;
using UnityEngine;

public class CommonMonster : Monster {

    public bool isMyTurn;
    private float movementTimer;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb2d;
    private int xDirection;
    private Vector3 direction;

    protected override void movement(Vector3 direction) { 
        transform.Translate(direction * speed * Time.deltaTime);
    }

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

    private void calculateNextDirection() {
        xDirection = (UnityEngine.Random.Range(0,2) * 2) - 1;
        direction = new Vector3(xDirection,0f,0f);
    }

    public int getHealthPoints() {
        return healthPoints;
    }

    void Start() {
        fullHealth();
        calculateNextDirection();
        movementTimer = 5f;
        speed = 0.45f;
        isMyTurn = false;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if(healthPoints<=0) {
            deactivate();
        }

        if (xDirection==0) {
            calculateNextDirection();
        }

        if(isMyTurn) {
            if(movementTimer >= 0f) {
                movement(direction);
                movementTimer -= Time.deltaTime;
            } else {
                shoot();
                movementTimer = 2f;
                xDirection=0;
                isMyTurn=false;
            }
        }
    }

}