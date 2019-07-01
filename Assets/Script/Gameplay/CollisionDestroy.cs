using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BomberCar.Networking;

namespace BomberCar.Gameplay
{
    public class CollisionDestroy : MonoBehaviour
    {
        [SerializeField]
        private NetworkIdentity networkIdentity;
        [SerializeField]
        private WhoActivatedMe whoActivatedMe;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            NetworkIdentity ni = collision.gameObject.GetComponent<NetworkIdentity>();
            if(ni == null || ni.GetID() != whoActivatedMe.GetActivator())
            {
                networkIdentity.GetSocket().Emit("collisionDestroy", new JSONObject(JsonUtility.ToJson(new IDdata()
                {
                    id = networkIdentity.GetID()
                })));
            }
        }
    }
}