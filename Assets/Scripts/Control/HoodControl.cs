using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoodControl : MonoBehaviour
{
    private float wantedAngle = -1f;

    private float minAngleLimit;
    private float maxAngleLimit;

    [Header("Hood Angle Control (Degrees)")]
    public float angleTolerance = 0.5f;
    public float rotatingAngleVelocity = 30f;
    public float idleAngle = 28f;

    // Turret Default Velocity
    void Start()
    {
        getLimits();
        setIdle();
    }

    // Hood Angle Control
    void Update()
    {
        if (wantedAngle != -1)
        {
            Debug.Log(getAngle() + " " + wantedAngle);

            if (Mathf.Abs((float)(getAngle() - wantedAngle)) <= angleTolerance)
            {
                wantedAngle = -1f;
                if (getAngle() < wantedAngle)
                {
                    setVelocity(-1f);
                }

                else
                {
                    setVelocity(1f);
                }

            }

            else if (getAngle() < wantedAngle)
            {
                setVelocity(-rotatingAngleVelocity);
            }

            else
            {
                setVelocity(rotatingAngleVelocity);
            }
        }

        else
            setVelocity(0.0f);
    }

    // Sets velocity of hood
    private void setVelocity(float vel)
    {
        var hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.targetVelocity = (float)vel;

        hinge.motor = motor;
    }

    // Moves the hood to idle position
    public void setIdle()
    {
        setAngle(idleAngle);
    }

    // Sets the angle
    public void setAngle(float angle)
    {
        wantedAngle = angle;
    }

    // Gets the angle from the game object
    public float getAngle()
    {
        return transform.rotation.eulerAngles.x;
    }

    // Sets the limits of the turret from the hinge joint
    private void getLimits()
    {
        var hinge = GetComponent<HingeJoint>();

        minAngleLimit = hinge.limits.min;
        maxAngleLimit = hinge.limits.max;
    }
}
