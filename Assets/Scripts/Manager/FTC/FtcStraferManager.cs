using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FtcStraferManager : MonoBehaviour
{
    public Transform robot;

    [Header("Subscribers-WheelMotors-DcMotorInput")]
    public RosSharp.RosBridgeClient.DcMotorInputSubscriber frontLeftWheelInput;
    public RosSharp.RosBridgeClient.DcMotorInputSubscriber frontRightWheelInput;

    public RosSharp.RosBridgeClient.DcMotorInputSubscriber backLeftWheelInput;
    public RosSharp.RosBridgeClient.DcMotorInputSubscriber backRightWheelInput;

    [Header("Publishers-WheelMotors-DcMotorOutput")]
    public RosSharp.RosBridgeClient.DcMotorOutputPublisher frontLeftWheelOutput;
    public RosSharp.RosBridgeClient.DcMotorOutputPublisher frontRightWheelOutput;

    public RosSharp.RosBridgeClient.DcMotorOutputPublisher backLeftWheelOutput;
    public RosSharp.RosBridgeClient.DcMotorOutputPublisher backRightWheelOutput;

    [Header("Subscribers-IntakeMotor-DcMotorInput")]
    public RosSharp.RosBridgeClient.DcMotorInputSubscriber intakeInput;

    [Header("Subscribers-FeederMotor-DcMotorInput")]
    public RosSharp.RosBridgeClient.DcMotorInputSubscriber feederInput;

    [Header("Subscribers-ShooterMotors-DcMotorInput")]
    public RosSharp.RosBridgeClient.DcMotorInputSubscriber leftShooterInput;
    public RosSharp.RosBridgeClient.DcMotorInputSubscriber rightShooterInput;

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

    [Header("Subsystem Controls")]
    public GameObject shooter;
    public GameObject intake;

    private FtcShooterControl shooterControl;
    private IntakeControl intakeControl;

    void Awake()
    {
        shooterControl = shooter.GetComponent<FtcShooterControl>();
        intakeControl = intake.GetComponent<IntakeControl>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float deltaTime = Time.realtimeSinceStartup - previousRealTime;
        
        // Drivetrain
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

        // Intake
        intakeControl.setVelocity(intakeInput.getCmd() * 150);
        if (intakeInput.getCmd() != 0)
            intakeControl.deployIntake();
        else
            intakeControl.retractIntake();

        // Feeder
        if (feederInput.getCmd() > 0)
            shooterControl.setShoot();
        else
            shooterControl.setIdle();

        // Shooter
        if (leftShooterInput.getCmd() > 0 && rightShooterInput.getCmd() > 0)
            shooterControl.setVelocity((leftShooterInput.getCmd() + rightShooterInput.getCmd()) / 2f);
        else
            shooterControl.setVelocity(0f);

        previousRealTime = Time.realtimeSinceStartup;
    }
}