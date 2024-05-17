using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {
    public UIDocument document;
    private Slider music, sounds;
    private DropdownField difficulty, effects;
    private Button goback;

    private void getVisualElements() {

        document = GetComponent<UIDocument>();
        music = document.rootVisualElement.Query<Slider>("MusicSlider");
        sounds = document.rootVisualElement.Query<Slider>("SoundsSlider");
        difficulty = document.rootVisualElement.Query<DropdownField>("DifficultySelector");
        effects = document.rootVisualElement.Query<DropdownField>("EffectsSelector");
        goback = document.rootVisualElement.Query<Button>("GoBack");

        music.RegisterValueChangedCallback(evt => setNewMusicVolume(evt));
        sounds.RegisterValueChangedCallback(evt => setNewSoundsVolume(evt));
        difficulty.RegisterValueChangedCallback(evt => setNewDifficulty(evt));
        effects.RegisterValueChangedCallback(evt => setNewEffect(evt));
        goback.RegisterCallback<ClickEvent>(evt => goBack(evt));
    }

    void Awake() {
        getVisualElements();
    }

    private void goBack(ClickEvent evt) {
        GameManager.instance.playSound("button");
        document.sortingOrder = 0;
    }

    private void setNewMusicVolume(ChangeEvent<float> evt) {
        GameManager.instance.music.volume = evt.newValue/100f;
    }

    private void setNewSoundsVolume(ChangeEvent<float> evt) {
        GameManager.instance.sounds.volume = evt.newValue/100f;
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

