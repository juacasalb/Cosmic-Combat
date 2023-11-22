using System;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    private static float planetRadius;

    static EntitySpawner()
    {
        planetRadius = 3.35f;
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