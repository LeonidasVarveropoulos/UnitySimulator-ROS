using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGoal : MonoBehaviour
{
    private ScoreKeeper scoreKeeper;
    public int pointsPerGoal = 0;

    void Awake()
    {
        scoreKeeper = GameObject.Find("ScoreKeeper").GetComponent<ScoreKeeper>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PowerCell")
        {
            scoreKeeper.addScoreRed(pointsPerGoal);
            Destroy(collision.collider.gameObject);
        }
    }
}
