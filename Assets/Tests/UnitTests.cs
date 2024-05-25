using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitarianTests {
    // A Test behaves as an ordinary method
    [Test]
    public void checkMovementFunction() {
        CommonMonster testingMonster = new CommonMonster();
        Vector3 assertDirectionVector;

        testingMonster.calculateNextDirection();
        assertDirectionVector = (testingMonster.xDirection>0) ? Vector3.right : Vector3.left;

        Assert.AreEqual(assertDirectionVector, testingMonster.getDirection());
    }

    [Test]
    public void checkLoosingHealthFunction() {
        Character testingCharacter = new Character();
        testingCharacter.fullHealth();

        int testingCharacterHealthPoints = testingCharacter.getHealthPoints();
        int randomDamage = UnityEngine.Random.Range(0, testingCharacterHealthPoints+1);
        int assertHealthPoints = testingCharacterHealthPoints-randomDamage;
        testingCharacter.looseHealthPoints(randomDamage, false);

        Assert.AreEqual(assertHealthPoints, testingCharacter.getHealthPoints());
    }

    [Test]
    public void checkPlanetPointGenerationFunction() {
        float planetRadius = 3.35f;
        float tolerance = 0.001f;

        Vector3 testingPoint = RandomClass.getRandomPointOnPlanetSurface();
        float assertDistance = Mathf.Sqrt(testingPoint.x*testingPoint.x + testingPoint.y*testingPoint.y);

        Assert.AreEqual(assertDistance, planetRadius, tolerance);
    }

    [Test]
    public void checkGrabingMunitionFunction() {
        int assertMunition;
        int randomMunition = UnityEngine.Random.Range(0, 3);

        Character testingCharacter = new Character();
        testingCharacter.ammo = new List<int> {0, 0, 0}; 

        GameObject munitionObject = new GameObject("Munition");
        Munition munitionComponent = munitionObject.AddComponent<Munition>();
        munitionComponent.rb2d = munitionComponent.gameObject.AddComponent<Rigidbody2D>();

        switch(randomMunition) {
            case 0: 
                munitionComponent.weaponType = WeaponType.LaserRay;
                assertMunition = 2;
                break;
            case 1:
                munitionComponent.weaponType = WeaponType.Mine;
                assertMunition = 2;
                break;
            default:
                munitionComponent.weaponType = WeaponType.Lightsaber;
                assertMunition = 3;
                break;
        }

        testingCharacter.addAmmo(munitionObject);
        Assert.AreEqual(assertMunition, testingCharacter.ammo[randomMunition]);
    }

}
