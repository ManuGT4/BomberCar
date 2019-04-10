using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GplayManager : MonoBehaviourPun {

    //Componentes
    private GameManager gameManager;
    private Camera cam;

    //Variables privadas
    private int carStyle;
    private int gameMode;


    [Header("Lugar de aparicion")]
    public Transform[] Reaparecer;

    [Header("UI")]
    public Joystick L3;
    public Text Crono;

    [Header("Jugadores")]
    public int PlayerCount;

    [Header("Teams")]
    public GameObject[] Red;
    public GameObject[] Blue;
    public GameObject[] Green;
    public GameObject[] Violet;

    [Header("Time")]
    public float duracion = 5.0f; //Valor en minutos
    public bool Finish = false;

    private const string Duracion = "td";

     
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        carStyle = gameManager.styleSelect;
        gameMode = gameManager.GameMode;
        cam = Camera.main;
    }

    void Start ()
    {
        PlayerCount = gameManager.PlayerCount;
        GameObject target;
        switch(carStyle)
        {
            case 0:
                target = PhotonNetwork.Instantiate("Buggy", Reaparecer[0].position, Quaternion.identity, 0 , gameManager.PlayerData);
                cam.GetComponent<CameraScript>().Player = target;
                break;
            case 1:
                target = PhotonNetwork.Instantiate("Racer", Reaparecer[0].position, Quaternion.identity, 0, gameManager.PlayerData);
                cam.GetComponent<CameraScript>().Player = target;
                break;
        }

        for (int i = 1; i < PlayerCount; i++)
        {
            PhotonNetwork.InstantiateSceneObject("BuggyBot", Reaparecer[i].position, Quaternion.identity,0 , gameManager.BotData[i - 1]);
        }
        duracion = duracion * 60; //Se pasa de minutos a segundos
	}
	
	void Update ()
    {
    }

    private void LateUpdate()
    {
        if (duracion != 0)
        {
            int min = Mathf.FloorToInt(duracion / 60);
            int sec = Mathf.FloorToInt(duracion % 60);
            Crono.text = min.ToString("00") + ":" + sec.ToString("00");

            if(PhotonNetwork.IsMasterClient)
                duracion -= Time.deltaTime;

            Hashtable time = new Hashtable() { { Duracion, duracion } };

            PhotonNetwork.CurrentRoom.SetCustomProperties(time);
        }else if (Finish)
        {
            StartCoroutine(GameOver());
        }
    }

    private IEnumerator GameOver()
    {
        yield return null;
    }

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {

    }
}
