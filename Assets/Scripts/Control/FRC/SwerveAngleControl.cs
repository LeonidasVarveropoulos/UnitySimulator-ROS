using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveAngleControl : MonoBehaviour
{
    private float wantedAngle;
    public float maxRotatingAngleVelocity;
    public float minRotatingAngleVelocity;
    public float angleTolerance = 5;

    // Start is called before the first frame update
    void Start()
    {
        setAngle(0f);
    }

    // Turret Angle Control
    void Update()
    {
        Debug.Log(getData());
        
        float negAngleDiff = wrapAngle(getData() - wantedAngle);
        float posAngleDiff = wrapAngle(wantedAngle - getData());

        if (posAngleDiff <= angleTolerance | negAngleDiff <= angleTolerance)
        {
            if (posAngleDiff < negAngleDiff)
            {
                setVelocity(minRotatingAngleVelocity);
            }

            else
            {
                setVelocity(-minRotatingAngleVelocity);
            }
        }

        else if (posAngleDiff < negAngleDiff)
        {
            setVelocity(maxRotatingAngleVelocity);
        }

        else
        {
            setVelocity(-maxRotatingAngleVelocity);
        }
    }

    // Sets the angle
    public void setAngle(float angle)
    {
        if (angle < 0)
        {
            angle += 360;
        }
        wantedAngle = angle;
    }

    // Gets the angle from the game object
    private float getData()
    {
        return transform.localEulerAngles.y;
    }

    // Sets velocity of module
    private void setVelocity(float vel)
    {
        var hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.targetVelocity = vel;

        hinge.motor = motor;
    }

    private float wrapAngle(float angle)
    {
        if (angle < 0.0)
            return 360 + angle;
        else if (angle >= 360)
            return angle - 360;

        return angle;
    }
}
