using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    LineRenderer laser;

    float nextFire;
    float fireRate = 3;

    float rotateSpeed = 5;

    public Joystick r3;

    public GameObject bullet;

    public LayerMask ColisionRaycast;
    RaycastHit2D hit;

	void Start () {
        laser = GetComponent<LineRenderer>();		
	}
	
	void Update () {
        if(r3.Horizontal != 0 || r3.Vertical != 0)
        {
            var atan2 = Mathf.Atan2(r3.Horizontal, -r3.Vertical) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, (atan2 - 90)), rotateSpeed * Time.deltaTime);
        }

        laser.SetPosition(0, Vector3.right * 10);

        if(hit = Physics2D.Raycast(transform.position, Vector2.right * 10, 30f, ColisionRaycast))
        {
            if(hit.collider.CompareTag("Enemigo") && Time.time > nextFire)
            {
                Debug.Log("Shoot");
                nextFire = Time.time + fireRate;
            }

        }
    }
}
