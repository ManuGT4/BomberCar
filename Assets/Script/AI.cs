using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Car
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        CarStyle = Random.Range(0, 2);
        Cars[CarStyle].SetActive(true);
        RandomizeCar();
    }
}
