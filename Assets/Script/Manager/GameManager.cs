using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BomberCar.Utility.Attributes;

namespace BomberCar.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager = null; 
        private Camera camera;

        [Header("Player Set")]
        [SerializeField]
        private GameObject player;

        [Header("Bots Config")]
        [GreyOut]
        [SerializeField]
        private int botCounts = 0;
        [SerializeField]
        private GameObject botPrefab;
        private GameObject[] bots = new GameObject[3];

        [Header("Plataformas")]
        public Transform[] Platforms;


        void Awake()
        {
            if (gameManager == null) gameManager = this;
            else if (gameManager != this) Destroy(gameManager);

            DontDestroyOnLoad(this);

            camera = Camera.main;
        }

        void Start()
        {
            player = Instantiate(player, Platforms[0]);
            player.GetComponent<Car>().Main = true;
        }

        public GameObject GetPlayer()
        {
            return player;
        }

        public Camera GetCamera()
        {
            return camera;
        }

        public GameObject[] GetBots()
        {
            return bots;
        }

        public void SetBotsCount(int count)
        {
            botCounts = count;
        }

        /// <summary>
        /// Add or Delete Bot
        /// </summary>
        /// <param name="add_delete">Add(0) or Delete(1)</param>
        public void Add_Delete_Bot(int add_delete)
        {
            switch (add_delete)
            {
                case 1:
                    if (botCounts < 3)
                    {
                        bots[botCounts] = Instantiate(botPrefab, Platforms[botCounts + 1]);

                        bots[botCounts].transform.parent = Platforms[botCounts + 1].transform;
                        botCounts++;
                    }
                    break;
                case -1:
                    if (botCounts > 0)
                    {
                        botCounts--;
                        Destroy(bots[botCounts]);
                    }
                    break;
            }
        }

    }
}
