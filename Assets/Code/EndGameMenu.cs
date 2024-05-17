using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour {
    public UIDocument document, hudDocument;
    private Button exit;

    private void getDocuments() {
        document = GetComponent<UIDocument>();
        hudDocument = GameObject.FindWithTag("HUD").GetComponent<UIDocument>();
    }

    private void getButtons() {
        exit = document.rootVisualElement.Query<Button>("Exit");
    }

    private void OnEnable() {
        exit.RegisterCallback<ClickEvent>(evt => exitGame(evt));
    }

    private void OnDisable() {
        exit.UnregisterCallback<ClickEvent>(evt => exitGame(evt));
    }

    void Awake() {
        getDocuments();
        getButtons();
    }

    private void exitGame(ClickEvent evt) {
        GameManager.instance.playSound("button");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        hudDocument.enabled = true;
        ShiftSystem.isGamePaused = false;
    }

}

