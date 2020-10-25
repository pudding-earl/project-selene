using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSelene {
    public class TerrainGenerator : MonoBehaviour {
        [SerializeField] private GameObject placeholder; // A placeholder object to represent datapoint positions

        public IEnumerator GeneratePlaceholders(List<Dataset> data, float delay = 0.1f, int amount = 100, int startingIndex = 0) {
            for(int i = startingIndex; i < amount + startingIndex; i++) {
                ShowPlaceholder(data[i]);
                yield return true;
            }
        }

        private void ShowPlaceholder(Dataset set) {
            GameObject _holder = Instantiate(placeholder, transform);
            _holder.transform.position = CalculationHandler.ConvertDataset(set);
        }
        
    }
}