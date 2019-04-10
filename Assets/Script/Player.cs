using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : Car
{

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {

            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                GameObject bomb = Instantiate(Bombs[0], Weapon.transform.position, Weapon.transform.rotation) as GameObject;
                Physics.IgnoreCollision(bomb.GetComponent<Collider>(), GetComponent<Collider>());
                Bullet bombScript = bomb.GetComponent<Bullet>();
                bombScript.Tirador = GetComponent<Collider>();
            }

            if (Input.GetButtonDown("Fire2"))
                acelera = !acelera;
            if (Input.GetButtonUp("Fire2"))
                acelera = !acelera;

            if (Input.GetButtonDown("Fire3"))
                reversa = !reversa;
            if (Input.GetButtonUp("Fire3"))
                reversa = !reversa;

            /*
            if (Input.GetButtonDown("Fire4"))
            {
                freno = !freno;
            }*/

            float rotation = Input.GetAxis("Horizontal") * maxAngle;

            FrontLeft.steerAngle = rotation;
            FrontRight.steerAngle = rotation;
        }
    }
}
