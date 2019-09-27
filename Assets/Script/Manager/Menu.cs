using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BomberCar.Manager;

public class Menu : MonoBehaviour
{
    //Componentes
    private GameManager gameManager;

    [Header("Canvas Objects")]
    [SerializeField]
    private GameObject selectMode;
    [SerializeField]
    private GameObject playMode;
    [SerializeField]
    private GameObject selectCar;

    //Camera Config
    private Camera cam;
    private Vector3 velocity = Vector3.zero;
    private Vector3 CamPosition = new Vector3(0, 3, -6);
    private Vector3 CustomPosition = new Vector3(1.75f, 1.8f, -4);

    [Header("Platform Config")]
    //Platform Config    
    [SerializeField]
    public Vector3[] PositionPlatform;
    [SerializeField]
    public Vector3[] PositionHide;


    public void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Start()
    {
        cam = gameManager.GetCamera();
    }

    #region BOTONES

    public void ChangeCar()
    {
        gameManager.GetPlayer().GetComponent<Car>().ChangeCar();
    }

    public void ChangeChasis(int dir)
    {
        gameManager.GetPlayer().GetComponent<Car>().ChangeChassis(dir); 
    }

    public void ChangeWheelFront(int dir)
    {
        gameManager.GetPlayer().GetComponent<Car>().ChangeWheelFront(dir);
    }

    public void ChangeWheelBack(int dir)
    {
        gameManager.GetPlayer().GetComponent<Car>().ChangeWheelBack(dir);
    }

    public void ChangeFront(int dir)
    {
        gameManager.GetPlayer().GetComponent<Car>().ChangeFront(dir);
    }

    public void ChangeMiddle(int dir)
    {
        gameManager.GetPlayer().GetComponent<Car>().ChangeMiddle(dir);
    }

    public void ChangeBack(int dir)
    {
        gameManager.GetPlayer().GetComponent<Car>().ChangeBack(dir);
    }

    public void RandomButton()
    {
        gameManager.GetPlayer().GetComponent<Car>().RandomizeCar();
    }

    public void Bots(int dir)
    {
        gameManager.Add_Delete_Bot(dir);
    }

    #endregion

    #region TRANISICIONES EN EL MENU

    public void PlatformInit()
    {
        StartCoroutine(PlatformPosition());
    }

    /// <summary>
    /// Make the transicion between a Selectmode and GameMode
    /// </summary>
    /// <param name="mode">Singleplayer or Multiplayer</param>
    /// <returns></returns>
    private IEnumerator PlatformPosition()
    {
        if (selectMode.activeSelf)
        {
            selectMode.SetActive(false);

            int tmp = 0;
            for (float t = 0; t < 0.8f ; t += Time.deltaTime)
            {
                foreach (var platform in gameManager.Platforms)
                {
                    platform.position = Vector3.Lerp(platform.position, PositionPlatform[tmp], 0.1f);
                    tmp++;
                }
                tmp = 0;
                yield return null;
            }

            foreach (var platform in gameManager.Platforms)
            {
                platform.position = PositionPlatform[tmp];
                tmp++;
            }

            playMode.SetActive(true);
        } else {
            playMode.SetActive(false);

            foreach (var bot in gameManager.GetBots()) {
                Destroy(bot);
            }
            gameManager.SetBotsCount(0);

            int tmp = 0;
            for (float t = 0; t < 0.8f; t += Time.deltaTime)
            {
                foreach (var platform in gameManager.Platforms)
                {
                    platform.position = Vector3.Lerp(platform.position, PositionHide[tmp] , 0.1f);
                    tmp++;
                }
                tmp = 0;
                yield return null;
            }

            foreach (var platform in gameManager.Platforms)
            {
                platform.position = PositionHide[tmp];
                tmp++;
            }

            selectMode.SetActive(true);
        }
    }

    public void MoveCamera(int position)
    {
        StartCoroutine(CameraPosition(position));
    }

    /// <summary>
    /// Make camera movements
    /// </summary>
    /// <param name="position">First position (0) or Second position (1)</param>
    /// <returns></returns>
    public IEnumerator CameraPosition(int position)
    {
        switch (position)
        {
            case 0:
                selectMode.SetActive(false);

                for (float t = 0; t < 0.8f; t += Time.deltaTime)
                {
                    cam.transform.position = Vector3.SmoothDamp (cam.transform.position, CustomPosition,  ref velocity, 0.25f);
                    yield return null;
                }

                cam.transform.position = CustomPosition;

               selectCar.SetActive(true);
                break;

            case 1:

                selectCar.SetActive(false);

                for (float t = 0; t < 0.8f; t += Time.deltaTime)
                {
                    cam.transform.position = Vector3.SmoothDamp(cam.transform.position, CamPosition, ref velocity, 0.25f);
                    yield return null;
                }

                cam.transform.position = CamPosition;

                selectMode.SetActive(true);
                break;
        }
    }

    #endregion
}
