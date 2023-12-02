using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    private float speed;
    private bool isAlive;
    private int healthPoints;
    private bool inShootMode;
    private List<int> ammo;
    private WeaponType currentWeapon;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void handleWanderMode() {
        float horizontalInput = Input.GetAxis("Horizontal");

        if(horizontalInput==0) {
            animator.SetBool("isMoving", false);
        } else {
            if(horizontalInput<0)
                spriteRenderer.flipX = true;
            else 
                spriteRenderer.flipX = false;
            animator.SetBool("isMoving", true);
        }

        transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
            inShootMode = true;
    }

    private void handleShootMode() {
        if (Input.GetKeyDown(KeyCode.A)) changeWeapon(-1);
        else if (Input.GetKeyDown(KeyCode.D)) changeWeapon(1);

        if (Input.GetMouseButtonDown(0))
            shoot();

        if (Input.GetKeyDown(KeyCode.Space))
            inShootMode = false;
    }

    private void changeWeapon(int direction) {
        int weaponCount = System.Enum.GetValues(typeof(WeaponType)).Length;
        int currentIndex = (int)currentWeapon;
        currentIndex = (currentIndex + direction + weaponCount) % weaponCount;
        currentWeapon = (WeaponType)currentIndex;
    }

    private void shoot() {
        switch (currentWeapon) {
            case WeaponType.Missile:
                break;
            case WeaponType.Ray:
                if(ammo[0]>0) {
                    ammo[0]-=1;
                }
                break;
            case WeaponType.Mine:
                if(ammo[1]>0) {
                    ammo[1]-=1;
                }
                break;
            case WeaponType.Lightsaber:
                if(ammo[2]>0) {
                    ammo[2]-=1;
                }
                break;
        }
    }

    private void looseHealthPoints(int damage) {
        healthPoints-=damage;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ray")) {
            looseHealthPoints(50);
        } else if (other.CompareTag("Mine")) {
            looseHealthPoints(50);
        } else if (other.CompareTag("Lightsaber")) {
            looseHealthPoints(40);
        } else if (other.CompareTag("Missile")) {
            looseHealthPoints(40);
        } else if (other.CompareTag("Munition")) {
            addAmmo(other);
        }
    }

    private void fullHealth() {
        healthPoints=100;
        isAlive=true;
    }
    
    private void addAmmo(Collider other) {
        Munition munition = other.GetComponent<Munition>();
        if(munition!=null) {
            WeaponType weaponType = munition.weaponType;
            switch(weaponType) {
                case WeaponType.Ray:
                    ammo[0]+=2;
                    break;
                case WeaponType.Mine:
                    ammo[1]+=2;
                    break;
                case WeaponType.Lightsaber:
                    ammo[2]+=3;
                    break;
            }
        }
    }

    private void Start() {
        speed = 0.35f;
        inShootMode = false;
        currentWeapon = WeaponType.Missile;
        ammo = new List<int> {0, 0, 0};
        fullHealth();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (!inShootMode)
            handleWanderMode();
        else
            handleShootMode();
        if(healthPoints<=0) {
            isAlive=false;
            gameObject.SetActive(false);
        }
    }
}
