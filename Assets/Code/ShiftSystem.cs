using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ShiftSystem : MonoBehaviour {
    private const string qmark = "?";
    private UIDocument document, pauseMenuDocument, endGameMenuDocument;
    public static bool isCooperative, isGamePaused, isGameFinished;
    public static List<int> lifesList;
    private static List<int> scoresList;
    private int shiftCounter;
    public int totalShifts;
    private static Planet planet;
    private static GameObject planetGameObject;
    private List<Transform> mobiles;
    public static float shiftTimer;
    private static GameObject entityOnCurrentTurn;
    private string _lifes, _score, _lightsabers, _mines, _laserrays, _secondsleft, _selectedweapon, _shifts, _health;
    private Label lifes, score, lightsabers, mines, laserrays, secondsleft, selectedweapon, shifts, health, menutitle;

    public void setCharacters() {  
        List<string> characterNames = new List<string>{
            GameManager.instance.name1, 
            GameManager.instance.name2, 
            GameManager.instance.name3
        };
        
        List<GameObject> list = new List<GameObject>();
        foreach(string name in characterNames) {
            list.Add(GameObject.Find(name));
            CharacterSpawner.generateCharacterOnPlanetSurface(GameObject.Find(name));
        }
        assignMobiles(list);
    }

    public static void assignMobiles(List<GameObject> list) {
        getPlanet();
        foreach(GameObject child in list) {
            if (planet != null && child != null) child.transform.SetParent(planet.transform);        
        }
    }

    public static List<Transform> findMobilesEntities() {
        return planet.getMobiles();
    }

    public void resetMobility() {
        planet.resetMobility();
    }
    private void endGame(string title) {
        if(title.Equals("Has perdido...")) GameManager.instance.playSound("gameover");
        document.enabled = false;
        menutitle.text = title;
        Time.timeScale = 0f;
        endGameMenuDocument.enabled = true;
        isGamePaused = true;
        GameManager.playerScore = scoresList[0];
        scoresList[0] = 0;
    }

    private void checkGameStatus() {
        if(isGameFinished) endGame("¡Has ganado!");
    }

    private void detectPauseKeyCode() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(isGamePaused) {
                resumeGame();
            } else {
                pauseGame();
            }
        }
    }

    private void checkSpawning() {
        if (totalShifts % 5 == 1) {
            GameManager.instance.monsterSpawning();
            GameManager.instance.munitionSpawning();
            GameManager.instance.munitionSpawning();
        }
    }

    private static void distributeToSingleCharacter(int score) {
        string name = entityOnCurrentTurn.name;
        int characterNumber = name[name.Length - 1] - '0';
        if(characterNumber != 67) scoresList[characterNumber-1]+=score;
        else scoresList[0]+=score;
    }

    public static void distributeScoreToCharacters(int score) {
        if(isCooperative) scoresList[0]+=score;
        else distributeToSingleCharacter(score);
    }

    private void calculateTurn() {
        mobiles = findMobilesEntities();

        shiftCounter = shiftCounter % mobiles.Count;

        foreach(Transform mobile in mobiles) {
            if(mobile.gameObject.GetComponent<CommonMonster>()!=null)
                mobile.gameObject.GetComponent<CommonMonster>().isMyTurn=false;
            else if(mobile.gameObject.GetComponent<Boss>()!=null)
                mobile.gameObject.GetComponent<Boss>().isMyTurn=false;
            else if(mobile.gameObject.GetComponent<Character>()!=null)
                mobile.gameObject.GetComponent<Character>().isMyTurn=false;
        }

        if(mobiles[shiftCounter].gameObject.GetComponent<CommonMonster>()!=null) {
            mobiles[shiftCounter].gameObject.GetComponent<CommonMonster>().isMyTurn = true;
            health.text = _health + mobiles[shiftCounter].gameObject.GetComponent<CommonMonster>().getHealthPoints();
        }
        if(mobiles[shiftCounter].gameObject.GetComponent<Boss>()!=null) {
            mobiles[shiftCounter].gameObject.GetComponent<Boss>().isMyTurn = true;
            health.text = _health + mobiles[shiftCounter].gameObject.GetComponent<Boss>().getHealthPoints();
        }
        if(mobiles[shiftCounter].gameObject.GetComponent<Character>()!=null) {
            if(!mobiles[shiftCounter].gameObject.GetComponent<Character>().isAlive &&
                lifesList[mobiles[shiftCounter].gameObject.GetComponent<Character>().characterNumber-1]>0)
                CharacterSpawner.generateCharacterOnPlanetSurface(mobiles[shiftCounter].gameObject);
            mobiles[shiftCounter].gameObject.GetComponent<Character>().isMyTurn = true;
            health.text = _health + mobiles[shiftCounter].gameObject.GetComponent<Character>().getHealthPoints();
        }
            

        if(lifesList[0] <= 0 || lifesList.Sum() <= 0) {
            endGame("Has perdido...");
        }

        entityOnCurrentTurn = mobiles[shiftCounter].gameObject;
    }

    private void getMobileInfo() {
        if(entityOnCurrentTurn.GetComponent<Character>()!=null) {
            Character characterInfo = entityOnCurrentTurn.GetComponent<Character>();
            lightsabers.text = _lightsabers + characterInfo.ammo[2];
            mines.text = _mines + characterInfo.ammo[1];
            laserrays.text = _laserrays + characterInfo.ammo[0];
            selectedweapon.text = _selectedweapon + characterInfo.translateCurrentWeapon();

            if(!isCooperative) {
                int dataIndex = characterInfo.characterNumber-1;
                score.text = _score + scoresList[dataIndex];
                lifes.text = _lifes + lifesList[dataIndex];
            } else {
                score.text = _score + scoresList[0];
                lifes.text = _lifes + lifesList[0];
            }
        } else {
            lightsabers.text = _lightsabers + qmark;
            mines.text = _mines + qmark;
            laserrays.text = _laserrays + qmark;
            selectedweapon.text = _selectedweapon + qmark;
            lifes.text = _lifes + "∞";
            score.text = _score + qmark;
        }
    }

    public void shiftController() {
        shiftTimer -= Time.deltaTime;
        secondsleft.text = shiftTimer.ToString("F1") + _secondsleft;
        if(shiftTimer <= 0f) {
            resetMobility();
            calculateTurn();
            checkSpawning();
            shiftCounter++;
            totalShifts++;
            shifts.text = _shifts + totalShifts;
            shiftCounter = shiftCounter % mobiles.Count;
            shiftTimer = GameManager.instance.shiftDuration;
        }
        getMobileInfo();
        detectPauseKeyCode();
        checkGameStatus();
    }

    private void getLabels() {
        document = GetComponent<UIDocument>();
        lifes = document.rootVisualElement.Query<Label>("Lifes");
        score = document.rootVisualElement.Query<Label>("Score");
        lightsabers = document.rootVisualElement.Query<Label>("LightSaber");
        mines = document.rootVisualElement.Query<Label>("Mines");
        laserrays = document.rootVisualElement.Query<Label>("LaserRay");
        secondsleft = document.rootVisualElement.Query<Label>("ShiftInfo");
        selectedweapon = document.rootVisualElement.Query<Label>("SelectedWeapon");
        shifts = document.rootVisualElement.Query<Label>("TotalShifts");
        health = document.rootVisualElement.Query<Label>("HealthPoints");

        menutitle = endGameMenuDocument.rootVisualElement.Query<Label>("Menu");
    }

    private void setAuxText() {
        _lifes = lifes.text;
        _score = score.text;
        _lightsabers = lightsabers.text;
        _mines = mines.text;
        _laserrays = laserrays.text;
        _secondsleft = secondsleft.text;
        _selectedweapon = selectedweapon.text;
        _shifts = shifts.text;
        _health = health.text;
    }

    private static void getPlanet() {
        planet = GameObject.Find("Planet").GetComponent<Planet>();
    }

    private void pauseGame() {
        Time.timeScale = 0f;
        pauseMenuDocument.enabled = true;
        isGamePaused = true;
    }

    private void resumeGame() {
        Time.timeScale = 1f;
        pauseMenuDocument.enabled = false;
        isGamePaused = false;
    }

    private void getSideMenus() {
        pauseMenuDocument = GameObject.FindWithTag("Pause").GetComponent<UIDocument>();
        endGameMenuDocument = GameObject.FindWithTag("EndGame").GetComponent<UIDocument>();

    }

    void Awake() {
        getPlanet();
        getSideMenus();
        getLabels();
        setAuxText();
        setCharacters();
    }

    void Start() {
        isCooperative = GameManager.instance.isCooperativeMode;
        shiftCounter = 0;
        totalShifts = 0;
        shiftTimer = 0f;
        isGamePaused = false;
        isGameFinished = false;
        lifesList = new List<int>{3,3,3};
        scoresList = new List<int>{0,0,0};
    }

    void Update() {
        shiftController();
    }

}
