using UnityEngine;

namespace Example_1___Primitives
{
    public class LineCode : MonoBehaviour
    {
        [SerializeField] private Transform origin;
        [SerializeField] private Transform destination;
        [SerializeField] private Material material;

        private LineRenderer _lineRenderer;

        private void Start()
        {
            if (!TryGetComponent(out _lineRenderer))
            {
                _lineRenderer = gameObject.AddComponent<LineRenderer>();
            }

            _lineRenderer.materials[0] = material;
            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
        }

        private void Update()
        {
            _lineRenderer.SetPosition(0, origin.position);
            _lineRenderer.SetPosition(1, destination.position);
        }
    }
}