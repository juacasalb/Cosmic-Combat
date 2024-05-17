using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class PlanetShopMenu : MonoBehaviour { 
    public UIDocument document, characterShopDocument;
    private static string notEnoughScore, bought, jsonData, jsonUrl, userName;
    private static Label notEnoughScoreRedPlanetMsg, boughtRedPlanetMsg, 
    notEnoughScoreBluePlanetMsg, boughtBluePlanetMsg, playerScore;
    public static Sprite redPlanetBoughtSprite, bluePlanetBoughtSprite, redPlanetSprite, bluePlanetSprite;
    public Sprite _redPlanetBoughtSprite, _bluePlanetBoughtSprite, _redPlanetSprite, _bluePlanetSprite;
    private static Button goback, redPlanetBuy, bluePlanetBuy;
    private static VisualElement redPlanetDisplayer, bluePlanetDisplayer;
    private static PlayerDataContainer container;

    private void getDocuments() {
        document = GetComponent<UIDocument>();
        characterShopDocument = GameObject.FindWithTag("CharacterShop").GetComponent<UIDocument>();
    }

    private static void getJSONPath() {
        string url = Application.dataPath + "/Code/PlayerData.json";
        jsonUrl = url.Replace("/", "\\");
        jsonData = File.ReadAllText(jsonUrl);
    }

    private void getVisualElements() {
        notEnoughScore = "¡Faltan puntos!";
        bought = "COMPRADO";

        playerScore = document.rootVisualElement.Query<Label>("Score");
        boughtRedPlanetMsg = document.rootVisualElement.Query<Label>("RedPlanetBoughtLabel");
        boughtBluePlanetMsg = document.rootVisualElement.Query<Label>("BluePlanetBoughtLabel");
        notEnoughScoreRedPlanetMsg = document.rootVisualElement.Query<Label>("NotEnoughScoreRedPlanet");
        notEnoughScoreBluePlanetMsg = document.rootVisualElement.Query<Label>("NotEnoughScoreBluePlanet");
        redPlanetBuy = document.rootVisualElement.Query<Button>("RedPlanetScore");
        bluePlanetBuy = document.rootVisualElement.Query<Button>("BluePlanetScore");
        redPlanetDisplayer = document.rootVisualElement.Query<VisualElement>("RedPlanetDisplayer");
        bluePlanetDisplayer = document.rootVisualElement.Query<VisualElement>("BluePlanetDisplayer");
        goback = document.rootVisualElement.Query<Button>("GoBack");

        redPlanetBuy.RegisterCallback<ClickEvent>(evt => checkScoreToBuyRedPlanet(evt));
        bluePlanetBuy.RegisterCallback<ClickEvent>(evt => checkScoreToBuyBluePlanet(evt));
        goback.RegisterCallback<ClickEvent>(evt => goBack(evt));
    }

    public static void getPlanetsInUser() {
        checkScoreToBuyRedPlanet(null);
        checkScoreToBuyBluePlanet(null);
    }

    private void getSprites() {
        redPlanetBoughtSprite = _redPlanetBoughtSprite;
        bluePlanetBoughtSprite = _bluePlanetBoughtSprite;
        redPlanetSprite = _redPlanetSprite;
        bluePlanetSprite = _bluePlanetSprite;
    }

    void Awake() {
        getDocuments();
        getVisualElements();
        getSprites();
    }

    void Update() {
        getJSONPath();
    }

    private static PlayerData getUserFromJSON() {
        userName = GameManager.instance.playerUserName;
        container = JsonConvert.DeserializeObject<PlayerDataContainer>(jsonData);
        return container.playerData.Find(p => p.username == userName);
    }

    private static int getScoreFromJSON() {
        getJSONPath();
        PlayerData userFromJSON = getUserFromJSON();
        int userScore = userFromJSON.score;
        playerScore.text = userScore.ToString();
        return userScore;
    }

    private static void insertPlanetSkinInJSON(string planetName, int value) {
        container.playerData.Find(p => p.username == userName).earnedItems.Add(planetName);
        container.playerData.Find(p => p.username == userName).score -= value;
        playerScore.text = container.playerData.Find(p => p.username == userName).score.ToString();
        string updatedJsonData = JsonConvert.SerializeObject(container, Formatting.Indented);
        File.WriteAllText(jsonUrl, updatedJsonData);
    }

    private static void activateBuying(string planetName) {
        switch(planetName) {
            case "redplanet":
                redPlanetDisplayer.style.backgroundImage = redPlanetSprite.texture;
                redPlanetBuy.style.display = DisplayStyle.Flex;
                boughtRedPlanetMsg.text = "";
                notEnoughScoreRedPlanetMsg.text = "";
                break;
            default:
                bluePlanetDisplayer.style.backgroundImage = bluePlanetSprite.texture;
                bluePlanetBuy.style.display = DisplayStyle.Flex;
                boughtBluePlanetMsg.text = "";
                notEnoughScoreBluePlanetMsg.text = "";
                break;
        }
    }

    private static void desactivateBuying(string planetName) {
        switch(planetName) {
            case "redplanet":
                redPlanetDisplayer.style.backgroundImage = redPlanetBoughtSprite.texture;
                boughtRedPlanetMsg.text = bought;
                notEnoughScoreRedPlanetMsg.text = "";
                redPlanetBuy.style.display = DisplayStyle.None;
                break;
            default:
                bluePlanetDisplayer.style.backgroundImage = bluePlanetBoughtSprite.texture;
                boughtBluePlanetMsg.text = bought;
                notEnoughScoreBluePlanetMsg.text = "";
                bluePlanetBuy.style.display = DisplayStyle.None;
                break;
        }
    }

    private static bool checkPlanetSkinInJSON(string planetName) {
        bool isPlanetInJSON = false;
        PlayerData userFromJSON = getUserFromJSON();
        List<string> planetsInUser = userFromJSON.earnedItems;
        foreach (string planet in planetsInUser) {
            if (planetName.Equals(planet)) {
                isPlanetInJSON = true;
                desactivateBuying(planetName);
            }
        }
        return isPlanetInJSON;
    }

    private void goBack(ClickEvent evt) {
        GameManager.instance.playSound("button");
        document.sortingOrder = 0;
        CharacterShopMenu.getCharactersInUser();
    }

    private static void checkScoreToBuyRedPlanet(ClickEvent evt) {
        int value = 500;
        int score = getScoreFromJSON();
        string planetName = "redplanet";

        bool isRedPlanetBought = checkPlanetSkinInJSON(planetName);
        if(evt!=null) {
            if(score >= value && !isRedPlanetBought) {
                if(!isRedPlanetBought) {
                    insertPlanetSkinInJSON(planetName, value);
                    desactivateBuying(planetName);
                }
            } else {
                notEnoughScoreRedPlanetMsg.text = notEnoughScore;
            }
        } else {
            if(isRedPlanetBought) {
                desactivateBuying(planetName);
            } else {
                activateBuying(planetName);
            }
        }
    }

    private static void checkScoreToBuyBluePlanet(ClickEvent evt) {
        int value = 750;
        int score = getScoreFromJSON();
        string planetName = "blueplanet";

        bool isBluePlanetBought = checkPlanetSkinInJSON(planetName);
        if(evt!=null) {
            if(score >= value && !isBluePlanetBought) {
                if(!isBluePlanetBought) {
                    insertPlanetSkinInJSON(planetName, value);
                    desactivateBuying(planetName);
                }
            } else {
                notEnoughScoreBluePlanetMsg.text = notEnoughScore;
            }
        } else {
            if(isBluePlanetBought) {
                desactivateBuying(planetName);
            } else {
                activateBuying(planetName);
            }
        }
    }

}

