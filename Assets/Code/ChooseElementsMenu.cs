using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;

public class ChooseElementsMenu : MonoBehaviour {
    public UIDocument document;
    private static string jsonUrl, jsonData;
    public Sprite demonSprite, robotSprite, bluishSprite, slimySprite, redPlanetSprite, bluePlanetSprite, greenPlanetSprite;
    private static DropdownField character1, character2, character3, planet, gameMode;
    private Button goback, play;
    private VisualElement characterSpriter1, characterSpriter2, characterSpriter3, planetSpriter;
    private static PlayerDataContainer container;

    private void getVisualElements() {

        document = GetComponent<UIDocument>();
        character1 = document.rootVisualElement.Query<DropdownField>("CharacterSelector1");
        character2 = document.rootVisualElement.Query<DropdownField>("CharacterSelector2");
        character3 = document.rootVisualElement.Query<DropdownField>("CharacterSelector3");
        characterSpriter1 = document.rootVisualElement.Query<VisualElement>("CharacterSprite1");
        characterSpriter2 = document.rootVisualElement.Query<VisualElement>("CharacterSprite2");
        characterSpriter3 = document.rootVisualElement.Query<VisualElement>("CharacterSprite3");
        planetSpriter = document.rootVisualElement.Query<VisualElement>("PlanetSprite");
        planet = document.rootVisualElement.Query<DropdownField>("PlanetSelector");
        gameMode = document.rootVisualElement.Query<DropdownField>("ModeSelector");
        goback = document.rootVisualElement.Query<Button>("GoBack");
        play = document.rootVisualElement.Query<Button>("Play");

        character1.RegisterValueChangedCallback(evt => setNewCharacter1(evt));
        character2.RegisterValueChangedCallback(evt => setNewCharacter2(evt));
        character3.RegisterValueChangedCallback(evt => setNewCharacter3(evt));
        planet.RegisterValueChangedCallback(evt => setNewPlanet(evt));
        gameMode.RegisterValueChangedCallback(evt => setNewGameMode(evt));
        goback.RegisterCallback<ClickEvent>(evt => goBack(evt));
        play.RegisterCallback<ClickEvent>(evt => playGame(evt));
    }

    private static void getJSONPath() {
        string url = Application.persistentDataPath + "/PlayerData.json";
        jsonUrl = url.Replace("/", "\\");
        jsonData = File.ReadAllText(jsonUrl);
    }

    void Awake() {
        getVisualElements();
    }

    void Update() {
        getJSONPath();
    }

    public static void setDropdownFieldValues() {
        getJSONPath();
        PlayerData userFromJSON = getUserFromJSON();

        List<string> userEarnedItems, characters, planets;
        userEarnedItems = userFromJSON.earnedItems;
        characters = new List<string>{ "Baboso" };
        planets = new List<string>{ "Gamínedes" };

        foreach (string item in userEarnedItems) {
            switch(item) {
                case "demon":
                    characters.Add("Demonio");
                    break;
                case "bluish":
                    characters.Add("Azulín");
                    break;
                case "robot":
                    characters.Add("Robot");
                    break;
                case "redplanet":
                    planets.Add("Heliscopos");
                    break;
                case "blueplanet":
                    planets.Add("Andrómeda");
                    break;
                default:
                    break;
            }
        }

        character1.choices = characters;
        character2.choices = characters;
        character3.choices = characters;
        planet.choices = planets;
    }

    private static PlayerData getUserFromJSON() {
        string userName = GameManager.instance.playerUserName;
        container = JsonConvert.DeserializeObject<PlayerDataContainer>(jsonData);
        return container.playerData.Find(p => p.username.Equals(userName));
    }

    private void goBack(ClickEvent evt) {
        GameManager.instance.playSound("button");
        document.sortingOrder = 0;
    }

    private void playGame(ClickEvent evt) {
        GameManager.instance.playSound("button");
        SceneManager.LoadScene("DemoGame");
    }

    private void setNewCharacter1(ChangeEvent<string> evt) {
        string name;
        string str = evt.newValue.ToString();
        switch (str) {
            case "Baboso":
                name = "Slimy1";
                characterSpriter1.style.backgroundImage = slimySprite.texture;
                break;
            case "Robot":
                name = "Robot1";
                characterSpriter1.style.backgroundImage = robotSprite.texture;
                break;
            case "Azulín":
                name = "Bluish1";
                characterSpriter1.style.backgroundImage = bluishSprite.texture;
                break;
            default:
                name = "Demon1";
                characterSpriter1.style.backgroundImage = demonSprite.texture;
                break;
        }
        GameManager.instance.name1 = name;
    }

    private void setNewCharacter2(ChangeEvent<string> evt) {
        string name;
        string str = evt.newValue.ToString();
        switch (str) {
            case "Baboso":
                name = "Slimy2";
                characterSpriter2.style.backgroundImage = slimySprite.texture;
                break;
            case "Robot":
                name = "Robot2";
                characterSpriter2.style.backgroundImage = robotSprite.texture;
                break;
            case "Azulín":
                name = "Bluish2";
                characterSpriter2.style.backgroundImage = bluishSprite.texture;
                break;
            default:
                name = "Demon2";
                characterSpriter2.style.backgroundImage = demonSprite.texture;
                break;
        }
        GameManager.instance.name2 = name;
    }

    private void setNewCharacter3(ChangeEvent<string> evt) {
        string name;
        string str = evt.newValue.ToString();
        switch (str) {
            case "Baboso":
                name = "Slimy3";
                characterSpriter3.style.backgroundImage = slimySprite.texture;
                break;
            case "Robot":
                name = "Robot3";
                characterSpriter3.style.backgroundImage = robotSprite.texture;
                break;
            case "Azulín":
                name = "Bluish3";
                characterSpriter3.style.backgroundImage = bluishSprite.texture;
                break;
            default:
                name = "Demon3";
                characterSpriter3.style.backgroundImage = demonSprite.texture;
                break;
        }
        GameManager.instance.name3 = name;
    }

    private void setNewGameMode(ChangeEvent<string> evt) {
        bool mode;
        string str = evt.newValue.ToString();
        switch (str) {
            case "Cooperativo":
                mode = true;
                break;
            default:
                mode = false;
                break;
        }
        GameManager.instance.isCooperativeMode = mode;
    }

    private void setNewPlanet(ChangeEvent<string> evt) {
        string material;
        string str = evt.newValue.ToString();
        switch (str) {
            case "Gamínedes":
                material = "Verde";
                planetSpriter.style.backgroundImage = greenPlanetSprite.texture;
                break;
            case "Andrómeda":
                material = "Azul";
                planetSpriter.style.backgroundImage = bluePlanetSprite.texture;
                break;
            default:
                material = "Rojo";
                planetSpriter.style.backgroundImage = redPlanetSprite.texture;
                break;
        }
        GameManager.instance.planetMaterial = material;
    }

}

