using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxSpeed = 0.5f;
    [SerializeField] private bool infiniteHorizontal = true;

    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;
    private float _spriteWidth;
    private SpriteRenderer _mainRenderer;
    private SpriteRenderer[] _clones;
    
    private Vector3 _delta;
    private Vector3 _parallaxMove;
    private float _cameraDelta;
    private float _shiftAmount;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position;
        _mainRenderer = GetComponent<SpriteRenderer>();
        
        _spriteWidth = _mainRenderer.bounds.size.x;
        
        _clones  = new SpriteRenderer[2];
        _clones[0] = CreateClone(_spriteWidth);
        _clones[1] = CreateClone(-_spriteWidth);
    }

    private SpriteRenderer CreateClone(float offsetX)
    {
        var clone = new GameObject($"{name}_clone");
        var newRenderer = clone.AddComponent<SpriteRenderer>();
        newRenderer.sprite = _mainRenderer.sprite;
        newRenderer.sortingOrder = _mainRenderer.sortingOrder;
        
        clone.transform.SetParent(transform.parent);
        clone.transform.localScale = transform.localScale;
        clone.transform.position = transform.position + Vector3.right * offsetX;
        
        return newRenderer;
    }

    private void LateUpdate()
    {
        _delta = _cameraTransform.position - _lastCameraPosition;
        _parallaxMove = new Vector3(_delta.x * parallaxSpeed, _delta.y * parallaxSpeed);
        
        transform.position += _parallaxMove;
        foreach (var clone in _clones) clone.transform.position += _parallaxMove;
        
        _lastCameraPosition = _cameraTransform.position;

        if (!infiniteHorizontal) return;
        _cameraDelta = _cameraTransform.position.x - transform.position.x;
        if (Mathf.Abs(_cameraDelta) < _spriteWidth * 0.5f) return;
        _shiftAmount = Mathf.Sign(_cameraDelta) * _spriteWidth;
        transform.position += Vector3.right * _shiftAmount;
        foreach (var clone in _clones) clone.transform.position += Vector3.right * _shiftAmount;
    }
}