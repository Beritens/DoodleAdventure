using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public string firstScene;
    public void Continue(){
        if(PlayerPrefs.HasKey("currentScene")){
            SceneManager.LoadScene(PlayerPrefs.GetString("currentScene"));
        }
        else{
            newGame();
        }
    }
    public void newGame(){
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(firstScene);
    }
}
