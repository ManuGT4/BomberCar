using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour{


    public GameObject Player;
    public int Lerp;

    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        offset = new Vector3(Player.transform.position.x, Player.transform.position.y + 13.5f, Player.transform.position.z - 10f);
        transform.position = offset;
    }


    void Update ()
    {
        offset = new Vector3(Player.transform.position.x, Player.transform.position.y + 13.5f, Player.transform.position.z - 10);
        transform.position = Vector3.Slerp(transform.position, offset, Lerp);
    }
}
