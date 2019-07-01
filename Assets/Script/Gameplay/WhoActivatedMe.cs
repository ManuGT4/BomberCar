using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BomberCar.Utility.Attributes;

namespace BomberCar.Gameplay
{
    public class WhoActivatedMe : MonoBehaviour
    {
        [SerializeField]
        [GreyOut]
        private string whoActivatedMe;

        public void SetActivator(string ID)
        {
            whoActivatedMe = ID;
        }

        public string GetActivator()
        {
            return whoActivatedMe;
        }
    }

}
