using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectSelene {
    public class TerrainGenerator : MonoBehaviour {
        
        [SerializeField] private GameObject placeholder; // A placeholder object to represent datapoint positions
        [SerializeField] private int textureSize = 4097;
        [SerializeField] private RawImage image;

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

        public void GenerateHeightMap(List<Dataset> data) {
            ConvertedData _converted = ConvertValues(data);
            Debug.Log("Vectors calculated; Borders are as follows:");
            Debug.Log("Minimum: " + _converted.minimum);
            Debug.Log("Maximum: " + _converted.maximum);
            
            Color[] _pixels = new Color[textureSize * textureSize];
            Debug.Log("Pixels: " + _pixels.Length);
            for(int i = 0; i < _pixels.Length; i++) {
                _pixels[i] = Color.black;
            }
            int _temp = 0;
            for(int i = 0; i <= 5500000; i++) {
                Vector3 _point = _converted.points[i];
                Vector3 _rangePoint = new Vector3() {
                    x = (_point.x - _converted.minimum.x) / (_converted.maximum.x - _converted.minimum.x),
                    z = (_point.z - _converted.minimum.z) / (_converted.maximum.z - _converted.minimum.z),
                    y = (_point.y - _converted.minimum.y) / (_converted.maximum.y - _converted.minimum.y)
                };
                
                Vector2 _pixelPoint = new Vector2() {
                    x = (int)(textureSize * _rangePoint.x),
                    y = (int)(textureSize * _rangePoint.z)
                };

                int _pixelIndex = (int)(_pixelPoint.x * textureSize + _pixelPoint.y);
                _pixels[_pixelIndex] = new Color(_rangePoint.y, _rangePoint.y, _rangePoint.y, 1);
            }

            Texture2D _texture = new Texture2D(textureSize, textureSize) {
                wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Point
            };
            Debug.Log(_pixels[0]);
            _texture.SetPixels(_pixels);
            _texture.Apply();
            image.texture = _texture;
            SaveTexture(_texture, "C:/Users/puddi/Downloads/heightmap.png");
        }

        private ConvertedData ConvertValues(List<Dataset> data) {
            ConvertedData _converted;
            _converted.points = new List<Vector3>();
            _converted.minimum = Vector3.zero;
            _converted.maximum = Vector3.zero;
            
            foreach(Dataset _set in data) {
                Vector3 _point = CalculationHandler.ConvertDataset(_set);
                
                if(_point.x < _converted.minimum.x)
                    _converted.minimum.x = _point.x;
                if(_point.x > _converted.maximum.x)
                    _converted.maximum.x = _point.x;
                if(_point.y < _converted.minimum.y)
                    _converted.minimum.y = _point.y;
                if(_point.y > _converted.maximum.y)
                    _converted.maximum.y = _point.y;
                if(_point.z < _converted.minimum.z)
                    _converted.minimum.z = _point.z;
                if(_point.z > _converted.maximum.z)
                    _converted.maximum.z = _point.z;
                
                _converted.points.Add(_point);
            }

            return _converted;
        }
        
        public static void SaveTexture(Texture2D texture, string fullPath)
        {
            byte[] _bytes =texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(fullPath, _bytes);
            Debug.Log(_bytes.Length/1024  + "Kb was saved as: " + fullPath);
        }

        public struct ConvertedData {
            public List<Vector3> points;
            
            // These are used to find the edges/borders of the area that's being plotted.
            // A height value is not needed for these as it will be represented by the greyscale color, so Y is Z.
            public Vector3 minimum;
            public Vector3 maximum;
        }
    }
}