using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShiftSystem : MonoBehaviour {

    public static bool isCooperative;
    public static List<int> lifes;
    private static List<int> scores;
    private int shiftCounter;
    public int totalShifts;
    private static Planet planet;
    private static GameObject planetGameObject;
    private List<Transform> mobiles;
    public static float shiftTimer;
    private static GameObject entityOnCurrentTurn;

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
    private void gameOver() {
        Debug.Log("Game Over!");
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
        if(characterNumber != 67) scores[characterNumber-1]+=score;
        else scores[0]+=score;
        
    }

    public static void distributeScoreToCharacters(int score) {
        if(isCooperative) scores[0]+=score;
        else distributeToSingleCharacter(score);
    }

    private void calculateTurn() {
        mobiles = findMobilesEntities();

        foreach(Transform mobile in mobiles) {
            if(mobile.gameObject.GetComponent<CommonMonster>()!=null)
                mobile.gameObject.GetComponent<CommonMonster>().isMyTurn=false;
            else if(mobile.gameObject.GetComponent<Boss>()!=null)
                mobile.gameObject.GetComponent<Boss>().isMyTurn=false;
            else if(mobile.gameObject.GetComponent<Character>()!=null)
                mobile.gameObject.GetComponent<Character>().isMyTurn=false;
        }

        if(mobiles[shiftCounter].gameObject.GetComponent<CommonMonster>()!=null)
            mobiles[shiftCounter].gameObject.GetComponent<CommonMonster>().isMyTurn = true;
        if(mobiles[shiftCounter].gameObject.GetComponent<Boss>()!=null)
            mobiles[shiftCounter].gameObject.GetComponent<Boss>().isMyTurn = true;
        if(mobiles[shiftCounter].gameObject.GetComponent<Character>()!=null) {
            if(mobiles[shiftCounter].gameObject.GetComponent<Character>().isAlive==false)
                CharacterSpawner.generateCharacterOnPlanetSurface(mobiles[shiftCounter].gameObject);
            mobiles[shiftCounter].gameObject.GetComponent<Character>().isMyTurn = true;
        }
            

        if(lifes[0] <= 0 || lifes.Sum() <= 0) {
            gameOver();
        }

        entityOnCurrentTurn = mobiles[shiftCounter].gameObject;

        Debug.Log("Es turno de:"+mobiles[shiftCounter].gameObject.name); //
    }

    public void shiftController() {
        shiftTimer -= Time.deltaTime;
        if(shiftTimer <= 0f) {
            resetMobility();
            calculateTurn();
            checkSpawning();
            shiftCounter++;
            totalShifts++;
            shiftCounter = shiftCounter % mobiles.Count;
            shiftTimer = 10f;
        }
    }

    private static void getPlanet() {
        planet = GameObject.Find("Planet").GetComponent<Planet>();
    }

    void Awake() {
        getPlanet();
    }

    void Start() {
        isCooperative = true;
        shiftCounter = 0;
        totalShifts = 1;
        shiftTimer = 0f;
        lifes = new List<int>{3,3,3};
        scores = new List<int>{0,0,0};
    }

    void Update() {
        shiftController();
    }

}
