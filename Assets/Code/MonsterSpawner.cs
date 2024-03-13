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
        Debug.Log(index + " " + monsterList.Count);
    }
    
    void Start() {
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");
        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");

        monsterList = new List<GameObject>(monsterObjects);
        boss = bossObject.GetComponent<Boss>();
    }

}