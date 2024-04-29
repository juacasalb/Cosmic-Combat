using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public GameObject circlePrefab;
    public GameObject weaponSet;
    public bool isMyTurn;
    private bool isAITurn;
    private int isFirstMinePlaced;
    private List<GameObject> circles;
    private float speed;
    public bool isAlive;
    private int healthPoints;
    private bool inShootMode;
    private bool canShoot;
    private float shotCooldown;
    private Rigidbody2D rb2d;
    public List<int> ammo;
    public WeaponType currentWeapon;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector3 basePosition = new Vector3(30f,0f,0f);
    public int characterNumber;
    private int xDirection;
    private float movementIATimer;
    private Vector3 directionIA;
    private GameObject shiftSign;

    private void handleWanderMode() {
        setDots(false);
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
            isMyTurn = false;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            inShootMode = false;
        }
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

    private void setDotsOnScreen() {

        setDots(true);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.0f;

        Vector3 actualPosition = (mousePosition.normalized)*0.15f + transform.position;

        float i = 0.0f;
        foreach(var circle in circles) {
            Vector3 dotPosition = Vector3.Lerp(mousePosition,actualPosition,i);
            circle.transform.position = dotPosition;
            i+=0.2f;
        }
        
    }

    private void shootMissile(bool isShotByIACharacter) {
        Missile missile = weaponSet.transform.Find("Missile").GetComponent<Missile>();
        Vector3 actualPosition = transform.position;
        Vector3 mousePosition;
        if (isShotByIACharacter) {
            float randomX = UnityEngine.Random.Range(-1f, 1f);
            float randomY = UnityEngine.Random.Range(-1f, 1f);
            mousePosition = new Vector3(randomX, randomY, 0);
        }
        else mousePosition = clickCoordinates(actualPosition);
        
        Vector3 normalizedPosition = mousePosition.normalized;

        missile.gameObject.transform.position = new Vector3(actualPosition.x + normalizedPosition.x, 
            actualPosition.y + normalizedPosition.y);

        missile.gameObject.SetActive(true);
        missile._direction = normalizedPosition;
    }

    private void useLightsaber() {
        Lightsaber lightsaber = weaponSet.transform.Find("Lightsaber").GetComponent<Lightsaber>();
        Vector3 actualPosition = transform.position;
        Vector3 mousePosition = clickCoordinates(actualPosition);
        Vector3 normalizedPosition = mousePosition.normalized;

        lightsaber.gameObject.transform.position = new Vector3(actualPosition.x + (normalizedPosition.x*0.5f), 
            actualPosition.y + (normalizedPosition.y*0.5f));

        lightsaber.gameObject.SetActive(true);
    }

    private void shootLaserRay() {
        LaserRay laserRay = weaponSet.transform.Find("LaserRay").GetComponent<LaserRay>();
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
        List<Transform> mines = new List<Transform>{weaponSet.transform.Find("Mine1"), weaponSet.transform.Find("Mine2")};
        Vector3 actualPosition = transform.position;
        Vector3 mousePosition = clickCoordinates(actualPosition);
        Vector3 placePosition = Vector3.zero;
        placePosition.x = mousePosition.x >= 0.0f ? 1.0f : -1.0f;
        placePosition.y = mousePosition.y >= 0.0f ? 1.0f : -1.0f;

        mines[isFirstMinePlaced].transform.position = new Vector3(
            actualPosition.x + placePosition.x, 
            actualPosition.y + placePosition.y);
        mines[isFirstMinePlaced].gameObject.SetActive(true);

        isFirstMinePlaced = isFirstMinePlaced==0 ? 1 : 0;
        
    }

    private void shoot() {
        this.canShoot = false;
        switch (currentWeapon) {
            case WeaponType.Missile:
                shootMissile(false);
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
        handleWanderMode();
    }

    public void looseHealthPoints(int damage) {
        healthPoints-=damage;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<Munition>() != null) {
            addAmmo(other);
        }
    }

    private void fullHealth() {
        healthPoints=200;
    }

    private void instantiateShoot() {
        canShoot=true;
        shotCooldown=10f;
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
            other.gameObject.GetComponent<Munition>().deactivate();
        }
    }

    private void actionByIA() {
        if (movementIATimer >= 0.0f) {
            transform.Translate(directionIA * speed * Time.deltaTime);
        } else {
            shootMissile(true);
            isMyTurn = false;
            movementIATimer = 5f;
            xDirection = 0;
        }
    }

    void setDots(bool isActive) {
        foreach(var circle in circles) {
            circle.SetActive(isActive);
        }
    }

    void createDots() {
        circles = new List<GameObject>();
        for(int i = 0; i <= 5; i++) {
            GameObject circleInst = Instantiate(circlePrefab, new Vector3(15.0f, 0.0f, 0.0f), Quaternion.identity);
            circleInst.SetActive(true);
            circles.Add(circleInst);
        }
    }

    private void obtainNumberInName() {
        string name = gameObject.name;
        characterNumber = (int)name[name.Length - 1] - '0';
    }

    private void decreaseLifes() {
        ShiftSystem.lifesList[characterNumber-1]--;
    }

    public void activate(Vector3 position) {
        isAlive = true;
        gameObject.transform.position = position;
        getRigidBody2D();
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    public void deactivate() {
        isAlive = false;
        decreaseLifes();
        handleWanderMode();
        gameObject.transform.position = basePosition;
        getRigidBody2D();
        rb2d.bodyType = RigidbodyType2D.Static;
        if (ShiftSystem.lifesList[characterNumber-1] <= 0) Planet.deleteMobile(gameObject);
    }

    public int getHealthPoints() {
        return healthPoints;
    }

    private void getRigidBody2D() {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    public string translateCurrentWeapon() {
        string myweapon;
        switch (currentWeapon) {
            case WeaponType.LaserRay:
                myweapon = "Rayo Láser";
                break;
            case WeaponType.Mine:
                myweapon = "Mina";
                break;
            case WeaponType.Lightsaber:
                myweapon = "Sable Láser";
                break;
            default:
                myweapon = "Misil";
                break;
        }
        return myweapon;
    }

    private void Start() {
        speed = 0.35f;
        isMyTurn = false;
        inShootMode = false;
        currentWeapon = WeaponType.Missile;
        ammo = new List<int> {5, 5, 5}; //
        getRigidBody2D();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isFirstMinePlaced = 0; 
        xDirection = 0;
        movementIATimer = 5f;
        shiftSign = transform.GetChild(0).gameObject;

        obtainNumberInName();
        instantiateShoot();
        fullHealth();
        createDots();
        setDots(true);
    }

    private void Update() {
        if(healthPoints<=0) {
            deactivate();
            fullHealth();
        }

        if(xDirection==0) {
            xDirection = (UnityEngine.Random.Range(0,2) * 2) - 1;
            directionIA = new Vector3(xDirection,0f,0f);
        }

        if(isMyTurn) {
            movementIATimer -= Time.deltaTime;
            if(ShiftSystem.isCooperative || (!ShiftSystem.isCooperative && characterNumber==1)) {
                shiftSign.SetActive(true);
                if (!inShootMode)
                    handleWanderMode();
                else
                    handleShootMode();
                if(inShootMode || !isMyTurn)
                    setDotsOnScreen();
            } else if (!ShiftSystem.isCooperative && characterNumber>1) {
                actionByIA();
            }
        } else {
            setDots(false);
            shiftSign.SetActive(false);
        }
        setShotCooldown();
    }
}
