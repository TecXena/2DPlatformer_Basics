using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement_ExitOrRestartGame : MonoBehaviour
{
    private CheckNameOfActiveScene checkScene;
    private bool isPlayTesting;

    void Start()
    {
        checkScene = GameObject.Find("Main Camera").GetComponent<CheckNameOfActiveScene>();
        isPlayTesting = checkScene.isPlayTesting;
    } 

    public void DoExitGame() 
    {
        Application.Quit();
        Debug.Log("Quited Game");
    }

    public void RestartWholeGame()
    {
        if (isPlayTesting == false)
        {
            PermanentUI.perm.cherries = 0;
            PermanentUI.perm.health = 5;
        }
        SceneManager.LoadScene(0);
        Debug.Log("Restarted Game");
    }
}
