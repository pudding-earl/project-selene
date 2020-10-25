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
            ReadData();
        }
        
        private void ReadData() {
            string[] _datasetStrings = dataFile.text.Split(lineSeparator);

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
            Debug.Log($"Data read and converted with {data.Count} sets.");
            StartCoroutine(terrainGenerator.GeneratePlaceholders(data, 0.01f, 10000));

        }
    }

    public struct Dataset {
        public float latitude;
        public float longitude;
        public float height;
        public float slope;
    }
}