using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public void MoveInput(Vector2 moveInput) { this.moveInput = moveInput; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.linearVelocityX = moveInput.x * moveSpeed;
    }
}
