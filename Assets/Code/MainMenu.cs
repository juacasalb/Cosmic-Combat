using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private UIDocument document;
    private Button play, options, exit;

    private void getButtons() {
        document = GetComponent<UIDocument>();
        play = document.rootVisualElement.Query<Button>("Play");
        options = document.rootVisualElement.Query<Button>("Options");
        exit = document.rootVisualElement.Query<Button>("Exit");

        play.RegisterCallback<ClickEvent>(evt => playGame(evt));
        options.RegisterCallback<ClickEvent>(evt => optionsMenu(evt));
        exit.RegisterCallback<ClickEvent>(evt => exitGame(evt));
    }

    void Awake() {
        getButtons();
    }

    private void playGame(ClickEvent evt) {
        SceneManager.LoadScene("DemoGame");
    }

    private void optionsMenu(ClickEvent evt) {
        //SceneManager.LoadScene("DemoGame");
    }

    private void exitGame(ClickEvent evt) {
        Application.Quit();
    }

}

