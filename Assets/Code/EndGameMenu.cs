using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour {
    public UIDocument document;
    private Button exit;

    private void getButtons() {
        document = GetComponent<UIDocument>();
        exit = document.rootVisualElement.Query<Button>("Exit");
    }

    private void OnEnable() {
        exit.RegisterCallback<ClickEvent>(evt => exitGame(evt));
    }

    private void OnDisable() {
        exit.UnregisterCallback<ClickEvent>(evt => exitGame(evt));
    }

    void Awake() {
        getButtons();
    }

    private void exitGame(ClickEvent evt) {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        ShiftSystem.isGamePaused = false;
    }

}

