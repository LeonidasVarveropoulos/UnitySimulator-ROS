using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FtcStraferManager : MonoBehaviour
{
    public Transform robot;

    [Header("Subscribers- DcMotorInput")]
    public RosSharp.RosBridgeClient.DcMotorInputSubscriber frontLeftWheelInput;
    public RosSharp.RosBridgeClient.DcMotorInputSubscriber frontRightWheelInput;

    public RosSharp.RosBridgeClient.DcMotorInputSubscriber backLeftWheelInput;
    public RosSharp.RosBridgeClient.DcMotorInputSubscriber backRightWheelInput;

    [Header("Publishers- DcMotorOutput")]
    public RosSharp.RosBridgeClient.DcMotorOutputPublisher frontLeftWheelOutput;
    public RosSharp.RosBridgeClient.DcMotorOutputPublisher frontRightWheelOutput;

    public RosSharp.RosBridgeClient.DcMotorOutputPublisher backLeftWheelOutput;
    public RosSharp.RosBridgeClient.DcMotorOutputPublisher backRightWheelOutput;

    private float frontLeftWheelCmd = 0f;
    private float frontRightWheelCmd = 0f;
    private float backLeftWheelCmd = 0f;
    private float backRightWheelCmd = 0f;

    private float frontLeftWheelEnc = 0f;
    private float frontRightWheelEnc = 0f;
    private float backLeftWheelEnc = 0f;
    private float backRightWheelEnc = 0f;

    private float previousRealTime;

    public float wheelSeparationWidth = 0.0f;
    public float wheelSeparationLength = 0.0f;
    public float wheelRadius = 0.0f;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float deltaTime = Time.realtimeSinceStartup - previousRealTime;

        frontLeftWheelCmd = frontLeftWheelInput.getCmd();
        frontRightWheelCmd = frontRightWheelInput.getCmd();
        backLeftWheelCmd = backLeftWheelInput.getCmd();
        backRightWheelCmd = backRightWheelInput.getCmd();

        frontLeftWheelOutput.updateEncoders(frontLeftWheelEnc);
        frontRightWheelOutput.updateEncoders(frontRightWheelEnc);
        backLeftWheelOutput.updateEncoders(backLeftWheelEnc);
        backRightWheelOutput.updateEncoders(backRightWheelEnc);

        var linearVelocityX = (frontLeftWheelCmd + frontRightWheelCmd + backLeftWheelCmd + backRightWheelCmd) * (wheelRadius / 4);

        var linearVelocityY = (-frontLeftWheelCmd + frontRightWheelCmd + backLeftWheelCmd - backRightWheelCmd) *(wheelRadius / 4);

        var angularVelocity = (-frontLeftWheelCmd + frontRightWheelCmd - backLeftWheelCmd + backRightWheelCmd) * (wheelRadius / (4 * (wheelSeparationWidth + wheelSeparationLength)));

        robot.Translate(new Vector3(linearVelocityX * deltaTime, linearVelocityY * deltaTime));

        var angVelZ = (angularVelocity / (Mathf.PI / 180)) * deltaTime;
        robot.Rotate(Vector3.left, angVelZ);

        previousRealTime = Time.realtimeSinceStartup;
    }
}