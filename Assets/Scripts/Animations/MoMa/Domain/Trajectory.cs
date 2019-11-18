using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MoMa
{
    public class Trajectory
    {
        public List<Point> points = new List<Point>();

        public Snippet GetLocalSnippet(
            int presentFrame)
        {
            // Validate input
            if (presentFrame < CharacterController.FeaturePastPoints - 1 || presentFrame > points.Count - CharacterController.FeaturePoints)
            {
                Debug.LogError("Attempt to create a Snippet the exceedes the past or the future limit");
                throw new Exception("Attempt to create a Snippet the exceedes the past or the future limit");
            }

            // Find present position and rotation
            Vector2 presentPosition = this.points[presentFrame].position;
            Quaternion presentRotation = this.points[presentFrame].rotation;

            // Build the new Snippet
            Snippet snippet = new Snippet();

            for (int i = 0; i < CharacterController.SnippetSize; i++)
            {
                // Compute the position of the points relative to the present position and rotation
                // Create a Point at the current position
                int addingFrame = presentFrame - CharacterController.FeaturePastPoints + 1 + i;
                Vector3 localPosition3D = new Vector3(this.points[addingFrame].position.x, 0, this.points[addingFrame].position.y);

                // Move it to the root
                localPosition3D.x -= presentPosition.x;
                localPosition3D.z -= presentPosition.y;

                // Rotate it to face upwards
                localPosition3D = Quaternion.Inverse(presentRotation) * localPosition3D;

                // Compute the new rotation
                Quaternion localRotation = this.points[addingFrame].rotation * presentRotation;

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

        public class Point
        {
            public const int Decimals = 4;

            public Vector2 position;
            public Quaternion rotation;

            public float magnitude
            {
                get { return position.magnitude; }
            }

            public static float operator -(Point a, Point b)
                // TODO include rotation?
                => (a.position - b.position).magnitude;

            public static Point getMedianPoint(List<(Vector2, Vector3)> transform)
            {
                Vector2 position = new Vector2(0f, 0f);
                Vector3 rotation = new Vector3(0f, 0f, 0f);

                // Accumulate
                foreach ( (Vector2 currentPosition, Vector3 currentRotation) in transform)
                {
                    position += currentPosition;
                    rotation += currentRotation;
                }

                // Divide
                position /= transform.Count;
                rotation /= transform.Count;

                return new Point(position, Quaternion.Euler(rotation));
            }

            public Point(Vector2 v, Quaternion rotation)
            {
                this.position.x = (float)Math.Round(v.x, Decimals);
                this.position.y = (float)Math.Round(v.y, Decimals);
                this.rotation = rotation;
            }

            public override string ToString()
            {
                return " pos: [" + this.position.x + ", " + this.position.y + "], rot: " + this.rotation.eulerAngles;
            }
        }

        public class Snippet
        {
            public Point[] points = new Point[CharacterController.SnippetSize];

            // Alternative, currently not in use
            public float CalcDiffExp(Snippet candidate)
            {
                // In case any Snippet has null Points, the difference is infinite
                float diff = 0f;
                int totalWeight = 0;

                // Diff of past Points
                for (int i = 0; i < CharacterController.FeaturePastPoints; i++)
                {
                    int weight = 2^i;
                    totalWeight += weight;

                    diff += (this.points[i] == null) ||
                        (candidate.points[i] == null) ?
                            Mathf.Infinity :
                            (this.points[i] - candidate.points[i]) * weight;
                }

                // Diff of future Points
                for (int i = 0; i < CharacterController.FeaturePoints; i++)
                {
                    int weight = 2^i;
                    totalWeight += weight;

                    diff +=
                        (this.points[CharacterController.SnippetSize - 1 - i] == null) ||
                        (candidate.points[CharacterController.SnippetSize - 1 - i] == null) ?
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
                for (int i = 0; i < CharacterController.SnippetSize; i++)
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
                for (int i=0; i < CharacterController.FeaturePastPoints - 1; i++)
                {
                    s += this.points[i] + ", ";
                }

                // Print seperator
                s += this.points[CharacterController.FeaturePastPoints - 1] + " || ";

                // Print future Points
                for (int i = CharacterController.FeaturePastPoints; i < CharacterController.SnippetSize - 1; i++)
                {
                    s += this.points[i] + ", ";
                }

                // Print end
                s += this.points[CharacterController.SnippetSize - 1] + "}";

                return s;
            }

        }
    }
}
