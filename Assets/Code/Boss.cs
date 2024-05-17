using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster {

    public bool isMyTurn;
    private float areaOfEffect;
    private float movementTimer;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isAlive;
    private Rigidbody2D rb2d;
    private int xDirection;
    private Vector3 direction;
    protected override void movement(Vector3 direction) { 
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private Vector3 getAnyCharacterPosition(Vector3 actualPosition) {
        Vector3 failedShotPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f), 0f);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(actualPosition, areaOfEffect);
        List<Vector3> characterPositions = new List<Vector3>{failedShotPosition};

        foreach (Collider2D collider in colliders) {
            if (collider.GetComponent<Character>() != null){
                    characterPositions.Add(collider.GetComponent<Character>().transform.position);
            }
        }

        int positionInList = (int)UnityEngine.Random.Range(0,characterPositions.Count);
        Vector3 finalPosition = characterPositions[positionInList] - actualPosition;

        return finalPosition;

    }

    protected override void shoot() {
        GameManager.instance.playSound("laserray");
        LaserRay laserRay = transform.Find("LaserRay").GetComponent<LaserRay>();
        string name = gameObject.name;

        Vector3 actualPosition = transform.position;
        Vector3 targetPosition = getAnyCharacterPosition(actualPosition);
        Vector2 normalizedPosition = targetPosition.normalized;

        float angle = Mathf.Atan2(normalizedPosition.y, normalizedPosition.x) * Mathf.Rad2Deg;
        laserRay.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        laserRay.gameObject.transform.position = new Vector3(
            actualPosition.x + normalizedPosition.x*0.5f, 
            actualPosition.y + normalizedPosition.y*0.5f);

        laserRay.gameObject.SetActive(true);
        laserRay._direction = normalizedPosition;
        laserRay.shooterName = name;
    }

    public override void looseHealthPoints(int damage) {
        GameManager.instance.playSound("damage");
        healthPoints-=(damage-10);
    }

    public override void fullHealth() {
        healthPoints=200;
    }

    public override void activate(Vector3 position) {
        ShiftSystem.assignMobiles(new List<GameObject>{gameObject});
        gameObject.transform.position = position;
        getRigidBody2D();
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void deactivate() {
        GameManager.instance.playSound("death");
        isAlive = false;
        ShiftSystem.distributeScoreToCharacters(150);
        GameManager.instance.isBossDefeated = true;
        gameObject.transform.position = basePosition;
        getRigidBody2D();
        rb2d.bodyType = RigidbodyType2D.Static;
        Planet.deleteMobile(gameObject);
    }

    private void calculateNextDirection() {
        xDirection = (UnityEngine.Random.Range(0,2) * 2) - 1;
        direction = new Vector3(xDirection,0f,0f);
    }

    private void getRigidBody2D() {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start() {
        fullHealth();
        calculateNextDirection();
        isAlive = true;
        movementTimer = 5f;
        speed = 0.45f;
        areaOfEffect = 15f;
        isMyTurn = false;
        getRigidBody2D();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if(healthPoints<=0 && isAlive) {
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
                movementTimer = 5f;
                xDirection=0;
                isMyTurn=false;
            }
        }
    }

}