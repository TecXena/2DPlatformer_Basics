using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement_RestartActiveScene : MonoBehaviour
{
    private CheckNameOfActiveScene checkScene;

    void Start()
    {
        checkScene = GameObject.Find("Main Camera").GetComponent<CheckNameOfActiveScene>();
    } 

    void Update()
    {
        if(Input.GetKeyDown("r"))
        {
            ResetScene();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            checkScene.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void ResetScene()
    {
        checkScene.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
