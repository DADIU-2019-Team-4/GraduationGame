using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoMa
{
    public class FollowerComponent
    {
        private const string PlayerTag = "Player"; // The Tag that the Player's GameObject has in the game
        public const float DotScale = 0.03f;
        public const float AlternativeDotScale = 0.03f;

        public Color colorPath = Color.yellow;
        public Color colorAlternativePath = Color.red;

        private Transform _model;
        private GameObject _path;
        private GameObject _altPaths;
        private GameObject[] _altPathArray = new GameObject[CharacterController.CandidateFramesSize];

        public FollowerComponent(Transform model)
        {
            this._model = model;
            this._path = new GameObject();
            this._altPaths = new GameObject();
            this._altPaths.name = "Alternative Paths";

            for (int i=0; i < CharacterController.CandidateFramesSize; i++)
            {
                _altPathArray[i] = new GameObject();
                _altPathArray[i].transform.parent = this._altPaths.transform;
            }
        }

        public void DrawPath(Trajectory.Snippet snippet)
        {
            GameObject.Destroy(_path);
            _path = new GameObject();
            _path.name = "Path";
            _path.transform.position = new Vector3(this._model.position.x, 0, this._model.position.z);
            _path.transform.rotation = Quaternion.Euler(0, this._model.rotation.eulerAngles.y, 0);

            foreach (Trajectory.Point point in snippet.points)
            {
                CreateDot(
                    new Vector3(point.position.x, 0, point.position.y),
                    DotScale,
                    _path.transform,
                    Color.red
                    );
            }
        }

        public void DrawAlternativePath(Trajectory.Snippet snippet, int offset, float weight)
        {
            GameObject.Destroy(_altPathArray[offset]);
            _altPathArray[offset] = new GameObject();
            _altPathArray[offset].name = "Path " + offset;
            _altPathArray[offset].transform.parent = _altPaths.transform;
            _altPathArray[offset].transform.position = new Vector3(this._model.position.x, 0, this._model.position.z);
            _altPathArray[offset].transform.rotation = Quaternion.Euler(0, this._model.rotation.eulerAngles.y, 0);

            foreach (Trajectory.Point point in snippet.points)
            {
                CreateDot(
                    new Vector3(point.position.x, 0, point.position.y),
                    AlternativeDotScale,
                    _altPathArray[offset].transform,
                    Color.green
                    );
            }
        }

        private GameObject CreateDot(Vector3 localPosition, float scale, Transform parent, Color color)
        {
            GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dot.transform.parent = parent;
            dot.transform.localPosition = localPosition;
            dot.transform.localScale = new Vector3(scale, scale, scale);
            MeshRenderer m = dot.GetComponent<MeshRenderer>();
            m.material.color = color;

            return dot;
        }
    }
}
