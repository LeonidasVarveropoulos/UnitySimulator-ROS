using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    private float wantedAngle = -1f;

    private float minAngleLimit;
    private float maxAngleLimit;

    [Header("Turret Angle Control")]
    public float angleTolerance = 0.1f;
    public float rotatingAngleVelocity = 1.0f;

    // Turret Default Velocity
    void Start()
    {
        getLimits();
        setIdle();
    }

    // Turret Angle Control
    void Update()
    {
        Debug.Log(getData());
        if (wantedAngle != -1)
        {
            float negAngleDiff = wrapAngle(getData() - wantedAngle);
            float posAngleDiff = wrapAngle(wantedAngle - getData());

            if (posAngleDiff <= angleTolerance | negAngleDiff <= angleTolerance)
            {
                setIdle();
            }

            else if (posAngleDiff < negAngleDiff)
            {
                if (wrapAngle(minAngleLimit - getData()) <= posAngleDiff)
                {
                    if (wrapAngle(getData() - maxAngleLimit) <= negAngleDiff)
                        wantedAngle = -1f;
                    else
                        setVelocity(-rotatingAngleVelocity);
                }
                    
                else
                    setVelocity(rotatingAngleVelocity);
            }

            else
            {
                if (wrapAngle(getData() - maxAngleLimit) <= negAngleDiff)
                {
                    if (wrapAngle(minAngleLimit - getData()) <= posAngleDiff)
                        wantedAngle = -1f;
                    else
                        setVelocity(rotatingAngleVelocity);
                }
                    
                else
                    setVelocity(-rotatingAngleVelocity);
            }
        }
    }

    // Sets velocity of turret
    public void setVelocity(float vel)
    {
        var hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.targetVelocity = (float) -vel * (180/Mathf.PI);

        hinge.motor = motor;
    }

    // Stops the turret from moving
    public void setIdle()
    {
        setVelocity(0.0f);
        wantedAngle = -1f;
    }

    // Sets the angle
    public void setAngle(float angle)
    {
        wantedAngle = angle;
    }

    // Gets the angle from the game object
    public float getData()
    {
        return convertUnityAngle(transform.localEulerAngles.y);
    }

    // Sets the limits of the turret from the hinge joint
    private void getLimits()
    {
        var hinge = GetComponent<HingeJoint>();

        minAngleLimit = convertUnityAngle(hinge.limits.min);
        maxAngleLimit = convertUnityAngle(hinge.limits.max);
    }

    // Wraps the angle in radians
    private float wrapAngle(float angle)
    {
        if (angle < 0.0)
            return (Mathf.PI * 2) + angle;
        else if (angle >= (Mathf.PI * 2))
            return angle - (Mathf.PI * 2);

        return angle;
    }

    // Converts from the unity angle system in degrees to ROS in radians
    private float convertUnityAngle(float angle)
    {
        if (angle <= 0)
            return (-angle) * (Mathf.PI/180);
        else
            return (360 - angle) * (Mathf.PI / 180);
    }
}
