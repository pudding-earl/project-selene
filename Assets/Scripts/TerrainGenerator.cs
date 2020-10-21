using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSelene {
    public class TerrainGenerator : MonoBehaviour {
        [SerializeField] private GameObject placeholder; // A placeholder object to represent datapoint positions

        public IEnumerator GeneratePlaceholders(List<Dataset> data, float delay = 0.1f, int amount = 100, int startingIndex = 0) {
            for(int i = 0; i < amount; i++) {
                ShowPlaceholder(data[i]);
                yield return new WaitForSeconds(delay);
            }
        }

        private void ShowPlaceholder(Dataset set) {
            GameObject _holder = Instantiate(placeholder);
            _holder.transform.position = CalculationHandler.ConvertDataset(set);
        }
        
    }
}