using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [Header("Simple Subscriber and Publisher")]
    public SimpleSubscriberPublisher node;

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
            if (turretState.getData() == "idle")
                turretControl.setIdle();
            else if (turretState.getData() == "rotate_turret")
                turretControl.setAngle(turretWantedAngle.getData());

            // Flywheel
            if (flywheelState.getData() == "idle")
                flywheelControl.setIdle();
            else if (flywheelState.getData() == "spin_up")
                flywheelControl.setVelocity(flywheelWantedRpm.getData());

            // Hood
            if (hoodState.getData() == "idle")
                hoodControl.setIdle();
            else if (hoodState.getData() == "rotate_hood")
                hoodControl.setAngle(hoodWantedAngle.getData());

        }
        else if (shooterState.getData() == "prime")
        {
            var hoodAngle = (((Mathf.Exp(-0.106665f * turretVerticalOffset.getData()) * 40.5628f) + 1.0f) / 89.8105f) + 43.3079f;
            var flywheelRpm = (((Mathf.Exp(-0.092559f * turretVerticalOffset.getData()) * 24.7655f) + 1.0f) / 7795.87f) + 1810.29f;
            shooterControl.setPrime(turretVelocity.getData(), hoodAngle, flywheelRpm);
        }
        else if (shooterState.getData() == "shoot")
        {
            var hoodAngle = (((Mathf.Exp(-0.106665f * turretVerticalOffset.getData()) * 40.5628f) + 1.0f) / 89.8105f) + 43.3079f;
            var flywheelRpm = (((Mathf.Exp(-0.092559f * turretVerticalOffset.getData()) * 24.7655f) + 1.0f) / 7795.87f) + 1810.29f;
            shooterControl.setPrime(turretVelocity.getData(), hoodAngle, flywheelRpm);
            shooterControl.setShoot();
        }
    }
}