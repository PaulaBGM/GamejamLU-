using UnityEngine;

public class WineBottle : MonoBehaviour
{
    public bool IsCollected { get; private set; } = false;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public void SetCollected(bool value)
    {
        IsCollected = value;

        if (value)
        {
            _collider.enabled = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            _collider.enabled = true;
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void AttachTo(Transform parent, Vector3 localPosition)
    {
        transform.SetParent(parent);
        transform.localPosition = localPosition;
        transform.localRotation = Quaternion.identity;
    }

    public void Detach()
    {
        transform.SetParent(null);
    }
}
