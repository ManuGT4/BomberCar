using UnityEngine;
using System.Collections;
using Photon.Pun;

[System.Serializable]
public class CarMenuTheme
{
    public int Mat;
    public int aFront;
    public int aMiddle;
    public int aBack;
    public int wheelF;
    public int wheelB;
}

public class CarMenu : MonoBehaviourPun
{
    public ParticleSystem changeParticle;
    public ParticleSystem flashParticle;

    //Componentes
    private PhotonView pv;

    [Header("Car Set")]
    public GameObject car;

    public Material[] ChassisMaterials;

    public GameObject[] AddonFront;
    public GameObject[] AddonMiddle;
    public GameObject[] AddonBack;
    public GameObject[] WheelFront;
    public GameObject[] WheelBack;

    public CarTheme[] Theme;
    public CarTheme cT;

    [Header("Que auto es?")]
    public int WhatCar = 0;

    [Header("Auto Principal?")]
    public bool Principal;

    private object[] Data;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public void Start()
    {
        if (Principal)
        {
            switch (WhatCar)
            {
                case 0:
                    cT.Mat = PlayerPrefs.GetInt("Mat0", 0);
                    cT.aFront = PlayerPrefs.GetInt("aFront0", 0);
                    cT.aMiddle = PlayerPrefs.GetInt("aMiddle0", 0);
                    cT.aBack = PlayerPrefs.GetInt("aBack0", 0);
                    cT.wheelF = PlayerPrefs.GetInt("wheelF0", 0);
                    cT.wheelB = PlayerPrefs.GetInt("wheelB0", 0);
                    break;
                case 1:
                    cT.Mat = PlayerPrefs.GetInt("Mat1", 0);
                    cT.aFront = PlayerPrefs.GetInt("aFront1", 0);
                    cT.aMiddle = PlayerPrefs.GetInt("aMiddle1", 0);
                    cT.aBack = PlayerPrefs.GetInt("aBack1", 0);
                    cT.wheelF = PlayerPrefs.GetInt("wheelF1", 0);
                    cT.wheelB = PlayerPrefs.GetInt("wheelB1", 0);
                    break;
            }

            CarSet(cT);
        }
        else
        {
            Data = pv.InstantiationData;

            cT.Mat = (int)Data[0];
            cT.aFront = (int)Data[1];
            cT.aMiddle = (int)Data[2];
            cT.aBack = (int)Data[3];
            cT.wheelF = (int)Data[4];
            cT.wheelB = (int)Data[5];

            CarSet(cT);
        }

    }

    void SaveCar(CarTheme sv)
    {
        switch (WhatCar)
        {
            case 0:
                PlayerPrefs.SetInt("Mat0", sv.Mat);
                PlayerPrefs.SetInt("aFront0", sv.aFront);
                PlayerPrefs.SetInt("aMiddle0", sv.aMiddle);
                PlayerPrefs.SetInt("aBack0", sv.aBack);
                PlayerPrefs.SetInt("wheelF0", sv.wheelF);
                PlayerPrefs.SetInt("wheelB0", sv.wheelB);
                break;
            case 1:
                PlayerPrefs.SetInt("Mat1", sv.Mat);
                PlayerPrefs.SetInt("aFront1", sv.aFront);
                PlayerPrefs.SetInt("aMiddle1", sv.aMiddle);
                PlayerPrefs.SetInt("aBack1", sv.aBack);
                PlayerPrefs.SetInt("wheelF1", sv.wheelF);
                PlayerPrefs.SetInt("wheelB1", sv.wheelB);
                break;
        }
    }

    void DisableAllObjects()
    {
        foreach (GameObject gameObject in AddonFront)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in AddonMiddle)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in AddonBack)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in WheelFront)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in WheelBack)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    //Set a specific theme
    public void CarSet(CarTheme Set)
    {
        //switch material
        car.GetComponent<Renderer>().material = ChassisMaterials[Set.Mat];

        //remove all addons
        DisableAllObjects();

        //spawn particle
        changeParticle.Emit(30);
        flashParticle.Emit(2);

        //turn on random Addon in the Front
        if (Set.aFront > -1)
            AddonFront[Set.aFront].GetComponent<Renderer>().enabled = true;
        //turn on random Addon in the Middle
        if (Set.aMiddle > -1)
            AddonMiddle[Set.aMiddle].GetComponent<Renderer>().enabled = true;
        //turn on random Addon in the Back
        if (Set.aBack > -1)
            AddonBack[Set.aBack].GetComponent<Renderer>().enabled = true;

        //Wheels Front/ Back
        if (Set.wheelF > -1)      
            WheelFront[Set.wheelF].GetComponent<Renderer>().enabled = true;        
        if (Set.wheelB > -1)      
            WheelBack[Set.wheelB].GetComponent<Renderer>().enabled = true;
        
        if(Principal)
            SaveCar(Set);
    }

    //Randomize a Car theme
    public void RandomizeCar()
    {
        //Random int
        int chassis = Random.Range(0, ChassisMaterials.Length);
        int front = Random.Range(0, AddonFront.Length);
        int middle = Random.Range(0, AddonMiddle.Length);
        int back = Random.Range(0, AddonBack.Length);
        int wheelFront = Random.Range(0, WheelFront.Length);
        int wheelBack = Random.Range(0, WheelBack.Length);

        //switch material
        car.GetComponent<Renderer>().material = ChassisMaterials[chassis];

        //remove all addons
        DisableAllObjects();

        //spawn particle
        changeParticle.Emit(30);
        flashParticle.Emit(2);

        //turn on random Addon in the Front
        if (AddonFront.Length > 0)
            AddonFront[front].GetComponent<Renderer>().enabled = true;

        //turn on random Addon in the Middle
        if (AddonMiddle.Length > 0)
            AddonMiddle[middle].GetComponent<Renderer>().enabled = true;

        //turn on random Addon in the Back
        if (AddonBack.Length > 0)
            AddonBack[back].GetComponent<Renderer>().enabled = true;

        //Wheels Front/ Back
        if (WheelFront.Length > 0)       
            WheelFront[wheelFront].GetComponent<Renderer>().enabled = true;
        
        if (WheelBack.Length > 0)
            WheelBack[wheelBack].GetComponent<Renderer>().enabled = true;

        if (Principal)
        {
            switch (WhatCar)
            {
                case 0:
                    PlayerPrefs.SetInt("Mat0", chassis);
                    PlayerPrefs.SetInt("aFront0", front);
                    PlayerPrefs.SetInt("aMiddle0", middle);
                    PlayerPrefs.SetInt("aBack0", back);
                    PlayerPrefs.SetInt("wheelF0", wheelFront);
                    PlayerPrefs.SetInt("wheelB0", wheelBack);
                    break;
                case 1:
                    PlayerPrefs.SetInt("Mat1", chassis);
                    PlayerPrefs.SetInt("aFront1", front);
                    PlayerPrefs.SetInt("aMiddle1", middle);
                    PlayerPrefs.SetInt("aBack1", back);
                    PlayerPrefs.SetInt("wheelF1", wheelFront);
                    PlayerPrefs.SetInt("wheelB1", wheelBack);
                    break;
            }
        }
    }
}
