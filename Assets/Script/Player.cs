using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Car
{
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
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            float rotation = Input.GetAxis("Horizontal") * maxAngle;
        }
    }
}
