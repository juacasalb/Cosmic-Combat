using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour {

    public string playerUserName;
    public string planetMaterial;
    public AudioSource song;
    public bool areEffectsEnabled, isCooperativeMode, isBossDefeated;
    public float shiftDuration;
    public static int playerScore;
    public static GameManager instance = null;
    public int killedMonsters;
    public List<string> charactersInGame;
    public string name1,name2,name3, jsonData, jsonUrl;
    private PlayerDataContainer container;

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

    private void getJSONPath() {
        string url = Application.dataPath + "/Code/PlayerData.json";
        jsonUrl = url.Replace("/", "\\");
        jsonData = File.ReadAllText(jsonUrl);
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
        playerScore = 0;
        areEffectsEnabled = true;
        shiftDuration = 10f;
        planetMaterial = "Rojo";
        isCooperativeMode = true;
        isBossDefeated = false;
        song = GetComponent<AudioSource>();
    }
    
    void Update() {
        if(isBossDefeated) {
            rocketSpawning();
            isBossDefeated = false;
        }
        if(playerScore>0) {
            getJSONPath();
            container = JsonConvert.DeserializeObject<PlayerDataContainer>(jsonData);
            container.playerData.Find(p => p.username.Equals(playerUserName)).score += playerScore;
            string updatedJsonData = JsonConvert.SerializeObject(container, Formatting.Indented);
            File.WriteAllText(jsonUrl, updatedJsonData);
            playerScore = 0;
        }
    }

    void InitGame() {
        playerUserName = "admin";
        getJSONPath();
        name1 = "Slimy1";
        name2 = "Slimy2";
        name3 = "Slimy3";
    }
}
