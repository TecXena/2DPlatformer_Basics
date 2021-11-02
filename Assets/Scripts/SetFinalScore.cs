using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* This script works in tandem with PermanentUI and CheckNameOfActiveScene, this has to run late in order to get
    the value of CheckName Script which is delayed by 10 frames
        See Project Settings > Script Execution Order
 */
public class SetFinalScore : MonoBehaviour
{
    private CheckNameOfActiveScene accessScore;
    public TextMeshProUGUI finalScoretext;


    void Start()
    {
        accessScore = GameObject.Find("Main Camera").GetComponent<CheckNameOfActiveScene>();
        SetScore();
    }

    private void SetScore()
    {
        finalScoretext.text = "Final Cherries: " + accessScore.previous_Cherries.ToString();
    }

}
