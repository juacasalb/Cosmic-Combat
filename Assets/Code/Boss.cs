using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster {

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isAlive;
    private Rigidbody2D rb2d;

    void Start() {
        fullHealth();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void shoot() {
        // disparo con Ray
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

    void Update() {
        if(healthPoints<=0) {
            deactivate();
        }
    }

}