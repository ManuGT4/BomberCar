using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EstadoPatrulla : MonoBehaviour {

    public Transform[] WayPoints;

    private MaquinaDeEstados maquinaDeEstados;
    private EnemyAI controladorNavMesh;
    private ControladorVision controladorVision;

    [SerializeField]
    private float radioPatrulla = 5;
        
    private Vector3 originPosition; //Punto de origen 

    private void Awake()
    {
        controladorNavMesh = GetComponent<EnemyAI>();
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorVision = GetComponent<ControladorVision>();

        originPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        
        //Ve al jugador?
        //RaycastHit hit;
        if(controladorVision.SeePlayer)
        {
            controladorNavMesh.Target = controladorVision.Target;
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoPersecusion);
            return;
        }

        if (controladorNavMesh.HemosLlegado())
        {
            ActualizarWayPointDestino();
        }
	}

    void OnEnable()
    {
        //ActualizarWayPointDestino();
    }

    void ActualizarWayPointDestino()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radioPatrulla;
        randomDirection += originPosition;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit _hit, radioPatrulla + 1 , NavMesh.AllAreas);
        Vector3 finalPosition = _hit.position;
        controladorNavMesh.ActualizarDestinoNavMeshAgent(finalPosition);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && enabled)
        {
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoPersecusion);
        }
    }
}
