using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BomberCar.Utility {
    public static class MethodExtensions
    {
        public static string RemoveQuotes(this string Value) {
            return Value.Replace("\"", "");
        }

        public static string ChangeDot(this string Value)
        {
            return Value.Replace(",", ".");
        }

        public static string ChangeComma(this string Value)
        {
            return Value.Replace(".", ",");
        }

        public static float TwoDecimals(this float Value)
        {
            return Mathf.Round(Value * 1000.0f) /1000.0f;
        }
    }
}