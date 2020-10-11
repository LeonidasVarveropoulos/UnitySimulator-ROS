using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [Header("Subscribers- Float")]
    public RosSharp.RosBridgeClient.FloatSubscriber turretVelocity;
    public RosSharp.RosBridgeClient.FloatSubscriber turretVerticalOffset;
    public RosSharp.RosBridgeClient.FloatSubscriber flywheelWantedRpm;
    public RosSharp.RosBridgeClient.FloatSubscriber hoodWantedAngle;
    public RosSharp.RosBridgeClient.FloatSubscriber turretWantedAngle;

    [Header("Subscribers- String")]
    public RosSharp.RosBridgeClient.StringSubscriber intakeState;
    public RosSharp.RosBridgeClient.StringSubscriber shooterState;
    public RosSharp.RosBridgeClient.StringSubscriber flywheelState;
    public RosSharp.RosBridgeClient.StringSubscriber turretState; 
    public RosSharp.RosBridgeClient.StringSubscriber hoodState;

    [Header("Subsystem Controls")]
    public GameObject turret;
    public GameObject hood;
    public GameObject flywheel;
    public GameObject intake;

    private ShooterControl shooterControl;
    private TurretControl turretControl;
    private HoodControl hoodControl;
    private FlywheelControl flywheelControl;
    private IntakeControl intakeControl;

    void Awake()
    {
        shooterControl = turret.GetComponent<ShooterControl>();
        turretControl = turret.GetComponent<TurretControl>();
        hoodControl = hood.GetComponent<HoodControl>();
        flywheelControl = flywheel.GetComponent<FlywheelControl>();
        intakeControl = intake.GetComponent<IntakeControl>();
    }

    void FixedUpdate()
    {
        // Intake
        if (intakeState.getData() == "deploy")
            intakeControl.deployIntake();
        else if (intakeState.getData() == "retract")
            intakeControl.retractIntake();

        if (shooterState.getData() == "idle")
        {
            // Turret
            if (turretState.getData() == "rotate_turret")
                turretControl.setAngle(turretWantedAngle.getData());
            else
                turretControl.setIdle();

            // Flywheel
            if (flywheelState.getData() == "spin_up")
                flywheelControl.setVelocity(flywheelWantedRpm.getData());
            else
                flywheelControl.setIdle();

            // Hood
            if (hoodState.getData() == "rotate_hood")
                hoodControl.setAngle(hoodWantedAngle.getData());
            else
                hoodControl.setIdle();

            shooterControl.setIdle();

        }
        else if (shooterState.getData() == "prime")
        {
            float hoodAngle;
            float flywheelRpm;
            if (turretVerticalOffset.getData() < 14f)
            {
                hoodAngle = (Mathf.Pow(turretVerticalOffset.getData(), 2.0f) * -0.177536f) + (turretVerticalOffset.getData() * 4.88588f) + 30.3647f;
                flywheelRpm = 4750f;
            }
            else
            {
                hoodAngle = (Mathf.Pow(turretVerticalOffset.getData(), 4.0f) * -0.012616f) + (Mathf.Pow(turretVerticalOffset.getData(), 3.0f) * 0.904704f) + (Mathf.Pow(turretVerticalOffset.getData(), 2.0f) * -24.1019f) + (turretVerticalOffset.getData() * 283.051f) + -1173.54f;
                flywheelRpm = 4000f;
            }
      
            shooterControl.setPrime(turretVelocity.getData(), hoodAngle, flywheelRpm);
            Debug.Log(hoodAngle + " " + flywheelRpm);
        }
        else if (shooterState.getData() == "shoot")
        {
            float hoodAngle;
            float flywheelRpm;
            if (turretVerticalOffset.getData() < 14f)
            {
                hoodAngle = (Mathf.Pow(turretVerticalOffset.getData(), 2.0f) * -0.177536f) + (turretVerticalOffset.getData() * 4.88588f) + 30.3647f;
                flywheelRpm = 4750f;
            }
            else
            {
                hoodAngle = (Mathf.Pow(turretVerticalOffset.getData(), 4.0f) * -0.012616f) + (Mathf.Pow(turretVerticalOffset.getData(), 3.0f) * 0.904704f) + (Mathf.Pow(turretVerticalOffset.getData(), 2.0f) * -24.1019f) + (turretVerticalOffset.getData() * 283.051f) + -1173.54f;
                flywheelRpm = 4000f;
            }
            shooterControl.setPrime(turretVelocity.getData(), hoodAngle, flywheelRpm);
            shooterControl.setShoot();
        }
    }
}