using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TriangleNet.Geometry;

namespace ProjectSelene {
    public class TerrainGenerator : MonoBehaviour {

        [SerializeField] private Transform chunkPrefab;

        private List<Vector3> convertedData = new List<Vector3>();
        private TriangleNet.Meshing.IMesh mesh;
        private List<Vertex> vertices = new List<Vertex>();

        private List<float> elevations = new List<float>();

        /*public void OnDrawGizmos() {
            if (mesh == null) {
                return;
            }

            Gizmos.color = Color.red;
            foreach (Edge _edge in mesh.Edges) {
                Vertex _v0 = vertices[_edge.P0];
                Vertex _v1 = vertices[_edge.P1];

                Vector3 _p0 = new Vector3((float)_v0.x, 0.0f, (float)_v0.y);
                Vector3 _p1 = new Vector3((float)_v1.x, 0.0f, (float)_v1.y);
                Gizmos.DrawLine(_p0, _p1);
            }
        }*/

        public void Triangulate(List<Dataset> data) {

            Debug.Log("Raw Datasets: " + data.Count);

            foreach (Dataset _set in data) {
                Vector3 _point = CalculationHandler.ConvertDataset(_set);
                convertedData.Add(_point);
            }

            Debug.Log("Converted Datasets: " + convertedData.Count);

            Polygon _polygon = new Polygon();
            foreach (Vector3 _point in convertedData) {
                _polygon.Add(new Vertex(_point.x, _point.z));
                elevations.Add(_point.y);
            }

            Debug.Log("Elevations: " + elevations.Count);

            TriangleNet.Meshing.ConstraintOptions _options =
                new TriangleNet.Meshing.ConstraintOptions() { ConformingDelaunay = false };
            mesh = _polygon.Triangulate(_options);
            vertices = mesh.Vertices.ToList();

            Debug.Log("Vertices: " + vertices.Count);

            MakeMesh();
        }

        public void MakeMesh() {
            Debug.Log("Total Triangles: mesh.Triangles.Count");
            IEnumerator<ITriangle> triangleEnumerator = mesh.Triangles.GetEnumerator();
            for(int _chunkStart = 0; _chunkStart < mesh.Triangles.Count; _chunkStart += 13263) {
                // Unity formats
                List<Vector3> _vertices = new List<Vector3>();
                List<Vector3> _normals = new List<Vector3>();
                List<Vector2> _uvs = new List<Vector2>();
                List<int> _triangles = new List<int>();

                int _chunkEnd = _chunkStart + 13263;
                for (int i = _chunkStart; i < _chunkEnd; i++) {
                    if(!triangleEnumerator.MoveNext()) {
                        break;
                    }

                    ITriangle _triangle = triangleEnumerator.Current;

                    Vector3 v0 = GetPoint3D(_triangle.GetVertex(2).id);
                    Vector3 v1 = GetPoint3D(_triangle.GetVertex(1).id);
                    Vector3 v2 = GetPoint3D(_triangle.GetVertex(0).id);

                    _triangles.Add(vertices.Count);
                    _triangles.Add(vertices.Count + 1);
                    _triangles.Add(vertices.Count + 2);

                    _vertices.Add(v0);
                    _vertices.Add(v1);
                    _vertices.Add(v2);

                    Vector3 _normal = Vector3.Cross(v1 - v0, v2 - v0);
                    _normals.Add(_normal);
                    _normals.Add(_normal);
                    _normals.Add(_normal);

                    // Review this for texture issues
                    _uvs.Add(Vector2.zero);
                    _uvs.Add(Vector2.zero);
                    _uvs.Add(Vector2.zero);
                }

                Mesh _chunkMesh = new Mesh();
                _chunkMesh.vertices = _vertices.ToArray();
                _chunkMesh.uv = _uvs.ToArray();
                _chunkMesh.triangles = _triangles.ToArray();
                _chunkMesh.normals = _normals.ToArray();

                Transform _chunk = Instantiate<Transform>(chunkPrefab, transform.position, transform.rotation);
                _chunk.GetComponent<MeshFilter>().mesh = _chunkMesh;
                chunkPrefab.GetComponent<MeshCollider>().sharedMesh = _chunkMesh;
                _chunk.transform.parent = transform;
            }
        }

        // Returns the world-space vertex for a given vertex index
        public Vector3 GetPoint3D(int index) {
            if(index == 1630964) {
                Debug.Log("Invalid Index");
                return Vector3.zero;
            }
            Debug.Log("Vertex ID: " + index);
            Vertex vertex = vertices[index];
            float elevation = elevations[index];
            Debug.Log("Success");
            return new Vector3((float)vertex.x, elevation, (float)vertex.y);
        }

    }
}