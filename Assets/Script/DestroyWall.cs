using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    [SerializeField]
    private float WallResistence = 1500;

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 ImpactForce = collision.impulse / Time.fixedDeltaTime;
        Debug.Log(ImpactForce.magnitude / 1000);
        WallResistence -= ImpactForce.magnitude / 1000;

        if (WallResistence < 0)
        {
            Destroy(gameObject);
        }
    }
}
