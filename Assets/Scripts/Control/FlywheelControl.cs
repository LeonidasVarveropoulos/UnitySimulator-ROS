using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlywheelControl : MonoBehaviour
{
    private float wantedVelocity = 0.0f;
    private float currentVelocity = 0.0f;

    private float power = 1f;

    [Header("Flywheel Control")]
    public float powerIncreament = 0.1f;

    [Header("Power UI")]
    public Slider slider;
    public float maxRpm = 2800f;

    void Start()
    {
        setIdle();
    }

    void Update()
    {
        if (wantedVelocity == currentVelocity)
            power = 1f;

        else if (wantedVelocity > currentVelocity)
        {
            currentVelocity += Mathf.Pow(power += powerIncreament, 2.0f);
            if (currentVelocity >= wantedVelocity)
                currentVelocity = wantedVelocity;
        }
        else
        {
            currentVelocity -= Mathf.Pow(power += powerIncreament, 2.0f);
            if (wantedVelocity >= currentVelocity)
                currentVelocity = wantedVelocity;
        }
        slider.value = currentVelocity / maxRpm;
    }

    public void setVelocity(float vel)
    {
        wantedVelocity = vel;
    }

    public float getVelocity()
    {
        return currentVelocity;
    }

    public void setIdle()
    {
        setVelocity(0.0f);
    }
}
