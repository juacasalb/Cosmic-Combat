using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    private float speed;
    private bool isAlive;
    private int healthPoints;
    private bool inShootMode;
    private bool canShoot;
    private float shotCooldown;
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

        if (Input.GetMouseButtonDown(0) && canShoot) {
            shoot();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            inShootMode = false;
    }

    private void changeWeapon(int direction) {
        int weaponCount = System.Enum.GetValues(typeof(WeaponType)).Length;
        int currentIndex = (int)currentWeapon;
        currentIndex = (currentIndex + direction + weaponCount) % weaponCount;
        currentWeapon = (WeaponType)currentIndex;
        Debug.Log("El arma actual es:" + currentWeapon);
    }

    private Vector3 clickCoordinates(Vector3 actualPosition) {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)){
            mousePosition = hit.point;
            mousePosition = mousePosition - actualPosition;
        }
        return new Vector3(mousePosition.x, mousePosition.y, 0);
    }

    private void shootMissile() {
        Missile missile = transform.Find("Missile").GetComponent<Missile>();
        Vector3 actualPosition = transform.position;
        Vector3 mousePosition = clickCoordinates(actualPosition);
        Vector3 normalizedPosition = mousePosition.normalized;

        missile.gameObject.transform.position = new Vector3(actualPosition.x + normalizedPosition.x, 
            actualPosition.y + normalizedPosition.y);

        missile.gameObject.SetActive(true);
        missile._direction = normalizedPosition;
    }

    private void useLightsaber() {
        Lightsaber lightsaber = transform.Find("Lightsaber").GetComponent<Lightsaber>();
        lightsaber.gameObject.SetActive(true);
    }

    private void shootLaserRay() {
        LaserRay laserRay = transform.Find("LaserRay").GetComponent<LaserRay>();
        string name = gameObject.name;

        Vector3 actualPosition = transform.position;
        Vector3 mousePosition = clickCoordinates(actualPosition);
        Vector2 normalizedPosition = mousePosition.normalized;

        float angle = Mathf.Atan2(normalizedPosition.y, normalizedPosition.x) * Mathf.Rad2Deg;
        laserRay.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        laserRay.gameObject.transform.position = new Vector3(
            actualPosition.x + normalizedPosition.x*0.5f, 
            actualPosition.y + normalizedPosition.y*0.5f);

        laserRay.gameObject.SetActive(true);
        laserRay._direction = normalizedPosition;
        laserRay.shooterName = name;
    }

    private void placeMine() {
        Mine mine = GameObject.FindWithTag("Mine").GetComponent<Mine>();

        Vector3 actualPosition = transform.position;
        Vector3 mousePosition = clickCoordinates(actualPosition);
        Vector3 placePosition = Vector3.zero;
        placePosition.x = mousePosition.x >= 0.0f ? 1.0f : -1.0f;
        placePosition.y = mousePosition.y >= 0.0f ? 1.0f : -1.0f;

        mine.gameObject.transform.position = new Vector3(
            actualPosition.x + placePosition.x, 
            actualPosition.y + placePosition.y);
        
    }

    private void shoot() {
        this.canShoot = false;
        switch (currentWeapon) {
            case WeaponType.Missile:
                shootMissile();
                break;
            case WeaponType.LaserRay:
                if(ammo[0]>0) {
                    shootLaserRay();
                    ammo[0]-=1;
                }
                break;
            case WeaponType.Mine:
                if(ammo[1]>0) {
                    placeMine();
                    ammo[1]-=1;
                }
                break;
            case WeaponType.Lightsaber:
                if(ammo[2]>0) {
                    useLightsaber();
                    ammo[2]-=1;
                }
                break;
        }
    }

    public void looseHealthPoints(int damage) {
        healthPoints-=damage;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Munition")) {
            addAmmo(other);
        }
    }

    private void fullHealth() {
        healthPoints=200;
        isAlive=true;
    }

    private void instantiateShoot() {
        canShoot=true;
        shotCooldown=2f; //10f
    }

    private void setShotCooldown() {
        if(!canShoot) {
            shotCooldown-=Time.deltaTime;
        }
        if(shotCooldown<=0f) {
            instantiateShoot();
        }
    }
    
    private void addAmmo(Collision2D other) {
        Munition munition = other.gameObject.GetComponent<Munition>();
        if(munition!=null) {
            WeaponType weaponType = munition.weaponType;
            switch(weaponType) {
                case WeaponType.LaserRay:
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
        instantiateShoot();
        inShootMode = false;
        currentWeapon = WeaponType.Missile;
        ammo = new List<int> {5, 5, 5};
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
        setShotCooldown();
    }
}
