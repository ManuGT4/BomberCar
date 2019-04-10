using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaquinaDeEstados : MonoBehaviour {

    public MonoBehaviour EstadoPatrulla;
    public MonoBehaviour EstadoPersecusion;
    public MonoBehaviour EstadoInicial;

    private MonoBehaviour EstadoActual;


	void Start () {
        ActivarEstado(EstadoInicial);
	}

    public void ActivarEstado(MonoBehaviour nuevoEstado)
    {
        if(EstadoActual != null)  EstadoActual.enabled = false;
        EstadoActual = nuevoEstado;
        EstadoActual.enabled = true;
    }
}
