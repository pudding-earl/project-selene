using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSelene {
    public class DataReader : MonoBehaviour {
        [SerializeField] private TextAsset dataFile; // Reference to the CSV file being read

        private List<Dataset> data = new List<Dataset>(); // The data from the file after it has been converted

        private char lineSeparator = '\n';
        private char fieldSeparator = ',';

        private void Start() {
            ReadData();
        }

        private void ReadData() {
            Debug.Log("Reading and parsing data...");
            string[] _datasetStrings = dataFile.text.Split(lineSeparator);
            Debug.Log("Data read and parsed successfully.");
            
            Debug.Log("Converting data...");
            foreach(string _set in _datasetStrings) {
                string[] _splitData = _set.Split(',');
                
                print(_splitData);
                
                Dataset _dataset = new Dataset();
                _dataset.latitude = Convert.ToDouble(_splitData[0]);
                _dataset.longitude = Convert.ToDouble(_splitData[1]);
                _dataset.height = Convert.ToDouble(_splitData[2]);
                _dataset.slope = Convert.ToDouble(_splitData[3]);
                
                data.Add(_dataset);
            }

            Debug.Log($"Data converted successfully with {data.Count} sets.");

        }
    }

    public struct Dataset {
        public double latitude;
        public double longitude;
        public double height;
        public double slope;
    }
}