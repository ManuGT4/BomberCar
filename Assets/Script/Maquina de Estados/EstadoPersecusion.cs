using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoPersecusion : MonoBehaviour {

    private MaquinaDeEstados maquinaDeEstados;
    private EnemyAI controladorNavMesh;
    private ControladorVision controladorVision;

    private void Awake()
    {
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorNavMesh = GetComponent<EnemyAI>();
        controladorVision = GetComponent<ControladorVision>();
    }


    // Update is called once per frame
    void Update () {

        //Ve al jugador?
        //RaycastHit hit;
        if(!controladorVision.SeePlayer)
        {
            
        }

        controladorNavMesh.ActualizarDestinoNavMeshAgent();	
	}
}
