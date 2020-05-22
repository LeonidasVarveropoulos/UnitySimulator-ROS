using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    private double wantedAngle = -1;

    private double minAngleLimit;
    private double maxAngleLimit;

    [Header("Turret Angle Control")]
    public double angleTolerance = 0.1;
    public double rotatingAngleVelocity = 1.0;

    // Turret Default Velocity
    void Start()
    {
        getLimits();
        setIdle();
    }

    // Turret Angle Control
    void Update()
    {
        if (wantedAngle != -1)
        {
            double negAngleDiff = wrapAngle(getAngle() - wantedAngle);
            double posAngleDiff = wrapAngle(wantedAngle - getAngle());

            if (posAngleDiff <= angleTolerance | negAngleDiff <= angleTolerance)
            {
                setIdle();
            }

            else if (posAngleDiff < negAngleDiff)
            {
                if (wrapAngle(minAngleLimit - getAngle()) <= posAngleDiff)
                {
                    if (wrapAngle(getAngle() - maxAngleLimit) <= negAngleDiff)
                        wantedAngle = -1;
                    else
                        setVelocity(-rotatingAngleVelocity);
                }
                    
                else
                    setVelocity(rotatingAngleVelocity);
            }

            else
            {
                if (wrapAngle(getAngle() - maxAngleLimit) <= negAngleDiff)
                {
                    if (wrapAngle(minAngleLimit - getAngle()) <= posAngleDiff)
                        wantedAngle = -1;
                    else
                        setVelocity(rotatingAngleVelocity);
                }
                    
                else
                    setVelocity(-rotatingAngleVelocity);
            }
        }
    }

    // Sets velocity of turret
    public void setVelocity(double vel)
    {
        var hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.targetVelocity = (float) -vel * (180/Mathf.PI);

        hinge.motor = motor;
    }

    // Stops the turret from moving
    public void setIdle()
    {
        setVelocity(0.0);
        wantedAngle = -1;
    }

    // Sets the angle
    public void setAngle(double angle)
    {
        wantedAngle = angle;
    }

    // Gets the angle from the game object
    public double getAngle()
    {
        return convertUnityAngle(transform.rotation.eulerAngles.y - 90);
    }

    // Sets the limits of the turret from the hinge joint
    private void getLimits()
    {
        var hinge = GetComponent<HingeJoint>();

        minAngleLimit = convertUnityAngle(hinge.limits.min - 90);
        maxAngleLimit = convertUnityAngle(hinge.limits.max - 90);
    }

    // Wraps the angle in radians
    private double wrapAngle(double angle)
    {
        if (angle < 0.0)
            return (Mathf.PI * 2) + angle;
        else if (angle >= (Mathf.PI * 2))
            return angle - (Mathf.PI * 2);

        return angle;
    }

    // Converts from the unity angle system in degrees to ROS in radians
    private double convertUnityAngle(double angle)
    {
        if (angle <= 0)
            return (-angle) * (Mathf.PI/180);
        else
            return (360 - angle) * (Mathf.PI / 180);
    }
}
