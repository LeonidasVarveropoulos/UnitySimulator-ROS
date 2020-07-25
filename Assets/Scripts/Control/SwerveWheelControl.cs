using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveWheelControl : MonoBehaviour
{
    public float velMulti = 5f;

    // Start is called before the first frame update
    void Start()
    {
        setVelocity(1f);
    }


    public void setVelocity(float vel)
    {
        var hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.targetVelocity = vel * velMulti;

        hinge.motor = motor;
    }
}
