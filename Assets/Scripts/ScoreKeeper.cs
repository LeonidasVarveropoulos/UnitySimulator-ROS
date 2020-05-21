using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Text redScoreText;
    public Text blueScoreText;

    private int redScore = 0;
    private int blueScore = 0;

    public void addScoreRed(int points)
    {
        redScore += points;
        updateRedScore();
    }

    public void addScoreBlue(int points)
    {
        blueScore += points;
        updateBlueScore();
    }

    void updateRedScore()
    {
        redScoreText.text = "Red Score: " + redScore;
    }

    void updateBlueScore()
    {
        blueScoreText.text = "Blue Score: " + blueScore;
    }
}
