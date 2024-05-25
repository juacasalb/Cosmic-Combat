using System.Collections.Generic;
using UnityEngine;

public class CommonMonster : Monster {

    public bool isMyTurn;
    private float movementTimer;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb2d;
    public int xDirection;
    private Vector3 direction;

    public Vector3 getDirection() {
        return direction;
    }

    protected override void movement(Vector3 direction) { 
        transform.Translate(direction * speed * Time.deltaTime);
    }

    protected override void shoot() {
        GameManager.instance.playSound("missile");
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
        GameManager.instance.playSound("damage");
        healthPoints-=damage;
    }

    public override void fullHealth() {
        healthPoints=100;
    }

    public override void activate(Vector3 position) {
        ShiftSystem.assignMobiles(new List<GameObject>{gameObject});
        gameObject.transform.position = position;
        getRigidBody2D();
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void deactivate() {
        GameManager.instance.playSound("death");
        GameManager.instance.killedMonsters++;
        ShiftSystem.distributeScoreToCharacters(10);
        gameObject.transform.position = basePosition;
        getRigidBody2D();
        rb2d.bodyType = RigidbodyType2D.Static;
        Planet.deleteMobile(gameObject);
    }

    public void calculateNextDirection() {
        xDirection = (UnityEngine.Random.Range(0,2) * 2) - 1;
        direction = new Vector3(xDirection,0f,0f);
    }

    private void getRigidBody2D() {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start() {
        fullHealth();
        calculateNextDirection();
        movementTimer = 5f;
        speed = 0.45f;
        isMyTurn = false;
        getRigidBody2D();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if(healthPoints<=0) {
            deactivate();
            fullHealth();
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