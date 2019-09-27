using UnityEngine;
using BomberCar.Utility.Attributes;

[System.Serializable]
public class CarTheme
{
    public int Mat;
    public int aFront;
    public int aMiddle;
    public int aBack;
    public int wheelF;
    public int wheelB;
}

public class Car : MonoBehaviour
{
    [Header("Car Set")]
    public GameObject[] Cars;

    [GreyOut]
    [SerializeField]
    private CarTheme cT;

    public int CarStyle = 0;

    [Header("Auto Principal?")]
    [GreyOut]
    public bool Main = false;

    protected virtual void Start()
    {
        if (Main)
        {
            CarStyle = PlayerPrefs.GetInt("CarSelect", 0);
            LoadCar();
        }
    }

    #region SET CAR

    //Set a specific theme
    public void CarSet(CarTheme Set)
    {
        Cars[CarStyle].SetActive(true);
        CarObjects co = Cars[CarStyle].GetComponent<CarObjects>();

        //switch material
        co.Chassis.GetComponent<Renderer>().material = co.ChassisMaterials[Set.Mat];

        //remove all addons
        DisableAllObjects();

        //turn on random Addon in the Front
        if (Set.aFront > -1)
            co.AddonFront[Set.aFront].GetComponent<Renderer>().enabled = true;
        //turn on random Addon in the Middle
        if (Set.aMiddle > -1)
            co.AddonMiddle[Set.aMiddle].GetComponent<Renderer>().enabled = true;
        //turn on random Addon in the Back
        if (Set.aBack > -1)
            co.AddonBack[Set.aBack].GetComponent<Renderer>().enabled = true;

        //Wheels Front/ Back
        if (Set.wheelF > -1)
            co.WheelFront[Set.wheelF].GetComponent<Renderer>().enabled = true;
        if (Set.wheelB > -1)
            co.WheelBack[Set.wheelB].GetComponent<Renderer>().enabled = true;

        if (Main)
            SaveCar(Set);
    }

    void SaveCar(CarTheme sv)
    {
        PlayerPrefs.SetInt("Mat" + CarStyle, sv.Mat);
        PlayerPrefs.SetInt("aFront" + CarStyle, sv.aFront);
        PlayerPrefs.SetInt("aMiddle" + CarStyle, sv.aMiddle);
        PlayerPrefs.SetInt("aBack" + CarStyle, sv.aBack);
        PlayerPrefs.SetInt("wheelF" + CarStyle, sv.wheelF);
        PlayerPrefs.SetInt("wheelB" + CarStyle, sv.wheelB);
    }

    void LoadCar()
    {
        cT.Mat = PlayerPrefs.GetInt("Mat" + CarStyle, 0);
        cT.aFront = PlayerPrefs.GetInt("aFront" + CarStyle, 0);
        cT.aMiddle = PlayerPrefs.GetInt("aMiddle" + CarStyle, 0);
        cT.aBack = PlayerPrefs.GetInt("aBack" + CarStyle, 0);
        cT.wheelF = PlayerPrefs.GetInt("wheelF" + CarStyle, 0);
        cT.wheelB = PlayerPrefs.GetInt("wheelB" + CarStyle, 0);

        CarSet(cT);
    }

    void DisableAllObjects()
    {
        CarObjects co = Cars[CarStyle].GetComponent<CarObjects>();

        foreach (GameObject gameObject in co.AddonFront)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in co.AddonMiddle)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in co.AddonBack)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in co.WheelFront)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject gameObject in co.WheelBack)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    //Randomize a Car theme
    public void RandomizeCar()
    {
        CarObjects co = Cars[CarStyle].GetComponent<CarObjects>();

        //Random int
        int chassis = Random.Range(0, co.ChassisMaterials.Length);
        int front = Random.Range(0, co.AddonFront.Length);
        int middle = Random.Range(0, co.AddonMiddle.Length);
        int back = Random.Range(0, co.AddonBack.Length);
        int wheelFront = Random.Range(0, co.WheelFront.Length);
        int wheelBack = Random.Range(0, co.WheelBack.Length);

        //switch material
        co.Chassis.GetComponent<Renderer>().material = co.ChassisMaterials[chassis];

        //remove all addons
        DisableAllObjects();

        //turn on random Addon in the Front
        if (co.AddonFront.Length > 0)
            co.AddonFront[front].GetComponent<Renderer>().enabled = true;

        //turn on random Addon in the Middle
        if (co.AddonMiddle.Length > 0)
            co.AddonMiddle[middle].GetComponent<Renderer>().enabled = true;

        //turn on random Addon in the Back
        if (co.AddonBack.Length > 0)
            co.AddonBack[back].GetComponent<Renderer>().enabled = true;

        //Wheels Front/ Back
        if (co.WheelFront.Length > 0)
            co.WheelFront[wheelFront].GetComponent<Renderer>().enabled = true;
        
        if (co.WheelBack.Length > 0)
            co.WheelBack[wheelBack].GetComponent<Renderer>().enabled = true;

        if (Main)
        {
            PlayerPrefs.SetInt("Mat" + CarStyle, chassis);
            PlayerPrefs.SetInt("aFront" + CarStyle, front);
            PlayerPrefs.SetInt("aMiddle" + CarStyle, middle);
            PlayerPrefs.SetInt("aBack" + CarStyle, back);
            PlayerPrefs.SetInt("wheelF" + CarStyle, wheelFront);
            PlayerPrefs.SetInt("wheelB" + CarStyle, wheelBack);
        }
    }

    public void ChangeCar()
    {
        Cars[CarStyle].SetActive(false);

        if (CarStyle == 0)        
            CarStyle = 1;               
        else CarStyle = 0;

        LoadCar();

        Cars[CarStyle].SetActive(true);

        PlayerPrefs.SetInt("CarSelect", CarStyle);
    }

    public void ChangeChassis(int Direccion)
    {
        CarObjects co = Cars[CarStyle].GetComponent<CarObjects>();

        cT.Mat += Direccion;

        if (cT.Mat == co.ChassisMaterials.Length)        
            cT.Mat = 0;      

        if (cT.Mat < 0)        
            cT.Mat = co.ChassisMaterials.Length - 1;


        co.Chassis.GetComponent<Renderer>().material = co.ChassisMaterials[cT.Mat];

        PlayerPrefs.SetInt("Mat" + CarStyle, cT.Mat);
    }

    public void ChangeWheelFront(int Direccion)
    {
        CarObjects co = Cars[CarStyle].GetComponent<CarObjects>();

        co.WheelFront[cT.wheelF].GetComponent<Renderer>().enabled = false;

        cT.wheelF += Direccion;

        if (cT.wheelF == co.WheelFront.Length)        
            cT.wheelF = 0;
        
        if (cT.wheelF < 0)        
            cT.wheelF = co.WheelFront.Length - 1;
        

        co.WheelFront[cT.wheelF].GetComponent<Renderer>().enabled = true;

        PlayerPrefs.SetInt("wheelF" + CarStyle, cT.wheelF);
    }

    public void ChangeWheelBack(int Direccion)
    {
        CarObjects co = Cars[CarStyle].GetComponent<CarObjects>();

        co.WheelBack[cT.wheelB].GetComponent<Renderer>().enabled = false;

        cT.wheelB += Direccion;

        if (cT.wheelB == co.WheelBack.Length)        
            cT.wheelB = 0;
        
        if (cT.wheelB < 0)
            cT.wheelB = co.WheelBack.Length - 1;
        
        co.WheelBack[cT.wheelB].GetComponent<Renderer>().enabled = true;

        PlayerPrefs.SetInt("wheelB" + CarStyle, cT.wheelB);
    }

    public void ChangeFront(int Direccion)
    {
        CarObjects co = Cars[CarStyle].GetComponent<CarObjects>();

        co.AddonFront[cT.aFront].GetComponent<Renderer>().enabled = false;
        cT.aFront += Direccion;

        if (cT.aFront == co.AddonFront.Length)
            cT.aFront = 0;
        
        if (cT.aFront < 0)
            cT.aFront = co.AddonFront.Length - 1;
        
        co.AddonFront[cT.aFront].GetComponent<Renderer>().enabled = true;

        PlayerPrefs.SetInt("aFront" + CarStyle, cT.aFront);
    }

    public void ChangeMiddle(int Direccion)
    {
        CarObjects co = Cars[CarStyle].GetComponent<CarObjects>();

        co.AddonMiddle[cT.aMiddle].GetComponent<Renderer>().enabled = false;
        cT.aMiddle += Direccion;

        if (cT.aMiddle == co.AddonMiddle.Length)        
            cT.aMiddle = 0;       

        if (cT.aMiddle < 0)
            cT.aMiddle = co.AddonMiddle.Length - 1;
        
        co.AddonMiddle[cT.aMiddle].GetComponent<Renderer>().enabled = true;

        PlayerPrefs.SetInt("aMiddle" + CarStyle, cT.aMiddle);
    }

    public void ChangeBack(int Direccion)
    {
        CarObjects co = Cars[CarStyle].GetComponent<CarObjects>();

        co.AddonBack[cT.aBack].GetComponent<Renderer>().enabled = false;
        cT.aBack += Direccion;

        if (cT.aBack == co.AddonBack.Length)        
            cT.aBack = 0;
        
        if (cT.aBack < 0)        
            cT.aBack = co.AddonBack.Length - 1;        

        co.AddonBack[cT.aBack].GetComponent<Renderer>().enabled = true;

        PlayerPrefs.SetInt("aBack" + CarStyle, cT.aBack);
    }

    #endregion
}
