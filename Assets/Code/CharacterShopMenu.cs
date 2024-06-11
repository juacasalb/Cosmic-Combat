using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class CharacterShopMenu : MonoBehaviour { 
    public UIDocument document, planetShopDocument;
    private static string notEnoughScore, bought, jsonData, jsonUrl, userName;
    private static Label notEnoughScoreDemonMsg, boughtDemonMsg, notEnoughScoreBluishMsg, 
    boughtBluishMsg, notEnoughScoreRobotMsg, boughtRobotMsg, playerScore;
    public static Sprite demonBoughtSprite, robotBoughtSprite, bluishBoughtSprite, demonSprite, robotSprite, bluishSprite;
    public Sprite _demonBoughtSprite, _robotBoughtSprite, _bluishBoughtSprite, _demonSprite, _robotSprite, _bluishSprite;
    private static Button goback, goforward, demonBuy, bluishBuy, robotBuy;
    private static VisualElement demonDisplayer, bluishDisplayer, robotDisplayer;
    private static PlayerDataContainer container;

    private void getDocuments() {
        document = GetComponent<UIDocument>();
        planetShopDocument = GameObject.FindWithTag("PlanetShop").GetComponent<UIDocument>();
    }

    private static void getJSONPath() {
        string url = Application.persistentDataPath + "/PlayerData.json";
        jsonUrl = url.Replace("/", "\\");
        jsonData = File.ReadAllText(jsonUrl);
    }

    private void getVisualElements() {
        notEnoughScore = "¡Faltan puntos!";
        bought = "COMPRADO";

        playerScore = document.rootVisualElement.Query<Label>("Score");
        boughtDemonMsg = document.rootVisualElement.Query<Label>("DemonBoughtLabel");
        boughtRobotMsg = document.rootVisualElement.Query<Label>("RobotBoughtLabel");
        boughtBluishMsg = document.rootVisualElement.Query<Label>("BluishBoughtLabel");
        notEnoughScoreDemonMsg = document.rootVisualElement.Query<Label>("NotEnoughScoreDemon");
        notEnoughScoreBluishMsg = document.rootVisualElement.Query<Label>("NotEnoughScoreBluish");
        notEnoughScoreRobotMsg = document.rootVisualElement.Query<Label>("NotEnoughScoreRobot");
        demonBuy = document.rootVisualElement.Query<Button>("DemonScore");
        bluishBuy = document.rootVisualElement.Query<Button>("BluishScore");
        robotBuy = document.rootVisualElement.Query<Button>("RobotScore");
        demonDisplayer = document.rootVisualElement.Query<VisualElement>("DemonDisplayer");
        bluishDisplayer = document.rootVisualElement.Query<VisualElement>("BluishDisplayer");
        robotDisplayer = document.rootVisualElement.Query<VisualElement>("RobotDisplayer");
        goback = document.rootVisualElement.Query<Button>("GoBack");
        goforward = document.rootVisualElement.Query<Button>("GoForward");

        demonBuy.RegisterCallback<ClickEvent>(evt => checkScoreToBuyDemon(evt));
        bluishBuy.RegisterCallback<ClickEvent>(evt => checkScoreToBuyBluish(evt));
        robotBuy.RegisterCallback<ClickEvent>(evt => checkScoreToBuyRobot(evt));
        goback.RegisterCallback<ClickEvent>(evt => goBack(evt));
        goforward.RegisterCallback<ClickEvent>(evt => goForward(evt));
    }

    public static void getCharactersInUser() {
        checkScoreToBuyDemon(null);
        checkScoreToBuyBluish(null);
        checkScoreToBuyRobot(null);
    }

    private void getSprites() {
        demonBoughtSprite = _demonBoughtSprite;
        robotBoughtSprite = _robotBoughtSprite;
        bluishBoughtSprite = _bluishBoughtSprite;
        demonSprite = _demonSprite;
        robotSprite = _robotSprite;
        bluishSprite = _bluishSprite;
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
        return container.playerData.Find(p => p.username.Equals(userName));
    }

    private static int getScoreFromJSON() {
        getJSONPath();
        PlayerData userFromJSON = getUserFromJSON();
        int userScore = userFromJSON.score;
        playerScore.text = userScore.ToString();
        return userScore;
    }

    private static void insertCharacterSkinInJSON(string characterName, int value) {
        container.playerData.Find(p => p.username == userName).earnedItems.Add(characterName);
        container.playerData.Find(p => p.username == userName).score -= value;
        playerScore.text = container.playerData.Find(p => p.username == userName).score.ToString();
        string updatedJsonData = JsonConvert.SerializeObject(container, Formatting.Indented);
        File.WriteAllText(jsonUrl, updatedJsonData);
    }

    private static void activateBuying(string characterName) {
        switch(characterName) {
            case "demon":
                demonDisplayer.style.backgroundImage = demonSprite.texture;
                demonBuy.style.display = DisplayStyle.Flex;
                boughtDemonMsg.text = "";
                notEnoughScoreBluishMsg.text = "";
                break;
            case "bluish":
                bluishDisplayer.style.backgroundImage = bluishSprite.texture;
                bluishBuy.style.display = DisplayStyle.Flex;
                boughtBluishMsg.text = "";
                notEnoughScoreBluishMsg.text = "";
                break;
            default:
                robotDisplayer.style.backgroundImage = robotSprite.texture;
                robotBuy.style.display = DisplayStyle.Flex;
                boughtRobotMsg.text = "";
                notEnoughScoreRobotMsg.text = "";
                break;
        }
    }

    private static void desactivateBuying(string characterName) {
        switch(characterName) {
            case "demon":
                demonDisplayer.style.backgroundImage = demonBoughtSprite.texture;
                boughtDemonMsg.text = bought;
                notEnoughScoreDemonMsg.text = "";
                demonBuy.style.display = DisplayStyle.None;
                break;
            case "bluish":
                bluishDisplayer.style.backgroundImage = bluishBoughtSprite.texture;
                boughtBluishMsg.text = bought;
                notEnoughScoreBluishMsg.text = "";
                bluishBuy.style.display = DisplayStyle.None;
                break;
            default:
                robotDisplayer.style.backgroundImage = robotBoughtSprite.texture;
                boughtRobotMsg.text = bought;
                notEnoughScoreRobotMsg.text = "";
                robotBuy.style.display = DisplayStyle.None;
                break;
        }
    }

    private static bool checkCharacterSkinInJSON(string characterName) {
        bool isCharacterInJSON = false;
        PlayerData userFromJSON = getUserFromJSON();
        List<string> charactersInUser = userFromJSON.earnedItems;
        foreach (string character in charactersInUser) {
            if (characterName.Equals(character)) {
                isCharacterInJSON = true;
                desactivateBuying(characterName);
            }
        }
        return isCharacterInJSON;
    }

    private void goBack(ClickEvent evt) {
        GameManager.instance.playSound("button");
        document.sortingOrder = 0;
    }

    private void goForward(ClickEvent evt) {
        GameManager.instance.playSound("button");
        planetShopDocument.sortingOrder = 3;
        PlanetShopMenu.getPlanetsInUser();
    }

    private static void checkScoreToBuyDemon(ClickEvent evt) {
        int value = 250;
        int score = getScoreFromJSON();
        string characterName = "demon";

        bool isDemonBought = checkCharacterSkinInJSON(characterName);
        if(evt!=null) {
            if(score >= value && !isDemonBought) {
                if(!isDemonBought) {
                    insertCharacterSkinInJSON(characterName, value);
                    desactivateBuying(characterName);
                }
            } else {
                notEnoughScoreDemonMsg.text = notEnoughScore;
            }
        } else {
            if(isDemonBought) {
                desactivateBuying(characterName);
            } else {
                activateBuying(characterName);
            }
        }
    }

    private static void checkScoreToBuyBluish(ClickEvent evt) {
        int value = 1000;
        int score = getScoreFromJSON();
        string characterName = "bluish";

        bool isBluishBought = checkCharacterSkinInJSON(characterName);
        if(evt!=null) {
            if(score >= value && !isBluishBought) {
                if(!isBluishBought) {
                    insertCharacterSkinInJSON(characterName, value);
                    desactivateBuying(characterName);
                }
            } else {
                notEnoughScoreBluishMsg.text = notEnoughScore;
            }
        } else {
            if(isBluishBought) {
                desactivateBuying(characterName);
            } else {
                activateBuying(characterName);
            }
        }
    }

    private static void checkScoreToBuyRobot(ClickEvent evt) { 
        int value = 500;
        int score = getScoreFromJSON();
        string characterName = "robot";

        bool isRobotBought = checkCharacterSkinInJSON(characterName);
        if(evt!=null) {
            if(score >= value && !isRobotBought) {
                if(!isRobotBought) {
                    insertCharacterSkinInJSON(characterName, value);
                    desactivateBuying(characterName);
                }
            } else {
                notEnoughScoreRobotMsg.text = notEnoughScore;
            }
        } else {
            if(isRobotBought) {
                desactivateBuying(characterName);
            } else {
                activateBuying(characterName);
            }
        }
    }

}

