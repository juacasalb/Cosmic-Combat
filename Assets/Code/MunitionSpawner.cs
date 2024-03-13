using System;
using UnityEngine;
using System.Collections.Generic;

public class MunitionSpawner : MonoBehaviour {
    private static List<GameObject> munitionList;

    public static void generateMunitionOnPlanetSurface() {
        int index = RandomClass.getRandomEntityIndex(munitionList);
        if(index!=-1) {
            Vector3 randomPoint = RandomClass.getRandomPointOnPlanetSurface();
            munitionList[index].GetComponent<Munition>().activate(randomPoint);
        }
    }

    void Start() {
        GameObject[] munitionObjects = GameObject.FindGameObjectsWithTag("Munition");
        munitionList = new List<GameObject>(munitionObjects);
    }

}