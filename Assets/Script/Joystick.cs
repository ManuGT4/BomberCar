using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler , IEndDragHandler {

    public Canvas padre;
    Vector3 posInicial;
    public float radio;
    private Vector2 Axis;

    //Devuelve los axis
    public Vector2 axis
    {
        get
        {
            return Axis;
        }
    }

    //Devuelve movimiento horizontal
    public float Horizontal
    {
        get
        {
            if (Axis.x > 0.1f || Axis.x < -0.1f)
                return Axis.x;
            else
                return Axis.x = 0;
        }
    }

    //Devuelve movimiento vertical
    public float Vertical
    {
        get
        {
            if (Axis.y > 0.1f || Axis.y < -0.1f)
                return Axis.y;
            else
                return Axis.y = 0;
        }
    }

    //Devuelve si el stick se esta moviendo
    public bool isMoving
    {
        get
        {
            return Axis != Vector2.zero;
        }
    }

    public void Start()
    {
        //Posicion inicial del stick
        posInicial = transform.position;
    }

    //Se ejecuta al tomar el stick
    public void OnDrag(PointerEventData point)
    {		
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(padre.transform as RectTransform, point.position, padre.worldCamera, out pos);
        //Esto acomoda el punto para que este en el lugar que queremos
        Vector2 newPos = padre.transform.TransformPoint(pos) - posInicial;
        //El radio en el que se puede mover el stick
        newPos.x = Mathf.Clamp(newPos.x, -radio, radio);
        newPos.y = Mathf.Clamp(newPos.y, -radio, radio);
        Axis = newPos / radio;
  
        transform.localPosition = newPos;
    }

    //Al soltar el stick vuelve a la pos inicial
    public void OnEndDrag(PointerEventData point)
    {
        transform.position = posInicial;
        Axis = Vector2.zero;
    }
}
