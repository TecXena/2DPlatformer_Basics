using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_ContinuousMusic : MonoBehaviour
{
    private AudioSource bgMusic;
    // Start is called before the first frame update
    void Awake()
    {
        // Gets all of the objects with the same tag into an array
        GameObject[] objs = GameObject.FindGameObjectsWithTag("BgMusic");
        bgMusic = GetComponent<AudioSource>();

        // Checks if the number of objects (Length)
        //      in the array is greater than 1 
        if (objs.Length > 1)
        {
            // Destroys excess gameobjects that holds the bg music
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


}
