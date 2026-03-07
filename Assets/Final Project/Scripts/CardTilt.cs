using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CardTilt : MonoBehaviour
{
    [Header("Tilt Settings")]
    [SerializeField] private float _maxTiltAngle = 12f;
    [SerializeField] private float _rotationSmoothSpeed = 10f;

    [Header("Optional")]
    [SerializeField] private Camera _targetCamera;

    private Quaternion _initialLocalRotation;
    private Quaternion _targetLocalRotation;
    private Collider _cardCollider;

    private void Awake()
    {
        _cardCollider = GetComponent<Collider>();
        _initialLocalRotation = transform.localRotation;

        if (_targetCamera == null)
        {
            _targetCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (_targetCamera == null)
        {
            return;
        }

        UpdateTargetRotation();

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            _targetLocalRotation,
            Time.deltaTime * _rotationSmoothSpeed
        );
    }

    private void UpdateTargetRotation()
    {
        Ray ray = _targetCamera.ScreenPointToRay(Input.mousePosition);

        if (_cardCollider.Raycast(ray, out RaycastHit hit, 1000f))
        {
            Vector3 localHitPoint = transform.InverseTransformPoint(hit.point);

            Bounds localBounds = GetLocalBounds(_cardCollider);

            float normalizedX = 0f;
            float normalizedY = 0f;

            if (localBounds.extents.x > 0.0001f)
            {
                normalizedX = Mathf.Clamp(localHitPoint.x / localBounds.extents.x, -1f, 1f);
            }

            if (localBounds.extents.y > 0.0001f)
            {
                normalizedY = Mathf.Clamp(localHitPoint.y / localBounds.extents.y, -1f, 1f);
            }

            float tiltAroundX = -normalizedY * _maxTiltAngle;
            float tiltAroundY = normalizedX * _maxTiltAngle;

            Quaternion tiltRotation = Quaternion.Euler(tiltAroundX, tiltAroundY, 0f);
            _targetLocalRotation = _initialLocalRotation * tiltRotation;
        }
        else
        {
            _targetLocalRotation = _initialLocalRotation;
        }
    }

    private Bounds GetLocalBounds(Collider col)
    {
        if (col is BoxCollider box)
        {
            return new Bounds(box.center, box.size);
        }

        if (col is MeshCollider meshCollider && meshCollider.sharedMesh != null)
        {
            return meshCollider.sharedMesh.bounds;
        }

        if (col is SphereCollider sphere)
        {
            Vector3 size = Vector3.one * sphere.radius * 2f;
            return new Bounds(sphere.center, size);
        }

        if (col is CapsuleCollider capsule)
        {
            Vector3 size = Vector3.zero;

            switch (capsule.direction)
            {
                case 0:
                    size = new Vector3(capsule.height, capsule.radius * 2f, capsule.radius * 2f);
                    break;
                case 1:
                    size = new Vector3(capsule.radius * 2f, capsule.height, capsule.radius * 2f);
                    break;
                case 2:
                    size = new Vector3(capsule.radius * 2f, capsule.radius * 2f, capsule.height);
                    break;
            }

            return new Bounds(capsule.center, size);
        }

        return new Bounds(Vector3.zero, Vector3.one);
    }
}