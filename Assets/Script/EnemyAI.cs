using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ControladorVision))]
public class EnemyAI : Car
{
    private NavMeshAgent navMeshAgent;
    private ControladorVision controlVision;

    public Transform Target;
    public Transform RaycastShoot;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        controlVision = GetComponent<ControladorVision>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();        
    }

	void Update ()
    {
		if(controlVision.SeePlayer)
        {
            if (Time.time > nextFire 
                && Physics.Raycast(Weapon.transform.position, transform.forward , out RaycastHit hit , controlVision.rangoVision) 
                && hit.collider.CompareTag("Player"))
            {
                nextFire = Time.time + fireRate;
                GameObject bomb = Instantiate(Bombs[0], Weapon.transform.position, Weapon.transform.rotation) as GameObject;
                Physics.IgnoreCollision(bomb.GetComponent<Collider>(), GetComponent<BoxCollider>());
                Bullet bombScript = bomb.GetComponent<Bullet>();
                bombScript.Tirador = GetComponent<BoxCollider>();
            }
        }
	}

    public void ActualizarDestinoNavMeshAgent(Vector3 puntoDestino)
    {
        if(puntoDestino != null)
        {      
            navMeshAgent.destination = puntoDestino;
            navMeshAgent.isStopped = false;
        }
    }

    public void ActualizarDestinoNavMeshAgent()
    {
        navMeshAgent.destination = Target.position;
    }

    public void DetenerNavMeshAgent()
    {
        navMeshAgent.isStopped = true;
    }

    public bool HemosLlegado()
    {
        return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending;
    }
}
