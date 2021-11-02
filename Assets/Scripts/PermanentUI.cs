using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

/* This script works in tandem with CheckNameOfActiveScene, with PermanentUI occuring -1 frame before the start of the
    other scripts
    See Project Settings > Script Execution Order
 */

 /* Main Issue: Too confusing, this script does not reset since it is a singleton.
    Issues arises when maintaining variables that needs to be updated.
 */
public class PermanentUI : MonoBehaviour
{
    // Player Stats that carry on different scenes
    public int cherries;
    public int health;
    public TextMeshProUGUI collectableScore;
    public TextMeshProUGUI healthAmount;

    private CheckNameOfActiveScene checkSceneName;
    
    public static PermanentUI perm;
    // Static means that this variable belongs to the class itself
    // Everything can get access to this variable
    // But because this is only one, you can't make an instance/copy of it.


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        checkSceneName = GameObject.Find("Main Camera").GetComponent<CheckNameOfActiveScene>();

        // Makes it so that this thing can only exist once
        #region Singleton Pattern
        // If there is no permanent UI gameObject and active Scene is not equals to End
        if (!perm)
        {
            // This mean current instance
            // Perm variable will become this gameObject
            perm = this; 
        }
        else
        {
            // Destroy the other gameObject that has the perm variable
            Destroy(gameObject);
        }
        #endregion
    }

}
