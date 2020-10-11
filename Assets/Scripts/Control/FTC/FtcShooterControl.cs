using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FtcShooterControl : MonoBehaviour
{
    [Header("Other Controls")]
    public GameObject intake;

    private IntakeControl intakeControl;

    [Header("Shooting Control")]
    public float timeBetweenShots = 1.0f;
    public float shotForceMult = 0.5f;
    public GameObject shootingAngle;

    private bool isShooting = false;

    public GameObject prefab;

    private float timer = 0.0f;

    private float wantedVelocity = 0.0f;

    void Awake()
    {
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
                var newPosition = transform.position;
                newPosition.x += 0.0f;
                newPosition.z += 0.0f;
                newPosition.y += 0.1f;

                GameObject instance = (GameObject)Instantiate(prefab, newPosition, transform.rotation);
                var rigid = instance.GetComponent<Rigidbody>();

                rigid.AddForce((shootingAngle.transform.rotation * Vector3.forward) * wantedVelocity * shotForceMult, ForceMode.Impulse);
                intakeControl.subtractBall();
            }
        }
    }

    public void setIdle()
    {
        isShooting = false;
    }

    public void setShoot()
    {
        isShooting = true;
    }

    public void setVelocity(float x)
    {
        wantedVelocity = x;
    }
}
