using System;
using UnityEngine;
using System.Collections.Generic;

public class RandomClass : MonoBehaviour {
    private static float planetRadius = 3.35f;

    public static int getRandomEntityIndex(List<GameObject> list) {
        List<int> positions = new List<int>();
        int index = -1;
        bool noNumbers = false;
        while(index==-1 && !noNumbers) {
            int entityIndex = UnityEngine.Random.Range(0,list.Count);
            if(!(positions.Contains(entityIndex)))
                positions.Add(entityIndex);
            if(list[entityIndex].transform.position.x > 10f)
                index = entityIndex;
            if(positions.Count == list.Count)
                noNumbers = true;
        }
        return index;
    }

    public static Vector3 getRandomPointOnPlanetSurface() {
        int isXNegative = (UnityEngine.Random.Range(0,2) * 2) - 1;
        int isYNegative = (UnityEngine.Random.Range(0,2) * 2) - 1;

        float randomX = UnityEngine.Random.Range(0f, planetRadius);
        float randomY = (float)Math.Sqrt(Math.Pow(planetRadius, 2) - Math.Pow(randomX, 2));

        float x = randomX * isXNegative;
        float y = randomY * isYNegative;
        float z = 0;

        return new Vector3(x, y, z);
    }

}