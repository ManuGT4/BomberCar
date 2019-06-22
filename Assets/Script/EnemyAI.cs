using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ControladorVision))]
public class EnemyAI : Car
{
    private NavMeshAgent navMeshAgent;
    private ControladorVision controlVision;

    public Transform Target;
    public Transform RaycastShoot;

    public Transform[] WayPoints;
    [SerializeField]
    private float radioPatrulla = 5;

    private Vector3 originPosition; //Punto de origen 

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        controlVision = GetComponent<ControladorVision>();
        rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        navMeshAgent.Warp(transform.position);
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        ActualizarDestinoNavMeshAgent(Target.position);

    }

    void Update ()
    {
		if(controlVision.SeePlayer)
        {
            if (Time.time > nextFire && Physics.Raycast(Weapon.transform.position, transform.forward , out RaycastHit hit , controlVision.rangoVision) && hit.collider.CompareTag("Player"))           
                Shoot();            
        }

        if (controlVision.SeePlayer)
        {
            Target = controlVision.Target;
            return;
        }

        if (HemosLlegado())
        {
            ActualizarWayPointDestino();
        }
    }

    void ActualizarWayPointDestino()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radioPatrulla;
        randomDirection += originPosition;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit _hit, radioPatrulla + 1, NavMesh.AllAreas);
        Vector3 finalPosition = _hit.position;
        ActualizarDestinoNavMeshAgent(finalPosition);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && enabled)
        {
            //Apuntarle
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
