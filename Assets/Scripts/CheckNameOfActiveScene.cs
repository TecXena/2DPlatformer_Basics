using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* This script works in tandem with PermanentUI because this script occurs 10 frames after the default loading time
    See Project Settings > Script Execution Order
 */

/* Main Issue: Too confusing, not only can you not rename the script, but it handles two different functions
    Additionally, this should be just handled in one script.
 */
public class CheckNameOfActiveScene : MonoBehaviour
{

    public List<string> NoUIScenes = new List<string>();
    public string activeScene;
    public bool isPlayTesting;
    public int previous_Cherries;
    public int previous_Health;
    private GameObject playerUI;
    private PermanentUI permUI;
    
    

#region Checking Active Scene
    void Awake()
    {
        CheckActiveScene();
        Debug.Log("Awake is Running");
    }
#endregion

#region For Persisting Player Data & Deleting Player UI
    void Start()
    {
        // Checks if the permUI is null to continue on the scene without it
        if (PermanentUI.perm != null)
        {   
            // Grabs the Player UI and PermUI
            playerUI = GameObject.Find("Player Ui");
            permUI = playerUI.GetComponent<PermanentUI>();
        }

        if (isPlayTesting == false)
        {
            // Saves the value of the variables when this script is loaded in the scene
            previous_Cherries = permUI.cherries;
            previous_Health = permUI.health;
            Debug.Log("Saved Cherries: " + previous_Cherries + " Saved Health: " + previous_Health);
        }

        // Note: Find a better and flexible method to assign the scene that will have no UI
        if (activeScene.Equals("End") || activeScene.Equals("TitleScreen"))
        {
            Destroy(playerUI);
        }   
    }
#endregion

#region Custom Methods
    public void CheckActiveScene()
    {
        activeScene = SceneManager.GetActiveScene().name;
        Debug.Log(activeScene);
    }

    public void Reset()
    {
        // Resets the scores to the values that persisted when switching scenes
        permUI.cherries = previous_Cherries;
        permUI.health = previous_Health;

        // Updates this score
        permUI.collectableScore.text = permUI.cherries.ToString();
        permUI.healthAmount.text = permUI.health.ToString();
    }
#endregion
    



}
