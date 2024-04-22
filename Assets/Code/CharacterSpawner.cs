using System;
using UnityEngine;
using System.Collections.Generic;

public class CharacterSpawner : MonoBehaviour {
    private static List<GameObject> characterList;

    public static void generateCharacterOnPlanetSurface(GameObject character) {
        Vector3 randomPoint = RandomClass.getRandomPointOnPlanetSurface();
        character.GetComponent<Character>().activate(randomPoint);
    }

    void Start() {
        
    }

}