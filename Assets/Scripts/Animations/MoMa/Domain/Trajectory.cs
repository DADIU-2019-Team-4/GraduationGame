using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MoMa
{
    [System.Serializable]
    public class Trajectory
    {
        public List<Point> points = new List<Point>();

        public Snippet GetLocalSnippet(
            int presentFrame)
        {
            // Validate input
            if (presentFrame < SalamanderController.FeaturePastPoints - 1 || presentFrame > points.Count - SalamanderController.FeaturePoints)
            {
                Debug.LogError("Attempt to create a Snippet the exceedes the past or the future limit");
                throw new Exception("Attempt to create a Snippet the exceedes the past or the future limit");
            }

            // Find present position and rotation
            Vector2S presentPosition = points[presentFrame].position;
            QuaternionS presentRotation = this.points[presentFrame].rotation;

            // Build the new Snippet
            Snippet snippet = new Snippet();

            for (int i = 0; i < SalamanderController.SnippetSize; i++)
            {
                // Compute the position of the points relative to the present position and rotation
                // Create a Point at the current position
                int addingFrame = presentFrame - SalamanderController.FeaturePastPoints + 1 + i;
                Vector3 localPosition3D = new Vector3(this.points[addingFrame].position.x, 0, this.points[addingFrame].position.y);

                // Move it to the root
                localPosition3D.x -= presentPosition.x;
                localPosition3D.z -= presentPosition.y;

                // Rotate it to face upwards
                localPosition3D = Quaternion.Inverse((Quaternion) presentRotation) * localPosition3D;

                // Compute the new rotation
                QuaternionS localRotation = this.points[addingFrame].rotation * presentRotation;

                // Store the relative point to the snippet
                snippet.points[i] = new Point(localPosition3D.GetXZVector2(), localRotation);
            }

            return snippet;
        }

        public override string ToString()
        {
            string s = "Trajectory: {";

            if (this.points.Count > 0)
            {
                s += this.points[0];
            }

            foreach(Point p in this.points.GetRange(1, this.points.Count-1))
            {
                s += ", " + p;
            }

            return s + "}";
        }

        [System.Serializable]
        public class Point
        {
            public const int Decimals = 4;

            public Vector2S position;
            public QuaternionS rotation;

            public float magnitude
            {
                get { return position.magnitude; }
            }

            public static float operator -(Point a, Point b)
                // TODO include rotation?
                => (a.position - b.position).magnitude;

            public static Point getMedianPoint(List<Vector2S> positions)
            {
                Vector2S position = new Vector2S(0f, 0f);
                QuaternionS rotation;

                // Position
                foreach (Vector2S currentPosition in positions)
                {
                    position += currentPosition;
                }

                position /= positions.Count;

                // Rotation
                Vector2S displacement2D = (positions[positions.Count-1] - positions[0]);
                rotation = Quaternion.LookRotation(
                    new Vector3(displacement2D.x, 0, displacement2D.y),
                    Vector3.up);

                return new Point(position, rotation);
            }

            public Point(Vector2S v, QuaternionS rotation)
            {
                this.position = new Vector2S();
                this.position.x = (float) Math.Round(v.x, Decimals);
                this.position.y = (float) Math.Round(v.y, Decimals);
                this.rotation = rotation;
            }

            public override string ToString()
            {
                return " pos: [" + this.position.x + ", " + this.position.y + "], rot: " + this.rotation.eulerAngles;
            }
        }

        [System.Serializable]
        public class Snippet
        {
            public Point[] points = new Point[SalamanderController.SnippetSize];

            // Alternative, currently not in use
            public float CalcDiffExp(Snippet candidate)
            {
                // In case any Snippet has null Points, the difference is infinite
                float diff = 0f;
                int totalWeight = 0;

                // Diff of past Points
                for (int i = 0; i < SalamanderController.FeaturePastPoints; i++)
                {
                    int weight = 2^i;
                    totalWeight += weight;

                    diff += (this.points[i] == null) ||
                        (candidate.points[i] == null) ?
                            Mathf.Infinity :
                            (this.points[i] - candidate.points[i]) * weight;
                }

                // Diff of future Points
                for (int i = 0; i < SalamanderController.FeaturePoints; i++)
                {
                    int weight = 2^i;
                    totalWeight += weight;

                    diff +=
                        (this.points[SalamanderController.SnippetSize - 1 - i] == null) ||
                        (candidate.points[SalamanderController.SnippetSize - 1 - i] == null) ?
                            Mathf.Infinity :
                            (this.points[i] - candidate.points[i]) * weight;
                }

                return diff / totalWeight;
            }

            public float CalcDiff(Snippet candidate)
            {
                // In case any Snippet has null Points, the difference is infinite
                float diff = 0f;

                // Diff of past Points
                for (int i = 0; i < SalamanderController.SnippetSize; i++)
                {
                    diff += (this.points[i] == null) ||
                        (candidate.points[i] == null) ?
                            Mathf.Infinity :
                            (this.points[i] - candidate.points[i]);
                }

                return diff;
            }

            public override string ToString()
            {
                // Print start
                string s = "Snippet: {";

                // Print past Points
                for (int i=0; i < SalamanderController.FeaturePastPoints - 1; i++)
                {
                    s += this.points[i] + ", ";
                }

                // Print seperator
                s += this.points[SalamanderController.FeaturePastPoints - 1] + " || ";

                // Print future Points
                for (int i = SalamanderController.FeaturePastPoints; i < SalamanderController.SnippetSize - 1; i++)
                {
                    s += this.points[i] + ", ";
                }

                // Print end
                s += this.points[SalamanderController.SnippetSize - 1] + "}";

                return s;
            }

        }
    }
}
