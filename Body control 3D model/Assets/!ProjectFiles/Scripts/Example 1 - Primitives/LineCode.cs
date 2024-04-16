using UnityEngine;

namespace Example_1___Primitives
{
    public class LineCode : MonoBehaviour
    {
        private Transform _origin;
        private Transform _destination;
        private LineRenderer _lineRenderer;

        public void SetUp(Transform origin, Transform destination, Material material)
        {
            _origin = origin;
            _destination = destination;

            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.materials[0] = material;
            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
        }

        public void Update()
        {
            _lineRenderer.SetPosition(0, _origin.position);
            _lineRenderer.SetPosition(1, _destination.position);
        }
    }
}