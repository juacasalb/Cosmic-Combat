using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftSystem : MonoBehaviour {

    private int shiftCounter;
    public int totalShifts;
    private static Planet planet;
    private static GameObject planetGameObject;
    private List<Transform> mobiles;
    public static float shiftTimer;

    public static void deleteMobile(GameObject mobile) {
        planet.deleteMobile(mobile);
    }

    public static void assignMobiles(List<GameObject> list) {
        getPlanet();
        foreach(GameObject child in list) {
            if (planet != null && child != null) child.transform.SetParent(planet.transform);
            else Debug.Log(planet == null);
            
        }
    }

    public static List<Transform> findMobilesEntities() {
        return planet.getMobiles();
    }

    public void discardMobiles() {
        planet.discardMobiles(ref shiftCounter);
    }

    public void resetMobility() {
        planet.resetMobility();
    }

    private void checkSpawning() {
        if (totalShifts % 5 == 1) {
            GameManager.instance.monsterSpawning();
            GameManager.instance.munitionSpawning();
            GameManager.instance.munitionSpawning();
        }
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
        if(mobiles[shiftCounter].gameObject.GetComponent<Character>()!=null)
            mobiles[shiftCounter].gameObject.GetComponent<Character>().isMyTurn = true;

        Debug.Log("Es turno de:"+mobiles[shiftCounter].gameObject.name); //
    }

    public void shiftController() {
        shiftTimer -= Time.deltaTime;
        if(shiftTimer <= 0f) {
            discardMobiles();
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
        planet = GameObject.Find("Planet").GetComponent<Planet>();
    }

    void Start() {
        shiftCounter = 0;
        totalShifts = 1;
        shiftTimer = 0f;
    }

    void Update() {
        shiftController();
    }

}
