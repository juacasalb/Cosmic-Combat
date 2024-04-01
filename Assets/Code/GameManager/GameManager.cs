using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    public int killedMonsters;

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

    private void monsterSpawning() {
        if(killedMonsters>=10) {
            MonsterSpawner.generateMonsterOnPlanetSurface(true);
            killedMonsters = int.MinValue;
        }
        else MonsterSpawner.generateMonsterOnPlanetSurface(false);
    }

    private void munitionSpawning() {
        MunitionSpawner.generateMunitionOnPlanetSurface();
    }

    void Start() {
        killedMonsters = 0;
    }
    
    void Update() {
        
    }
    void InitGame() {
        
    }
}
