using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BomberCar.Player;
using BomberCar.Utility;
using BomberCar.Utility.Attributes;

namespace BomberCar.Networking
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkRotation : MonoBehaviour
    {
        [Header("Referenced Values")]
        [SerializeField]
        [GreyOut]
        private float oldTankRotation;
        [SerializeField]
        [GreyOut]
        private float oldBarrelRotation;

        [Header("Class References")]
        [SerializeField]
        private PlayerManager playerManager;

        private NetworkIdentity networkIdentity;
        private Rotation player;

        private float stillCounter = 0;

        // Start is called before the first frame update
        void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();

            player = new Rotation();
            player.tankRotation = "0";
            player.barrelRotation = "0";

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
                if(oldTankRotation != transform.localEulerAngles.z || oldBarrelRotation != playerManager.GetLastRotation())
                {
                    oldTankRotation = transform.localEulerAngles.z;
                    oldBarrelRotation = playerManager.GetLastRotation();

                    stillCounter = 0;
                    SendData();
                }
                else
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
            player.tankRotation = transform.localEulerAngles.z.TwoDecimals().ToString();
            player.barrelRotation = playerManager.GetLastRotation().TwoDecimals().ToString();

            networkIdentity.GetSocket().Emit("updateRotation", new JSONObject(JsonUtility.ToJson(player)));
        }
    }
}
