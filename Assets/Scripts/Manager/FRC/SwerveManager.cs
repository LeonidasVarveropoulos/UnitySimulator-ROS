using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveManager : MonoBehaviour
{
    [Header("Subscribers- Float")]
    public RosSharp.RosBridgeClient.FloatSubscriber wantedFrontRightAngle;
    public RosSharp.RosBridgeClient.FloatSubscriber wantedFrontRightWheel;

    public RosSharp.RosBridgeClient.FloatSubscriber wantedFrontLeftAngle;
    public RosSharp.RosBridgeClient.FloatSubscriber wantedFrontLeftWheel;

    public RosSharp.RosBridgeClient.FloatSubscriber wantedBackRightAngle;
    public RosSharp.RosBridgeClient.FloatSubscriber wantedBackRightWheel;

    public RosSharp.RosBridgeClient.FloatSubscriber wantedBackLeftAngle;
    public RosSharp.RosBridgeClient.FloatSubscriber wantedBackLeftWheel;

    [Header("Subsystem Controls")]
    public GameObject frontRightModule;
    private SwerveAngleControl frontRightAngleControl;
    public GameObject frontRightWheel;
    private SwerveWheelControl frontRightWheelControl;

    public GameObject frontLeftModule;
    private SwerveAngleControl frontLeftAngleControl;
    public GameObject frontLeftWheel;
    private SwerveWheelControl frontLeftWheelControl;

    public GameObject backRightModule;
    private SwerveAngleControl backRightAngleControl;
    public GameObject backRightWheel;
    private SwerveWheelControl backRightWheelControl;

    public GameObject backLeftModule;
    private SwerveAngleControl backLeftAngleControl;
    public GameObject backLeftWheel;
    private SwerveWheelControl backLeftWheelControl;

    void Awake()
    {
        frontRightAngleControl = frontRightModule.GetComponent<SwerveAngleControl>();
        frontRightWheelControl = frontRightWheel.GetComponent<SwerveWheelControl>();

        frontLeftAngleControl = frontLeftModule.GetComponent<SwerveAngleControl>();
        frontLeftWheelControl = frontLeftWheel.GetComponent<SwerveWheelControl>();

        backRightAngleControl = backRightModule.GetComponent<SwerveAngleControl>();
        backRightWheelControl = backRightWheel.GetComponent<SwerveWheelControl>();

        backLeftAngleControl = backLeftModule.GetComponent<SwerveAngleControl>();
        backLeftWheelControl = backLeftWheel.GetComponent<SwerveWheelControl>();
    }

    void FixedUpdate()
    {
        frontRightAngleControl.setAngle(wantedFrontRightAngle.getData());
        frontRightWheelControl.setVelocity(wantedFrontRightWheel.getData());

        frontLeftAngleControl.setAngle(wantedFrontLeftAngle.getData());
        frontLeftWheelControl.setVelocity(wantedFrontLeftWheel.getData());

        backRightAngleControl.setAngle(wantedBackRightAngle.getData());
        backRightWheelControl.setVelocity(wantedBackRightWheel.getData());

        backLeftAngleControl.setAngle(wantedBackLeftAngle.getData());
        backLeftWheelControl.setVelocity(wantedBackLeftWheel.getData());
    }
}
