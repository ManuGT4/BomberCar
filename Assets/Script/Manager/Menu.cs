using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Menu : MonoBehaviourPun
{
    [Header("Version del Juego")]
    public string gameVersion = "0.1f";

    [Header("Canvas Group")]
    public CanvasGroup MenuPrincipal;
    public CanvasGroup SelectMode;
    public CanvasGroup OptionMenu;
    public CanvasGroup SelectCar;
    public CanvasGroup Theme;
    public CanvasGroup Custom;
    public CanvasGroup Randomize;
    public CanvasGroup SinglePlayerConfig;
    public CanvasGroup MultiPlayerConfig;

    [Header("Camera Config")]
    public  Transform Camera;
    public  Vector3   CamPosition;
    public  Vector3   CustomPosition;
    private Vector3   velocity = Vector3.zero;
    private float     duration = 0.8f;

    [Header("Mapas")]
    public string[] Mapas;
    private int mapSelect = 0;
    public Text MapaName;

    [Header("Player Set")]
    public GameObject[] Car;
    public Quaternion FirstRotation;

    [Header("Pilares")]
    public GameObject PilarPrincipal;
    public GameObject[] Pilares;
    public Vector3 PilarInicial;
    public Vector3[]  PosicionPilares;
    public Vector3[] PosicionPilaresHide;
    Hashtable playerStats = new Hashtable();
    private int style = 0;
    private int nextTheme;

    [Header("Bots Config")]
    public int BotCounts = 0;
    private GameObject[] bots = new GameObject[3];

    //Componentes
    private GameManager gameManager;

    public void Awake()
    {
        style = PlayerPrefs.GetInt("CarSelect", 0);
        Car[style].SetActive(true);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.styleSelect = style;

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void PlayButton()
    {
        ChangeGroup(MenuPrincipal, SelectMode);
    }

    public void OptionButton()
    {
        
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void GameMode(int mode)
    {
        gameManager.styleSelect = style;
        SetPlayerProperties();

        switch (mode)
        {
            case 0:
                gameManager.PartidaOffline(mapSelect + 1);
                break;
            case 1:
                gameManager.StartPartida();
                break;
        }
    }

    public void SinglePlayer()
    {
        StartCoroutine(PilarPosition(0));
        MapaName.text = Mapas[0];
        PhotonNetwork.OfflineMode = true;
    }

    public void MultiPlayer()
    {
        ChangeGroup(SelectMode, MultiPlayerConfig);
    }

    public void SelectCarButton()
    {
        StartCoroutine(CameraPosition(0));
    }

    public void ThemeButton()
    {
        ChangeGroup(SelectCar, Theme);
    }

    public void CustomButton()
    {
        ChangeGroup(SelectCar, Custom);
    }

    public void RandomizeButton()
    {
        ChangeGroup(SelectCar, Randomize);
    }

    public void RandomButton()
    {
        CarMenu carRandom = Car[style].GetComponent<CarMenu>();

        carRandom.RandomizeCar();
    }

    public void ChangeCar()
    {
        Car[style].SetActive(false);

        if (style == 0) style = 1;
        else style = 0;

        Car[style].SetActive(true);

        PlayerPrefs.SetInt("CarSelect", style);
    }

    public void ChangeThemes(int dir)
    {
        nextTheme += dir;
        CarMenu carAdd = Car[style].GetComponent<CarMenu>();

        if (nextTheme == carAdd.Theme.Length)
        {
            Car[style].SetActive(false);

            nextTheme = 0;
            if (style == 0) style = 1;
            else style = 0;

            Car[style].SetActive(true);

            carAdd = Car[style].GetComponent<CarMenu>();
        }

        if (nextTheme < 0)
        {
            Car[style].SetActive(false);

            if (style == 0) style = 1;
            else style = 0;

            Car[style].SetActive(true);

            carAdd = Car[style].GetComponent<CarMenu>();

            nextTheme = Car[style].GetComponent<CarMenu>().Theme.Length - 1;
        }

        carAdd.CarSet(Car[style].GetComponent<CarMenu>().Theme[nextTheme]);

        PlayerPrefs.SetInt("CarSelect", style);
        gameManager.styleSelect = style;
    }

    public void Back(int menu)
    {
        switch (menu)
        {
            case 0: //Menu Principal
                ChangeGroup(SelectMode, MenuPrincipal);
                break;
            case 1: //Menu Option
                ChangeGroup(OptionMenu, MenuPrincipal);
                break;
            case 2: //Menu Select
                StartCoroutine(CameraPosition(1));
                break;
            case 3: //Menu Select Car - Theme
                ChangeGroup(Theme, SelectCar);
                break;
            case 4: //Menu Select Car - Custom
                ChangeGroup(Custom, SelectCar);
                break;
            case 5: //Menu Select Car - Random
                ChangeGroup(Randomize, SelectCar);
                break;
            case 6: //SinglePlayer Config
                StartCoroutine(PilarPosition(1));                
                break;
        }
    }

    /// <summary>
    /// Cambia entre los canvas group
    /// </summary>
    /// <param name="actualGroup">Grupo Actual</param>
    /// <param name="nextGroup">Siguiente Grupo</param>
    private void ChangeGroup(CanvasGroup actualGroup , CanvasGroup nextGroup)
    {
        actualGroup.alpha = 0;
        actualGroup.interactable = false;
        actualGroup.blocksRaycasts = false;

        nextGroup.alpha = 1;
        nextGroup.interactable = true;
        nextGroup.blocksRaycasts = true;
    }

    private IEnumerator PilarPosition(int mode)
    {
        switch (mode)
        {
            case 0:
                SelectMode.alpha = 0;
                SelectMode.interactable = false;
                SelectMode.blocksRaycasts = false;

                for (float t = 0; t < duration; t += Time.deltaTime)
                {
                    PilarPrincipal.transform.position = Vector3.Lerp(PilarPrincipal.transform.position, PosicionPilares[0], 0.1f);
                    Pilares[0].transform.position = Vector3.Lerp(Pilares[0].transform.position, PosicionPilares[1], 0.1f);
                    Pilares[1].transform.position = Vector3.Lerp(Pilares[1].transform.position, PosicionPilares[2], 0.1f);
                    Pilares[2].transform.position = Vector3.Lerp(Pilares[2].transform.position, PosicionPilares[3], 0.1f);
                    yield return null;
                }

                PilarPrincipal.transform.position = PosicionPilares[0];
                Pilares[0].transform.position = PosicionPilares[1];
                Pilares[1].transform.position = PosicionPilares[2];
                Pilares[2].transform.position = PosicionPilares[3];

                SinglePlayerConfig.alpha = 1;
                SinglePlayerConfig.interactable = true;
                SinglePlayerConfig.blocksRaycasts = true;

                break;

            case 1:

                SinglePlayerConfig.alpha = 0;
                SinglePlayerConfig.interactable = false;
                SinglePlayerConfig.blocksRaycasts = false;

                foreach (var bot in bots)
                {
                    Destroy(bot);
                }
                BotCounts = 0;

                for (float t = 0; t < duration; t += Time.deltaTime)
                {
                    PilarPrincipal.transform.position = Vector3.Lerp(PilarPrincipal.transform.position, PilarInicial, 0.1f);
                    Pilares[0].transform.position = Vector3.Lerp(Pilares[0].transform.position, PosicionPilaresHide[1], 0.1f);
                    Pilares[1].transform.position = Vector3.Lerp(Pilares[1].transform.position, PosicionPilaresHide[2], 0.1f);
                    Pilares[2].transform.position = Vector3.Lerp(Pilares[2].transform.position, PosicionPilaresHide[3], 0.1f);
                    yield return null;
                }

                PilarPrincipal.transform.position = PilarInicial;
                Pilares[0].transform.position = PosicionPilaresHide[1];
                Pilares[1].transform.position = PosicionPilaresHide[2];
                Pilares[2].transform.position = PosicionPilaresHide[3];

                SelectMode.alpha = 1;
                SelectMode.interactable = true;
                SelectMode.blocksRaycasts = true;
                break;
        }
    }

    public IEnumerator CameraPosition(int mode)
    {
        switch (mode)
        {
            case 0:
                SelectMode.alpha = 0;
                SelectMode.interactable = false;
                SelectMode.blocksRaycasts = false;

                for (float t = 0; t < duration; t += Time.deltaTime)
                {
                    Camera.position = Vector3.SmoothDamp(Camera.position, CustomPosition, ref velocity, 0.25f);
                    yield return null;
                }

                Camera.position = CustomPosition;

                SelectCar.alpha = 1;
                SelectCar.interactable = true;
                SelectCar.blocksRaycasts = true;

                break;

            case 1:

                SelectCar.alpha = 0;
                SelectCar.interactable = false;
                SelectCar.blocksRaycasts = false;

                for (float t = 0; t < duration; t += Time.deltaTime)
                {
                    Camera.position = Vector3.SmoothDamp(Camera.position, CamPosition, ref velocity, 0.25f);
                    yield return null;
                }

                Camera.position = CamPosition;

                SelectMode.alpha = 1;
                SelectMode.interactable = true;
                SelectMode.blocksRaycasts = true;
                break;
        }
    }

    public void ChangeChassis(int Direccion)
    {
        CarMenu carScript = Car[style].GetComponent<CarMenu>();

        carScript.cT.Mat += Direccion;

        if (carScript.cT.Mat == carScript.ChassisMaterials.Length)
        {
            carScript.cT.Mat = 0;
        }

        if (carScript.cT.Mat < 0)
        {
            carScript.cT.Mat = carScript.ChassisMaterials.Length - 1;
        }

        carScript.car.GetComponent<Renderer>().material = carScript.ChassisMaterials[carScript.cT.Mat];

        switch (style)
        {
            case 0:
                PlayerPrefs.SetInt("Mat0", carScript.cT.Mat);
                break;
            case 1:
                PlayerPrefs.SetInt("Mat1", carScript.cT.Mat);
                break;
        }
    }

    public void ChangeWheelFront(int Direccion)
    {
        CarMenu carScript = Car[style].GetComponent<CarMenu>();
        carScript.WheelFront[carScript.cT.wheelF].GetComponent<Renderer>().enabled = false;

        carScript.cT.wheelF += Direccion;

        if (carScript.cT.wheelF == carScript.WheelFront.Length)
        {
            carScript.cT.wheelF = 0;
        }

        if (carScript.cT.wheelF < 0)
        {
            carScript.cT.wheelF = carScript.WheelFront.Length - 1;
        }

        carScript.WheelFront[carScript.cT.wheelF].GetComponent<Renderer>().enabled = true;

        switch (style)
        {
            case 0:
                PlayerPrefs.SetInt("wheelF0", carScript.cT.wheelF);
                break;
            case 1:
                PlayerPrefs.SetInt("wheelF1", carScript.cT.wheelF);
                break;
        }
    }

    public void ChangeWheelBack(int Direccion)
    {
        CarMenu carScript = Car[style].GetComponent<CarMenu>();
        carScript.WheelBack[carScript.cT.wheelB].GetComponent<Renderer>().enabled = false;

        carScript.cT.wheelB += Direccion;

        if (carScript.cT.wheelB == carScript.WheelBack.Length)
        {
            carScript.cT.wheelB = 0;
        }

        if (carScript.cT.wheelB < 0)
        {
            carScript.cT.wheelB = carScript.WheelBack.Length - 1;
        }

        carScript.WheelBack[carScript.cT.wheelB].GetComponent<Renderer>().enabled = true;

        switch (style)
        {
            case 0:
                PlayerPrefs.SetInt("wheelB0", carScript.cT.wheelB);
                break;
            case 1:
                PlayerPrefs.SetInt("wheelB1", carScript.cT.wheelB);
                break;
        }
    }

    public void ChangeFront(int Direccion)
    {
        CarMenu carScript = Car[style].GetComponent<CarMenu>();
        carScript.AddonFront[carScript.cT.aFront].GetComponent<Renderer>().enabled = false;
        carScript.cT.aFront += Direccion;

        if (carScript.cT.aFront == carScript.AddonFront.Length)
        {
            carScript.cT.aFront = 0;
        }

        if (carScript.cT.aFront < 0)
        {
            carScript.cT.aFront = carScript.AddonFront.Length - 1;
        }

        carScript.AddonFront[carScript.cT.aFront].GetComponent<Renderer>().enabled = true;

        switch (style)
        {
            case 0:
                PlayerPrefs.SetInt("aFront0", carScript.cT.aFront);
                break;
            case 1:
                PlayerPrefs.SetInt("aFront1", carScript.cT.aFront);
                break;
        }
    }

    public void ChangeMiddle(int Direccion)
    {
        CarMenu carScript = Car[style].GetComponent<CarMenu>();
        carScript.AddonMiddle[carScript.cT.aMiddle].GetComponent<Renderer>().enabled = false;
        carScript.cT.aMiddle += Direccion;

        if (carScript.cT.aMiddle == carScript.AddonMiddle.Length)
        {
            carScript.cT.aMiddle = 0;
        }

        if (carScript.cT.aMiddle < 0)
        {
            carScript.cT.aMiddle = carScript.AddonMiddle.Length - 1;
        }

        carScript.AddonMiddle[carScript.cT.aMiddle].GetComponent<Renderer>().enabled = true;

        switch (style)
        {
            case 0:
                PlayerPrefs.SetInt("aMiddle0", carScript.cT.aMiddle);    
                break;
            case 1:
                PlayerPrefs.SetInt("aMiddle1", carScript.cT.aMiddle);
                break;
        }
    }

    public void ChangeBack(int Direccion)
    {
        CarMenu carScript = Car[style].GetComponent<CarMenu>();
        carScript.AddonBack[carScript.cT.aBack].GetComponent<Renderer>().enabled = false;
        carScript.cT.aBack += Direccion;

        if (carScript.cT.aBack == carScript.AddonBack.Length)
        {
            carScript.cT.aBack = 0;
        }

        if (carScript.cT.aBack < 0)
        {
            carScript.cT.aBack = carScript.AddonBack.Length - 1;
        }

        carScript.AddonBack[carScript.cT.aBack].GetComponent<Renderer>().enabled = true;

        switch (style)
        {
            case 0:
                PlayerPrefs.SetInt("aBack0", carScript.cT.aBack);
                break;
            case 1:
                PlayerPrefs.SetInt("aBack1", carScript.cT.aBack);
                break;
        }
    }

    private void SetPlayerProperties()
    {
        CarMenu cs = Car[style].GetComponent<CarMenu>();
        playerStats.Add("Style" , style);
        playerStats.Add("Mat" , cs.cT.Mat);
        playerStats.Add("Front", cs.cT.aFront);
        playerStats.Add("Middle", cs.cT.aMiddle);
        playerStats.Add("Back", cs.cT.aBack);
        playerStats.Add("WheelF", cs.cT.wheelF);
        playerStats.Add("WheelB", cs.cT.wheelB);

        gameManager.PlayerData[0] = cs.cT.Mat;
        gameManager.PlayerData[1] = cs.cT.aFront;
        gameManager.PlayerData[2] = cs.cT.aMiddle;
        gameManager.PlayerData[3] = cs.cT.aBack;
        gameManager.PlayerData[4] = cs.cT.wheelF;
        gameManager.PlayerData[5] = cs.cT.wheelB;

        PhotonNetwork.SetPlayerCustomProperties(playerStats);
    }

    public void Add_Delete_Bot(int add_delete)
    {
        switch (add_delete)
        {
            case 0:
                if(BotCounts < 3)
                {
                    BotCounts++;
                    gameManager.PlayerCount ++;
                    int random = Random.Range(0, 1);
                    gameManager.RandomBot(BotCounts - 1 , random);

                    bots[BotCounts - 1] = PhotonNetwork.Instantiate("BuggyMenu" , Vector3.zero , FirstRotation, 0 , gameManager.BotData[BotCounts - 1]);
                    bots[BotCounts - 1].transform.position = new Vector3(Pilares[BotCounts - 1].transform.position.x,
                        Pilares[BotCounts - 1].transform.position.y + 0.9f , Pilares[BotCounts - 1].transform.position.z);
                    bots[BotCounts - 1].transform.parent = Pilares[BotCounts - 1].transform;
                }
                break;
            case 1:
                if(BotCounts > 0)
                {
                    Destroy(bots[BotCounts - 1]);
                    gameManager.BotData.Remove(gameManager.BotData[BotCounts-1]);

                    BotCounts--;
                    gameManager.PlayerCount--;
                }
                break;
        }
    }

    public void ChangeMap(int direction)
    {
        mapSelect += direction;

        if (mapSelect == Mapas.Length)
            mapSelect = 0;
        else if (mapSelect < 0)
            mapSelect = Mapas.Length - 1;

        MapaName.text = Mapas[mapSelect];
    }


}
