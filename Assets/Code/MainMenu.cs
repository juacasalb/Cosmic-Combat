using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour {
    public UIDocument document, optionsDocument, chooseElementsDocument, loginDocument;
    private Button play, options, exit, goback;

    private void getDocuments() {
        document = GetComponent<UIDocument>();
        optionsDocument = GameObject.FindWithTag("Options").GetComponent<UIDocument>();
        chooseElementsDocument = GameObject.FindWithTag("ChooseElements").GetComponent<UIDocument>();
        loginDocument = GameObject.FindWithTag("Login").GetComponent<UIDocument>();
    }
    private void getButtons() {
        play = document.rootVisualElement.Query<Button>("Play");
        options = document.rootVisualElement.Query<Button>("Options");
        exit = document.rootVisualElement.Query<Button>("Exit");
        goback = document.rootVisualElement.Query<Button>("GoBack");

        play.RegisterCallback<ClickEvent>(evt => playGame(evt));
        options.RegisterCallback<ClickEvent>(evt => optionsMenu(evt));
        exit.RegisterCallback<ClickEvent>(evt => exitGame(evt));
        goback.RegisterCallback<ClickEvent>(evt => goBack(evt));
    }

    void Awake() {
        getDocuments();
        getButtons();
    }

    private void goBack(ClickEvent evt) {
        loginDocument.sortingOrder = 2;
    }

    private void playGame(ClickEvent evt) {
        chooseElementsDocument.sortingOrder = 2;
    }

    private void optionsMenu(ClickEvent evt) {
        optionsDocument.sortingOrder = 2;
    }

    private void exitGame(ClickEvent evt) {
        Application.Quit();
    }

}

