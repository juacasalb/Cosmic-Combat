using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    private float movementSpeed = 1f;
    private bool isAlive;
    private int healthPoints;
    private bool inShootMode;
    private List<int> ammo;
    private WeaponType currentWeapon;

    private void HandleWanderMode() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * movementSpeed * Time.deltaTime);

        Vector3 planetCenter = Vector3.zero;
        Vector3 characterPosition = transform.position;
        float angle = Mathf.Atan2(characterPosition.y - planetCenter.y, characterPosition.x - planetCenter.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

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
                // Código Missile
                break;
            case WeaponType.Ray:
                // Código Ray
                break;
            case WeaponType.Mine:
                // Código Mine
                break;
            case WeaponType.Lightsaber:
                // Código Lightsaber
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
        inShootMode = false;
        currentWeapon = WeaponType.Missile;
        ammo = new List<int> {0, 0, 0};
        fullHealth();
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
    }
}
