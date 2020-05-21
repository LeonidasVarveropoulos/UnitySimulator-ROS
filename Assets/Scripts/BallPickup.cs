using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallPickup : MonoBehaviour
{
    private int numBalls = 3;
    public Text powerCellText;
    public int maxNumberBalls = 5;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PowerCell" && numBalls< maxNumberBalls)
        {
            numBalls++;
            Destroy(collision.collider.gameObject);
            updateText();
        }
    }

    public void subtractBall()
    {
        numBalls--;
        updateText();
    }

    void updateText()
    {
        powerCellText.text = "Power Cells: " + numBalls;
    }

    public int getNumberBalls()
    {
        return numBalls;
    }
}
