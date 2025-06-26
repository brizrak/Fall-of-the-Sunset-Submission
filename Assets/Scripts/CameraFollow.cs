using Player;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool useBounds;
    [SerializeField] private PolygonCollider2D levelBounds;
    
    private Transform _target;

    private void Awake()
    {
        gameManager.OnPlayerSpawned += SetTarget;
    }

    private void OnDestroy()
    {
        gameManager.OnPlayerSpawned -= SetTarget;
    }

    private void SetTarget(GameObject target)
    {
        _target = target.transform;
    }
    
    private void LateUpdate()
    {
        if (!_target) return;
        
        var desiredPosition = _target.position + offset;
        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime * 60);
        
        if (useBounds)
        {
            smoothedPosition = ClampPositionToBounds(smoothedPosition);
        }
        
        smoothedPosition.z = transform.position.z;
        
        transform.position = smoothedPosition;
    }
    
    private Vector3 ClampPositionToBounds(Vector3 position)
    {
        if (levelBounds.OverlapPoint(position)) return position;
        var closestPoint = levelBounds.ClosestPoint(position);
        return new Vector3(closestPoint.x, closestPoint.y, position.z);
    }
}