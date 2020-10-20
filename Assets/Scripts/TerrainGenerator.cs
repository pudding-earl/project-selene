using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSelene {
    public class TerrainGenerator : MonoBehaviour {
        [SerializeField] private GameObject placeholder; // A placeholder object to represent datapoint positions

        public void ShowPlaceholders(List<Dataset> data, int divisor = 1000) {
            for(int i = 0; i < data.Count; i++) {
                foreach(Dataset _set in data) {
                    if(i % divisor == 0) {
                        //Instantiate(placeholder, CalculationHandler.ConvertDataset(_set), Quaternion.identity);
                        print(i);
                    }
                }
            }
        }
    }
}