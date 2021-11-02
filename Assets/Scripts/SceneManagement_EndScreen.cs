using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement_EndScreen : MonoBehaviour
{
    /*
    General Note===========

    Have a system that uses incrementation or decrementation when moving scenes 
        instead of hardcoding it in the inspector
    */
    
    [SerializeField] private string nextScene;

    // When Player reaches the door, they will load 
    // the endScene variable that holds the scene named "End"
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    public void TitleScreenStart()
    {
        SceneManager.LoadScene(nextScene);
    }
}
