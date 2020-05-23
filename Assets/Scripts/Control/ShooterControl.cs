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
        Debug.Log(Time.time);
        if (Time.time - timer >= timeBetweenShots)
        {
            timer = Time.time;
            var x = 0.1f * Mathf.Cos((turretControl.getAngle() * Mathf.Rad2Deg + 90) * Mathf.Deg2Rad);
            var z = 0.1f * Mathf.Sin((turretControl.getAngle() * Mathf.Rad2Deg + 90) * Mathf.Deg2Rad);
            var newPosition = transform.position;
            newPosition.x += x;
            newPosition.z += z;
            newPosition.y += 0.4f;

            GameObject instance = (GameObject)Instantiate(prefab, newPosition, transform.rotation);
            var rigid = instance.GetComponent<Rigidbody>();

            var xForce = (flywheelControl.getVelocity() * shotForceMult) * Mathf.Cos((turretControl.getAngle() * Mathf.Rad2Deg + 90) * Mathf.Deg2Rad);
            var zForce = (flywheelControl.getVelocity() * shotForceMult) * Mathf.Sin((turretControl.getAngle() * Mathf.Rad2Deg + 90) * Mathf.Deg2Rad);
            var yForce = hoodControl.getAngle();
            rigid.AddForce(xForce, yForce, zForce, ForceMode.Impulse);
        }
      
        if (isShooting)
        {
    
        }
    }

    public void setIdle()
    {
        turretControl.setIdle();
        hoodControl.setIdle();
        flywheelControl.setIdle();
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
