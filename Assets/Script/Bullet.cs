using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private Rigidbody rb;
    private bool FirstCollision = false;

    [HideInInspector]
    public BoxCollider Tirador;

    public int Force = 10;
    public int NumRebote = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }

    private void Start()
    {
        rb.AddForce(transform.forward * Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!FirstCollision)
        {
            Physics.IgnoreCollision(Tirador, GetComponent<SphereCollider>(), false);
            FirstCollision = true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
