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
        //disparo con Missile
    }

    public override void looseHealthPoints(int damage) {
        
    }

    public override void fullHealth() {
        healthPoints=100;
        isAlive=true;
    }

    void Update() {
        
    }

}