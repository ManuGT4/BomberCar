using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BomberCar.Networking;
using BomberCar.Utility;

namespace BomberCar.Player
{
    public class PlayerManager : MonoBehaviour
    {
        const float BARREL_PIVOT_OFFSET = 90.0f;

        [Header("Data")]
        [SerializeField]
        private float speed = 2;
        [SerializeField]
        private float rotation = 60;

        [Header("Object References")]
        [SerializeField]
        private Transform barrelPivot;
        [SerializeField]
        private Transform bulletSpawnPoint;

        [Header("Class References")]
        [SerializeField]
        private NetworkIdentity networkIdentity;

        private float lastRotation;

        //Shooting
        private BulletData bulletData;
        private Cooldown shootingCooldown;

        private void Start()
        {
            shootingCooldown = new Cooldown(1);
            bulletData = new BulletData();
            bulletData.position = new Position();
            bulletData.direction = new Position();
        }

        // Update is called once per frame
        void Update()
        {
            if (networkIdentity.Iscontrolling())
            {
                CheckMovement();
                CheckAiming();
                CheckShooting();
            }
        }

        public float GetLastRotation()
        {
            return lastRotation;
        }

        public void SetRotation(float Value)
        {
            barrelPivot.rotation = Quaternion.Euler(0, 0, Value + BARREL_PIVOT_OFFSET);
        }

        private void CheckMovement()
        {
            float horizontal = Input.GetAxis("Horizontal_J1");
            float vertical = Input.GetAxis("Vertical_J1");

            transform.position += transform.up * vertical * speed * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, -horizontal * rotation * Time.deltaTime));
        }

        private void CheckAiming()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();
            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;

            lastRotation = rot;

            barrelPivot.rotation = Quaternion.Euler(0, 0, rot + BARREL_PIVOT_OFFSET);
        }

        private void CheckShooting()
        {
            shootingCooldown.CooldownUpdate();

            if (Input.GetMouseButton(0) && !shootingCooldown.IsOnCooldown())
            {
                shootingCooldown.StartCooldown();

                //Define bullet
                bulletData.activator = NetworkClient.ClientID;
                bulletData.position.x = bulletSpawnPoint.position.x.TwoDecimals().ToString().ChangeDot();
                bulletData.position.y = bulletSpawnPoint.position.y.TwoDecimals().ToString().ChangeDot();
                bulletData.direction.x = bulletSpawnPoint.up.x.ToString().ChangeDot();
                bulletData.direction.y = bulletSpawnPoint.up.y.ToString().ChangeDot();

                //Send bullet
                networkIdentity.GetSocket().Emit("fireBullet", new JSONObject(JsonUtility.ToJson(bulletData)));
            } 
        }
    }
}
