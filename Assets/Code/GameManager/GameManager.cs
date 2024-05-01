using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public AudioSource song;
    public bool areEffectsEnabled;
    public float shiftDuration;
    public static int playerScores;
    public static GameManager instance = null;
    public bool isGameFinished;
    public bool isBossDefeated;
    public int killedMonsters;
    public List<string> charactersInGame;

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
        playerScores = 0;
        areEffectsEnabled = true;
        shiftDuration = 10f;
        isBossDefeated = false;
        isGameFinished = false;
        song = GetComponent<AudioSource>();
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
        string name3 = "Demon3";
        charactersInGame = new List<string>{ name1, name2, name3 };
    }
}
