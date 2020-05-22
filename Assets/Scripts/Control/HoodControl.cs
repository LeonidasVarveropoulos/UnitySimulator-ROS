using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoodControl : MonoBehaviour
{
    private double wantedAngle = -1;

    private double minAngleLimit;
    private double maxAngleLimit;

    [Header("Hood Angle Control (Degrees)")]
    public double angleTolerance = 0.5;
    public double rotatingAngleVelocity = 30;

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

            if (Mathf.Abs((float)(getAngle() - wantedAngle)) <= angleTolerance)
            {
                wantedAngle = -1;
            }

            else if (getAngle() < wantedAngle)
            {
                setVelocity(rotatingAngleVelocity);
            }

            else
            {
                setVelocity(-rotatingAngleVelocity);
            }
        }

        else
            setVelocity(0.0);
    }

    // Sets velocity of hood
    private void setVelocity(double vel)
    {
        var hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.targetVelocity = (float)vel;

        hinge.motor = motor;
    }

    // Moves the hood to idle position
    public void setIdle()
    {
        setAngle(minAngleLimit);
    }

    // Sets the angle
    public void setAngle(double angle)
    {
        wantedAngle = angle;
    }

    // Gets the angle from the game object
    public double getAngle()
    {
        return transform.rotation.eulerAngles.y;
    }

    // Sets the limits of the turret from the hinge joint
    private void getLimits()
    {
        var hinge = GetComponent<HingeJoint>();

        minAngleLimit = hinge.limits.min;
        maxAngleLimit = hinge.limits.max;
    }
}
