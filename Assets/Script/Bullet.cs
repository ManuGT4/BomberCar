using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private Rigidbody rb;

    [HideInInspector]
    public Collider Tirador;

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
        Physics.IgnoreCollision(Tirador, GetComponent<Collider>(), false);

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
