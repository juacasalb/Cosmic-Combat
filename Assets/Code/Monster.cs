using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public enum monsterType {
        Ghost,
        Cyclops,
        Imp
    };

    protected Vector3 basePosition = new Vector3(20f,5f,0f);
    protected float speed;
    protected int healthPoints;

    protected virtual void movement(Vector3 direction) {
        
    }

    protected virtual void shoot() {
        
    }

    public virtual void looseHealthPoints(int damage) {
        
    }

    public virtual void fullHealth() {
        
    }

    public virtual void activate(Vector3 position) {
        
    }

    public virtual void deactivate() {

    }

    void Update() {
        
    }


}
