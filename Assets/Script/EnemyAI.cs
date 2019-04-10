using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    }

    void Start ()
    {
	}
	
	void Update ()
    {
		if(controlVision.SeePlayer)
        {
            RaycastHit hit;
            if (Time.time > nextFire 
                && Physics.Raycast(RaycastShoot.position, transform.forward , out hit , controlVision.rangoVision) 
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
