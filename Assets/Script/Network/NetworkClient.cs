using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using BomberCar.Utility;
using BomberCar.Player;
using BomberCar.Scriptable;
using BomberCar.Gameplay;

namespace BomberCar.Networking
{
    public class NetworkClient : SocketIOComponent
    {
        public const float SERVER_UPDATE_TIME = 10;

        [Header("Network Client")]
        [SerializeField]
        private Transform networkContainer;
        [SerializeField]
        private GameObject playerPrefab;
        [SerializeField]
        private ServerObjects serverSpawnables;

        public static string ClientID { get; private set; }

        private Dictionary<string, NetworkIdentity> serverObjects;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            Initialize();
            SetupEvents();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        private void Initialize()
        {
            serverObjects = new Dictionary<string, NetworkIdentity>();
        }

        private void SetupEvents()
        {
            On("open", (E) =>
            {
                Debug.Log("Connection made to the server");
            });

            On("register", (E) =>
            {
                ClientID = E.data["id"].ToString().RemoveQuotes();

                Debug.LogFormat("Our Client's ID ({0})", ClientID);
            });

            On("spawn", (E) =>
            {
                //Handling all spawing all players
                //Passed data
                string id = E.data["id"].ToString().RemoveQuotes();

                GameObject go = Instantiate(playerPrefab, networkContainer);
                go.name = string.Format("Player({0})", id);
                NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
                ni.SetControllerID(id);
                ni.SetSocketReference(this);
                serverObjects.Add(id, ni);
            });

            On("disconnected", (E) =>
            {
                string id = E.data["id"].ToString().RemoveQuotes();

                GameObject go = serverObjects[id].gameObject;
                Destroy(go); //Remove from game
                serverObjects.Remove(id); //Remove from memory
            });

            On("updatePosition", (E) => {
                string id = E.data["id"].ToString().RemoveQuotes();

                float x = float.Parse(E.data["position"]["x"].str.ChangeComma());
                float y = float.Parse(E.data["position"]["y"].str.ChangeComma());

                NetworkIdentity ni = serverObjects[id];
                ni.transform.position = new Vector2(x ,y);
            });

            On("updateRotation", (E) => {
                string id = E.data["id"].ToString().RemoveQuotes();
                float tankRotation = float.Parse(E.data["tankRotation"].str.ChangeComma());
                float barrelRotation = float.Parse(E.data["barrelRotation"].str.ChangeComma());

                NetworkIdentity ni = serverObjects[id];
                ni.transform.localEulerAngles = new Vector3(0, 0, tankRotation);
                ni.GetComponent<PlayerManager>().SetRotation(barrelRotation);
            });

            On("serverSpawn", (E) => {
                string name = E.data["name"].str;
                string id = E.data["id"].ToString().RemoveQuotes();

                float x = float.Parse(E.data["position"]["x"].str.ChangeComma());
                float y = float.Parse(E.data["position"]["y"].str.ChangeComma());

                Debug.LogFormat("Server wants us to spawn a '{0}'", name);

                if (!serverObjects.ContainsKey(id))
                {
                    ServerObjectData sod = serverSpawnables.GetObjectByName(name);
                    var spawnedObject = Instantiate(sod.Prefab, networkContainer);
                    spawnedObject.transform.position = new Vector3(x, y, 0);

                    var ni = spawnedObject.GetComponent<NetworkIdentity>();
                    ni.SetControllerID(id);
                    ni.SetSocketReference(this);

                    //If bullet apply direction as well
                    if (name == "Bullet")
                    {
                        float directionX = float.Parse(E.data["direction"]["x"].str.ChangeComma());
                        float directionY = float.Parse(E.data["direction"]["y"].str.ChangeComma());
                        string activator = E.data["activator"].ToString().RemoveQuotes();
                        float speed = float.Parse(E.data["speed"].str.ChangeComma());

                        float rot = Mathf.Atan2(directionY, directionX) * Mathf.Rad2Deg;
                        Vector3 currentRotation = new Vector3(0, 0, rot - 90);
                        spawnedObject.transform.rotation = Quaternion.Euler(currentRotation);

                        WhoActivatedMe whoActivatedMe = spawnedObject.GetComponent<WhoActivatedMe>();
                        whoActivatedMe.SetActivator(activator);

                        Projectile projectile = spawnedObject.GetComponent<Projectile>();
                        projectile.Direction = new Vector2(directionX, directionY);
                        projectile.Speed = speed;
                    }

                    serverObjects.Add(id, ni);
                }
            });

            On("serverUnspawn", (E) => {              
                string id = E.data["id"].ToString().RemoveQuotes();
                NetworkIdentity ni = serverObjects[id];
                serverObjects.Remove(id);
                DestroyImmediate(ni.gameObject);
            });

            On("playerDied", (E) => {
                string id = E.data["id"].ToString().RemoveQuotes();
                NetworkIdentity ni = serverObjects[id];
                ni.gameObject.SetActive(false);
            });

            On("playerRespawn", (E) => {
                string id = E.data["id"].ToString().RemoveQuotes();
                float x = float.Parse(E.data["position"]["x"].str.ChangeComma());
                float y = float.Parse(E.data["position"]["y"].str.ChangeComma());
                NetworkIdentity ni = serverObjects[id];
                ni.transform.position = new Vector3(x, y, 0);
                ni.gameObject.SetActive(true);
            });
        }

        public void AttemptToJoinLobby()
        {
            Emit("joinGame");
        }
    }

    [Serializable]
    public class Player
    {
        public string id;
        public Position position;
    }

    [Serializable]
    public class Position
    {
        public string x;
        public string y;
    }

    [Serializable]
    public class Rotation
    {
        public string tankRotation;
        public string barrelRotation;
    }

    [Serializable]
    public class BulletData
    {
        public string id;
        public string activator;
        public Position position;
        public Position direction;
    }

    [Serializable]
    public class IDdata
    {
        public string id;
    }
}
