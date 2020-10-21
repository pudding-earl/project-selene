using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSelene {
    public static class CalculationHandler {

        private static float lunarRadius = 1737.4f;
        
        public static Vector3 ConvertDataset(Dataset dataset) {
            float _radius = lunarRadius + dataset.height;
            Vector3 _vector3 = new Vector3 {
                x = _radius * Mathf.Cos(dataset.latitude) * Mathf.Cos(dataset.longitude),
                z = _radius * Mathf.Cos(dataset.latitude) * Mathf.Sin(dataset.longitude),
                y = _radius * Mathf.Sin(dataset.latitude)
            };
            return _vector3;
        }
    }
}