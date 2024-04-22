using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public bool isGameFinished;
    public bool isBossDefeated;
    public int killedMonsters;
    private List<string> charactersInGame;

    void Awake(){
        //Check if instance already exists
        if (instance == null)
            
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

        //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
        Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    private void putCharactersOnPlanet() {
        List<GameObject> list = new List<GameObject>();
        foreach(string name in charactersInGame) {
            list.Add(GameObject.Find(name));
            characterSpawning(GameObject.Find(name));
        }
        ShiftSystem.assignMobiles(list);
    }

    public void characterSpawning(GameObject character) {
        CharacterSpawner.generateCharacterOnPlanetSurface(character);
    }

    public void monsterSpawning() {
        if(killedMonsters>=10) {
            MonsterSpawner.generateMonsterOnPlanetSurface(true);
            killedMonsters = int.MinValue;
        }
        else MonsterSpawner.generateMonsterOnPlanetSurface(false);
    }

    public void munitionSpawning() {
        MunitionSpawner.generateMunitionOnPlanetSurface();
    }

    public void rocketSpawning() {
        Rocket.generateRocketOnPlanetSurface();
    }

    void Start() {
        killedMonsters = 0;
        isBossDefeated = false;
        isGameFinished = false;
    }
    
    void Update() {
        if(isBossDefeated) {
            rocketSpawning();
            isBossDefeated = false;
        }
        if(isGameFinished) {
            Debug.Log("You win!"); //
            isGameFinished = false;
        }
    }

    void InitGame() {
        string name1 = "Demon1";
        string name2 = "Demon2";
        charactersInGame = new List<string>{ name1, name2 };
        putCharactersOnPlanet();
    }
}
