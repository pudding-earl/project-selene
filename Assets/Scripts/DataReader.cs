using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace ProjectSelene {
    public class DataReader : MonoBehaviour
    {
        [SerializeField] private TextAsset dataFile; // Reference to the CSV file being reads

        [SerializeField] private TerrainGenerator terrainGenerator;

        private List<Vector3> data = new List<Vector3>(); // The data from the file after it has been converted
        private Vector3 tempPosition;

        private char lineSeparator = '\n'; // Separates different datasets in the file
        private char fieldSeparator = ','; // Separates individual values in the datasets

        private bool foundCloseObject = false;

        private void Start() {
            ReadData();
        }

        private void ReadData() {
            string[] _datasetStrings = dataFile.text.Split(lineSeparator);

            for (int i = 0; i < _datasetStrings.Length - 1; i += 100)
            {
                string[] _splitData = _datasetStrings[i].Split(fieldSeparator);
                tempPosition = ConvertData(float.Parse(_splitData[0]), float.Parse(_splitData[1]), float.Parse(_splitData[2]));

                //scan all placed points for location distance
                foundCloseObject = false;
                foreach (Vector3 item in data) {
                    if (Vector3.Distance(item, tempPosition) < 100) {
                        foundCloseObject = true;
                    }
                }

                if (!foundCloseObject) {
                    //var sphere = Instantiate(SpherePrefab, CalculationHandler.ConvertDataset(_dataset), Quaternion.identity) as GameObject;
                    //PointData.Add(CalculationHandler.ConvertDataset(_dataset));

                    data.Add(ConvertData(float.Parse(_splitData[0]), float.Parse(_splitData[1]), float.Parse(_splitData[2])));

                }
            }

            Debug.Log($"Data read and formatted with {data.Count} sets.");
            StartCoroutine(terrainGenerator.GenerateTerrain(data));

        }

        public Vector3 ConvertData(float latitude, float longitude, float height) {
            float _radius = 1737.4f + height / 1000; // 1737.4 is the lunar radius
            Vector3 _vector3 = new Vector3
            {
                x = _radius * Mathf.Cos(latitude) * Mathf.Cos(longitude),
                z = _radius * Mathf.Cos(latitude) * Mathf.Sin(longitude),
                y = _radius * Mathf.Sin(latitude)
            };

            return _vector3;
        }
    }
}