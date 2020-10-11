using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntakeControl : MonoBehaviour
{

    [Header("Ball Pickup")]
    public Text powerCellText;
    public int maxNumberBalls = 5;
    public int numBalls = 3;
    public float timeOfBallContact = 1.0f;
    public string coliderTag = "PowerCell";

    [Header("Intake Motor")]
    public float wantedVelocity = 150f;

    private float timer = 0.0f;

    // Intake Motor Control
    void Start()
    {
        retractIntake();
    }

    public void deployIntake()
    {
        var hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.targetVelocity = wantedVelocity;

        hinge.motor = motor;
    }

    public void retractIntake()
    {
        var hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.targetVelocity = -wantedVelocity;

        hinge.motor = motor;
    }

    // Ball Pickup
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == coliderTag && numBalls < maxNumberBalls)
        {
            timer = Time.time;
        }
    }

    void OnCollisionStay(Collision collision)
    {
     
        if (collision.collider.tag == coliderTag && numBalls < maxNumberBalls && Time.time - timer >= timeOfBallContact)
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
        powerCellText.text = coliderTag + ": " + numBalls;
    }

    public int getNumberBalls()
    {
        return numBalls;
    }

    public void setVelocity(float x)
    {
        wantedVelocity = x;
    }
}
