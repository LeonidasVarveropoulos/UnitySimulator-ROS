using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterControl : MonoBehaviour
{
    [Header("Other Controls")]
    public GameObject turret;
    public GameObject hood;
    public GameObject flywheel;
    public GameObject intake;

    private TurretControl turretControl;
    private HoodControl hoodControl;
    private FlywheelControl flywheelControl;
    private IntakeControl intakeControl;

    [Header("Shooting Control")]
    public float timeBetweenShots = 1.0f;
    public float shotForceMult = 0.5f;
    public GameObject shootingAngle;

    private bool isShooting = false;

    public GameObject prefab;

    private float timer = 0.0f;

    void Awake()
    {
        turretControl = turret.GetComponent<TurretControl>();
        hoodControl = hood.GetComponent<HoodControl>();
        flywheelControl = flywheel.GetComponent<FlywheelControl>();
        intakeControl = intake.GetComponent<IntakeControl>();

        timer = Time.time;
    }

    void FixedUpdate()
    {
        if (isShooting)
        {
            if (Time.time - timer >= timeBetweenShots & intakeControl.getNumberBalls() > 0)
            {
                timer = Time.time;
                var x = 0.0f * Mathf.Cos(turretControl.getData());
                var z = 0.0f * Mathf.Sin(turretControl.getData());
                var newPosition = transform.position;
                newPosition.x += x;
                newPosition.z += z;
                newPosition.y += 0.3f;

                GameObject instance = (GameObject)Instantiate(prefab, newPosition, transform.rotation);
                var rigid = instance.GetComponent<Rigidbody>();

                rigid.AddForce((shootingAngle.transform.rotation * Vector3.forward) * flywheelControl.getData() * shotForceMult, ForceMode.Impulse);
                intakeControl.subtractBall();
            }
        }
    }

    public void setIdle()
    {
        isShooting = false;
    }

    public void setPrime(float turretVel, float hoodAngle, float flywheelRpm)
    {
        turretControl.setVelocity(turretVel);
        hoodControl.setAngle(hoodAngle);
        flywheelControl.setVelocity(flywheelRpm);
    }

    public void setShoot()
    {
        isShooting = true;
    }
}
