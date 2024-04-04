using System;
using UnityEngine;
using System.Collections.Generic;

public class CharacterSpawner : MonoBehaviour {
    private static List<GameObject> characterList;

    public static void generateCharacterOnPlanetSurface() {
        int index = RandomClass.getRandomEntityIndex(characterList);
        if(index!=-1) {
            Vector3 randomPoint = RandomClass.getRandomPointOnPlanetSurface();
            characterList[index].GetComponent<Character>().activate(randomPoint);
        }
    }

    void Start() {
        GameObject[] characterObjects = GameObject.FindGameObjectsWithTag("Character");
        characterList = new List<GameObject>(characterObjects);
    }

}