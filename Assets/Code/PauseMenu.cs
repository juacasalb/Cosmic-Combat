using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public UIDocument document;
    private Button restart, exit;

    private void getButtons() {
        document = GetComponent<UIDocument>();
        restart = document.rootVisualElement.Query<Button>("Restart");
        exit = document.rootVisualElement.Query<Button>("Exit");
    }

    private void OnEnable() {
        restart.RegisterCallback<ClickEvent>(evt => restartGame(evt));
        exit.RegisterCallback<ClickEvent>(evt => exitGame(evt));
    }

    private void OnDisable() {
        restart.UnregisterCallback<ClickEvent>(evt => restartGame(evt));
        exit.UnregisterCallback<ClickEvent>(evt => exitGame(evt));
    }

    void Awake() {
        getButtons();
    }

    private void restartGame(ClickEvent evt) {
        Time.timeScale = 1f;
        GameManager.instance.playSound("button");
        SceneManager.LoadScene("DemoGame");
        ShiftSystem.isGamePaused = false;
    }

    private void exitGame(ClickEvent evt) {
        Time.timeScale = 1f;
        GameManager.instance.playSound("button");
        SceneManager.LoadScene("MainMenu");
        ShiftSystem.isGamePaused = false;
    }

}

