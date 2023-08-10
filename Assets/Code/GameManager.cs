using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //Variables
    public int points = 0;
    public int keys = 0;
    public bool mega_key = false;
    public int bombs = 0;
    public TextMeshProUGUI points_txt, keys_txt, bombs_txt;
    void Awake(){
        //Check if instance already exists
        if (instance == null)
            
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

        //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
        Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script

        //Call the InitGame function to initialize the first level 
        InitGame();
    }
    
    public void increasePoints(int i) {
        if(points < 99) {
            points+=i;
            points_txt.text = points.ToString();
        }
    }
    public void decreasePoints(int i) {
        points-=i;
        points_txt.text = points.ToString();
    }
    public void increaseKeys(int i) {
        keys+=i;
        keys_txt.text = ("x"+keys.ToString());
    }
    public void decreaseKeys(int i) {
        keys-=i;
        keys_txt.text = ("x"+keys.ToString());
    }
    public void increaseBombs(int i) {
        if(bombs < 3) {
            bombs+=i;
            bombs_txt.text = ("x"+bombs.ToString());
        }
    }
    public void decreaseBombs(int i) {
        bombs-=i;
        bombs_txt.text = ("x"+bombs.ToString());
    }
    public void obtainMegaKey() {
        mega_key = true;
    }
    void Update() {
        points_txt = UIController.instance.ui_points_txt;
        keys_txt = UIController.instance.ui_keys_txt;
        bombs_txt = UIController.instance.ui_bombs_txt;
    }
    void InitGame(){
        
    }
}
