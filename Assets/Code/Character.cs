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

    private void HandleWanderMode() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float lag = 90f;

        if(horizontalInput!=0 || verticalInput!=0)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

        transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);

        Vector3 planetCenter = Vector3.zero;
        Vector3 characterPosition = transform.position;
        float angle = Mathf.Atan2(characterPosition.y - planetCenter.y, characterPosition.x - planetCenter.x) * Mathf.Rad2Deg - lag;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Input.GetKeyDown(KeyCode.Space))
            inShootMode = true;
    }

    private void HandleShootMode() {
        if (Input.GetKeyDown(KeyCode.A)) ChangeWeapon(-1);
        else if (Input.GetKeyDown(KeyCode.D)) ChangeWeapon(1);

        if (Input.GetMouseButtonDown(0))
            Shoot();

        if (Input.GetKeyDown(KeyCode.Space))
            inShootMode = false;
    }

    private void ChangeWeapon(int direction) {
        int weaponCount = System.Enum.GetValues(typeof(WeaponType)).Length;
        int currentIndex = (int)currentWeapon;
        currentIndex = (currentIndex + direction + weaponCount) % weaponCount;
        currentWeapon = (WeaponType)currentIndex;
    }

    private void Shoot() {
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

    private void flip() {
        if (Input.GetKeyDown(KeyCode.A)) spriteRenderer.flipX = true;
        else if (Input.GetKeyDown(KeyCode.D)) spriteRenderer.flipX = false;
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
            HandleWanderMode();
        else
            HandleShootMode();
        if(healthPoints<=0) {
            isAlive=false;
            gameObject.SetActive(false);
        }
        
        flip();
    }
}
