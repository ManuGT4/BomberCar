using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Car
{
    private string[] players = new string[4] { "_J1", "_J2", "_J3", "_J4" };
    private string actualPlayer;
    public int PlayerSelect = 0;

    private float inputAcceleration = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        actualPlayer = players[PlayerSelect];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("ShootFront" + actualPlayer) && Time.time > nextFire)
            Shoot();

        if (Input.GetButtonDown("HandBrake" + actualPlayer))
            actualBrakeForce = 50f;
        if (Input.GetButtonUp("HandBrake" + actualPlayer))
            actualBrakeForce = 0f;

        inputAcceleration = Input.GetAxis("Accelerate" + actualPlayer);
        motorForce = inputAcceleration > 0 ? inputAcceleration * MaxMotorForce : inputAcceleration * MaxReverseForce;

        rotation = Input.GetAxis("Horizontal" + actualPlayer) * maxAngle;
    }
}
