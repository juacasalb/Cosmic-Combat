using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftSystem : MonoBehaviour {

    private int shiftCounter;
    private static Planet planet;
    private List<Transform> mobiles;
    public static float shiftTimer;

    public static void deleteMobile(GameObject mobile) {
        planet.deleteMobile(mobile);
    }

    public static void assignMobiles(List<GameObject> list) {
        foreach(GameObject child in list)
            child.transform.SetParent(planet.transform);
    }

    public static List<Transform> findMobilesEntities() {
        return planet.getMobiles();
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

        Debug.Log("Es turno de:"+mobiles[shiftCounter].gameObject.name);
    }

    public void shiftController() {
        shiftTimer -= Time.deltaTime;
        if(shiftTimer <= 0f) {
            calculateTurn();
            shiftCounter++;
            shiftCounter = shiftCounter % mobiles.Count;
            shiftTimer = 40f;
        }
    }

    void Start() {
        shiftCounter = 0;
        shiftTimer = 0f;
        planet = GameObject.Find("Planet").GetComponent<Planet>();
    }

    void Update() {
        shiftController();
    }

}
