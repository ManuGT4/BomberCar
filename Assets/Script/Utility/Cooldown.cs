using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BomberCar.Utility
{
    public class Cooldown
    {
        private float lenght;
        private float currentTime;
        private bool onCooldown;

        public Cooldown(float Lenght = 1 , bool StartWithCooldown = false)
        {
            currentTime = 0;
            lenght = Lenght;
            onCooldown = StartWithCooldown;
        }

        public void CooldownUpdate()
        {
            if (onCooldown)
            {
                currentTime += Time.deltaTime;

                if(currentTime >= lenght)
                {
                    currentTime = 0;
                    onCooldown = false;
                }
            }
        }

        public bool IsOnCooldown()
        {
            return onCooldown;
        }

        public void StartCooldown()
        {
            onCooldown = true;
            currentTime = 0;
        }
    }
}
