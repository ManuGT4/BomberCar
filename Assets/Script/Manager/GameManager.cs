using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

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


    public void PartidaOffline(int mapa)
    {
        SceneManager.LoadScene(mapa);
    }
}
