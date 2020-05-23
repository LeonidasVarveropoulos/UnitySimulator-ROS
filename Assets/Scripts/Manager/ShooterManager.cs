using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterManager : MonoBehaviour
{
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
}
