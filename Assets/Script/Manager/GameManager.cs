using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks {

    [Header("Variables")]
    public int styleSelect;
    public int GameMode;
    public int PlayerCount = 1;

    string gameVersion = "1";


    //Player Data
    public object[] PlayerData = new object[6];

    //Bot Data
    public List<object[]> BotData = new List<object[]>();


    void Awake()
    {
        Object.DontDestroyOnLoad(this);
    }

    void Start ()
    {
	}

    public void RandomBot(int bot , int carStyle)
    {
        object[] botTmp = new object[6];
        switch (carStyle)
        {
            case 0:
                botTmp[0] = Random.Range(0, 7);
                botTmp[1] = Random.Range(0, 8);
                botTmp[2] = Random.Range(0, 8);
                botTmp[3] = Random.Range(0, 7);
                botTmp[4] = Random.Range(0, 4);
                botTmp[5] = Random.Range(0, 4);
                break;
            case 1:
                botTmp[0] = Random.Range(0, 1);
                botTmp[1] = Random.Range(0, 3);
                botTmp[2] = Random.Range(0, 1);
                botTmp[3] = Random.Range(0, 5);
                botTmp[4] = Random.Range(0, 1);
                botTmp[5] = Random.Range(0, 1);
                break;
        }
        BotData.Add(botTmp);
    }

    public void Connect()
    {

        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void StartPartida()
    {
        Connect();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No random room available, so we create one");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Se creo");
        base.OnJoinedRoom();
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        PhotonNetwork.JoinRandomRoom();
    }
    /*
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player entro a la room ", newPlayer.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            SceneManager.LoadScene(1);
        }
    }
    */

    public void PartidaOffline(int mapa)
    {
        PhotonNetwork.OfflineMode = true;
        SceneManager.LoadScene(mapa);
    }
}
