using UnityEngine;

namespace SplineMesh {
    [RequireComponent(typeof(Spline))]
    public class FollowSpline : MonoBehaviour {
        private GameObject generated;
        private Spline spline;
        public float Rate { get; set; }

        public GameObject Follower;
        public float DurationInSecond;

        private void Start()
        {
            spline = GetComponent<Spline>();
        }

        private void StartFollowing()
        {
            generated = Follower;
            generated.transform.position = transform.position;
            generated.transform.parent = gameObject.transform;
        }

        private void Update() {
            FromEndToStart();
        }

        public void FromStartToEnd()
        {
            Rate += Time.deltaTime / DurationInSecond;
            if (Rate < spline.nodes.Count)
                PlaceFollower();
        }

        public void FromEndToStart()
        {
            Rate -= Time.deltaTime / DurationInSecond;
            if (Rate >= 0)
                PlaceFollower();
        }

        private void PlaceFollower() {
            if (generated != null) {
                CurveSample sample = spline.GetSample(Rate);
                generated.transform.localPosition = sample.location;
            }
        }
    }
}
