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

        return characterPositions[positionInList] - actualPosition;

    }

    protected override void shoot() {
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

    protected void move() {
        float xDirection = 1-(2*UnityEngine.Random.value);
        Vector3 direction = new Vector3(xDirection,0f,0f);
        while(movementTimer >= 0f) {
            movement(direction);
            movementTimer -= Time.deltaTime;
        }
        movementTimer = 10f;
    }

    public override void looseHealthPoints(int damage) {
        healthPoints-=(damage-10);
    }

    public override void fullHealth() {
        healthPoints=200;
        isAlive=true;
    }

    public override void activate(Vector3 position) {
        gameObject.transform.position = position;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void deactivate() {
        isAlive = false;
        gameObject.transform.position = basePosition;
        rb2d.bodyType = RigidbodyType2D.Static;
    }

    void Start() {
        fullHealth();
        areaOfEffect = 15f;
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