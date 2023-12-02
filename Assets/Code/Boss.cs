using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster {

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void shoot() {
        // disparo con Ray
    }

    public override void looseHealthPoints(int damage) {
        
    }

    public override void fullHealth() {
        healthPoints=200;
        isAlive=true;
    }

    void Update() {

    }

}