using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSelene {
    public static class CalculationHandler {

        public static Vector3 ConvertDataset(Dataset dataset) {
            float _radius = 1737.4f + dataset.height; // 1737.4 is the lunar radius
            Vector3 _vector3 = new Vector3 {
                x = dataset.height * Mathf.Cos(dataset.latitude) * Mathf.Cos(dataset.longitude),
                z = dataset.height * Mathf.Cos(dataset.latitude) * Mathf.Sin(dataset.longitude),
                y = dataset.height * Mathf.Sin(dataset.latitude)
            };
            return _vector3;
        }
    }
}