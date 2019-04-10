using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	public Vector3 Rotation;

    private bool move = false;

    private void Start()
    {
        Invoke("StartRotation", 2f);
    }

    void Update ()
    {
        if (!move) return;

        transform.Rotate(Rotation * Time.deltaTime, Space.World);
    }

    private void StartRotation()
    {
        move = true;
    }
}
