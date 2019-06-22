using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BomberCar.Utility;
using BomberCar.Utility.Attributes;

namespace BomberCar.Networking
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkTransform : MonoBehaviour
    {
        [SerializeField]
        [GreyOut]
        private Vector3 oldPosition;

        private NetworkIdentity networkIdentity;
        private Player player;

        private float stillCounter = 0;

        // Start is called before the first frame update
        void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            oldPosition = transform.position;
            player = new Player();
            player.position = new Position();
            player.position.x = "0";
            player.position.y = "0";

            if (!networkIdentity.Iscontrolling())
            {
                enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (networkIdentity.Iscontrolling())
            {
                if(oldPosition != transform.position)
                {
                    oldPosition = transform.position;
                    stillCounter = 0;
                    SendData();
                }else
                {
                    stillCounter += Time.deltaTime;

                    if(stillCounter >= 1)
                    {
                        stillCounter = 0;
                        SendData();
                    }
                }
            }
        }

        private void SendData()
        {
            //Update player information
            player.position.x = transform.position.x.TwoDecimals().ToString().ChangeDot();
            player.position.y = transform.position.y.TwoDecimals().ToString().ChangeDot();

            networkIdentity.GetSocket().Emit("updatePosition" , new JSONObject(JsonUtility.ToJson(player)));
        }
    }
}
