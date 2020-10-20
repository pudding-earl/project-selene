using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSelene {
    public static class CalculationHandler {

        private static float lunarRadius = 1737.4f;
        
        public static Vector3 ConvertDataset(Dataset dataset) {
            Vector3 _vector3 = new Vector3 {
                x = lunarRadius * Mathf.Cos(dataset.latitude) * Mathf.Cos(dataset.longitude),
                y = lunarRadius * Mathf.Cos(dataset.latitude) * Mathf.Sin(dataset.longitude),
                z = lunarRadius * Mathf.Sin(dataset.latitude)
            };
            return _vector3;
        }
    }
}