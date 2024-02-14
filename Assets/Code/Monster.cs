using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public enum monsterType {
        Ghost,
        Cyclops,
        Imp
    };

    protected float speed;
    protected bool isAlive;
    protected int healthPoints;

    protected void movement(Vector3 direction) {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    protected virtual void shoot() {
        
    }

    public virtual void looseHealthPoints(int damage) {
        
    }

    public virtual void fullHealth() {
        
    }

    void Update() {
        
    }


}
