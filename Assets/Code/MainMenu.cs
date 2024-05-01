using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public UIDocument document, optionsDocument;
    private Button play, options, exit;

    private void getDocuments() {
        document = GetComponent<UIDocument>();
        optionsDocument = GameObject.FindWithTag("Options").GetComponent<UIDocument>();
    }
    private void getButtons() {
        play = document.rootVisualElement.Query<Button>("Play");
        options = document.rootVisualElement.Query<Button>("Options");
        exit = document.rootVisualElement.Query<Button>("Exit");

        play.RegisterCallback<ClickEvent>(evt => playGame(evt));
        options.RegisterCallback<ClickEvent>(evt => optionsMenu(evt));
        exit.RegisterCallback<ClickEvent>(evt => exitGame(evt));
    }

    void Awake() {
        getDocuments();
        getButtons();
    }

    private void playGame(ClickEvent evt) {
        SceneManager.LoadScene("DemoGame");
    }

    private void optionsMenu(ClickEvent evt) {
        optionsDocument.sortingOrder = 2;
    }

    private void exitGame(ClickEvent evt) {
        Application.Quit();
    }

}

