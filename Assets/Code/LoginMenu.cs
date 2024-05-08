using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class PlayerDataContainer {
    public List<PlayerData> playerData { get; set; }
}

public class LoginMenu : MonoBehaviour {
    public UIDocument document;
    private string userNameString, passwordString, jsonData, jsonUrl;
    private Label incorrectPassword;
    private TextField userName, password;
    private Button accept;
    private PlayerDataContainer container;

    private void getJSONPath() {
        string url = Application.dataPath + "/Code/PlayerData.json";
        jsonUrl = url.Replace("/", "\\");
        jsonData = File.ReadAllText(jsonUrl);
    }

    private void getJSONData() {
        container = JsonConvert.DeserializeObject<PlayerDataContainer>(jsonData);
    } 

    private void writeInJSON() {
        PlayerData newPlayer = new PlayerData
        {
            username = userNameString,
            password = passwordString,
            score = 0,
            earnedItems = new List<string>()
        };

        container.playerData.Add(newPlayer);
        //Debug.Log(playerData.Count);
        string updatedJsonData = JsonConvert.SerializeObject(container, Formatting.Indented);
        //Debug.Log(updatedJsonData);
        File.WriteAllText(jsonUrl, updatedJsonData);
    }

    private void initialiseStrings() {
        userNameString = "";
        passwordString = "";
    }

    private void getVisualElements() {

        document = GetComponent<UIDocument>();
        incorrectPassword = document.rootVisualElement.Query<Label>("IncorrectPassword");
        userName = document.rootVisualElement.Query<TextField>("UserName");
        password = document.rootVisualElement.Query<TextField>("Password");
        accept = document.rootVisualElement.Query<Button>("Accept");

        userName.RegisterValueChangedCallback(evt => setUserName(evt));
        password.RegisterValueChangedCallback(evt => setPassword(evt));
        accept.RegisterCallback<ClickEvent>(evt => checkAcceptance(evt));
    }

    void Awake() {
        getVisualElements();
        initialiseStrings();
        getJSONPath();
    }

    private void mainMenu() {
        document.sortingOrder = 0;
    }

    private void setIncorrectPasswordText() {
        incorrectPassword.text = "¡LA CONTRASEÑA NO COINCIDE!";
    }

    private void voidStrings() {
        incorrectPassword.text = "¡INTRODUCE UN USUARIO Y CONTRASEÑA!";
    }

    private void checkAcceptance(ClickEvent evt) {
        bool isPlayerInJSON = false;
        getJSONData();

        if ((userNameString == "" || passwordString == "")) {
            voidStrings();
        } else {
            foreach (PlayerData player in container.playerData) {
                if (player.username.Equals(userNameString)) {
                    isPlayerInJSON = true;
                    if (player.password.Equals(passwordString)) {
                        incorrectPassword.text = "";
                        mainMenu();
                    } else {
                        setIncorrectPasswordText();
                    }
                    break;
                }
            }
            if(!isPlayerInJSON) {
                incorrectPassword.text = "";
                writeInJSON();
                mainMenu();
            }
        }
    }

    private void setUserName(ChangeEvent<string> evt) {
        string str = evt.newValue.ToString();
        userNameString = str;
    }

    private void setPassword(ChangeEvent<string> evt) {
        string str = evt.newValue.ToString();
        passwordString = str;
    }

}

