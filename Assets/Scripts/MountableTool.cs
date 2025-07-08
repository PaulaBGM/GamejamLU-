using UnityEngine;

public abstract class MountableTool : MonoBehaviour
{
    public float speed = 6f;
    protected Transform mountPoint;
    protected Rigidbody2D rb;
    protected GameObject owner;

    public virtual void Initialize(GameObject owner, Transform mountPoint)
    {
        this.owner = owner;
        this.mountPoint = mountPoint;
        rb = owner.GetComponent<Rigidbody2D>();
        transform.SetParent(owner.transform);
        transform.localPosition = Vector3.zero;
    }

    public virtual void OnMounted() { }
    public virtual void OnDismounted() { }

    public virtual void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        if (rb != null)
            rb.linearVelocity = movement * speed;
    }
}
