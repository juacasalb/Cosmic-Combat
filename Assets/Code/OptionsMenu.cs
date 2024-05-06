using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {
    public UIDocument document;
    private Slider volume;
    private DropdownField difficulty, effects;
    private Button goback;

    private void getVisualElements() {

        document = GetComponent<UIDocument>();
        volume = document.rootVisualElement.Query<Slider>("VolumeSlider");
        difficulty = document.rootVisualElement.Query<DropdownField>("DifficultySelector");
        effects = document.rootVisualElement.Query<DropdownField>("EffectsSelector");
        goback = document.rootVisualElement.Query<Button>("GoBack");

        volume.RegisterValueChangedCallback(evt => setNewVolume(evt));
        difficulty.RegisterValueChangedCallback(evt => setNewDifficulty(evt));
        effects.RegisterValueChangedCallback(evt => setNewEffect(evt));
        goback.RegisterCallback<ClickEvent>(evt => goBack(evt));
    }

    void Awake() {
        getVisualElements();
    }

    private void goBack(ClickEvent evt) {
        document.sortingOrder = 0;
    }

    private void setNewVolume(ChangeEvent<float> evt) {
        GameManager.instance.song.volume = evt.newValue/100f;
    }

    private void setNewDifficulty(ChangeEvent<string> evt) {
        float shiftSeconds;
        string str = evt.newValue.ToString();
        switch (str) {
            case "Fácil":
                shiftSeconds = 15f;
                break;
            case "Difícil":
                shiftSeconds = 7f;
                break;
            default:
                shiftSeconds = 10f;
                break;
        }
        GameManager.instance.shiftDuration = shiftSeconds;
    }

    private void setNewEffect(ChangeEvent<string> evt) {
        bool effectsState;
        string str = evt.newValue.ToString();
        switch (str) {
            case "No":
                effectsState = false;
                break;
            default:
                effectsState = true;
                break;
        }
        GameManager.instance.areEffectsEnabled = effectsState;
    }

}

