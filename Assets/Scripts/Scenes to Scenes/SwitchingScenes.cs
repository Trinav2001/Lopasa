using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchingScenes : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private int target;
    

    public void loadNextScene()
    {
        target = PlayerPrefs.GetInt("PrevScene");
        
        if (target == 5) // the last level
        {
            PlayerPrefs.SetInt("Scenes", 2);
            SceneManager.LoadScene(0);
        }

        else
        {
            SceneManager.LoadScene(target + 1);
        }
    }

    public void loadStartScene(){
        int currentScreenIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentScreenIndex == 0)
        {
            SceneManager.LoadScene(2);
        }
        else 
        {
            SceneManager.LoadScene(0);
        }
   }
}