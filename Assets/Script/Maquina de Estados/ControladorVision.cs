using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorVision : MonoBehaviour {

    private EnemyAI controladorNavMesh;
    private SphereCollider ojos;

    [HideInInspector]
    public Transform Target;

    public float rangoVision = 13;
    public bool SeePlayer = false;

    private void Awake()
    {
        controladorNavMesh = GetComponent<EnemyAI>();
        ojos = GetComponent<SphereCollider>();
        ojos.radius = rangoVision;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SeePlayer = true;
            Target = other.transform;
            Debug.Log("i see you");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SeePlayer = false;
            Target = null;
            Debug.Log("i cant see other player");

        }
    }
}
