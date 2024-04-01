using System;
using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour {

    private static List<GameObject> monsterList;
    private static Boss boss;

    public static void generateMonsterOnPlanetSurface(bool spawnBoss) {
        int index = RandomClass.getRandomEntityIndex(monsterList);
        if(index!=-1) {
            Vector3 randomPoint = RandomClass.getRandomPointOnPlanetSurface();
            if(spawnBoss) boss.activate(randomPoint);
            else monsterList[index].GetComponent<CommonMonster>().activate(randomPoint);
        }
    }
    
    void Start() {
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");

        monsterList = new List<GameObject>(monsterObjects);
        boss = FindObjectOfType<Boss>();
    }

}