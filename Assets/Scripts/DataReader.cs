using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace ProjectSelene {
    public class DataReader : MonoBehaviour {
        [SerializeField] private TextAsset dataFile; // Reference to the CSV file being reads
        [SerializeField] private TerrainGenerator terrainGenerator;

        private List<Dataset> data = new List<Dataset>(); // The data from the file after it has been converted

        private char lineSeparator = '\n'; // Separates different datasets in the file
        private char fieldSeparator = ','; // Separates individual values in the datasets

        private void Start() {
            StartCoroutine(ReadData());
        }
        
        // TODO: consider changing this to an async task if time permits
        private IEnumerator ReadData() {
            Debug.Log("Reading and parsing data...");
            string[] _datasetStrings = dataFile.text.Split(lineSeparator);
            Debug.Log("Data read and parsed successfully.");

            Debug.Log("Converting data...");
            yield return new WaitForSeconds(0.25f); // Delay to allow proper debug logging

            for(int i = 0; i < _datasetStrings.Length - 1; i++) {
                string[] _splitData = _datasetStrings[i].Split(fieldSeparator);
                Dataset _dataset = new Dataset {
                    latitude = float.Parse(_splitData[0]),
                    longitude = float.Parse(_splitData[1]),
                    height = float.Parse(_splitData[2]),
                    slope = float.Parse(_splitData[3])
                };

                data.Add(_dataset);
            }
            Debug.Log($"Data converted with {data.Count} sets.");
            //terrainGenerator.ShowPlaceholders(data, 100000);

        }
    }

    public struct Dataset {
        public float latitude;
        public float longitude;
        public float height;
        public float slope;
    }
}