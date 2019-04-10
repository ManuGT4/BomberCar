using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtBar : MonoBehaviour {

    public Image Bar;

    public float HealtValue = 0;
    private float healtMax;

    private Quaternion rotation;
	

    void Start()
    {
        healtMax = HealtValue;
        rotation = transform.rotation;
    }


	void Update () {
        HealtChange(HealtValue);		
	}

    void HealtChange(float _healtValue)
    {
        float amount = (_healtValue / healtMax) * 360 / 360f;
        Bar.fillAmount = amount;
    }

    private void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
