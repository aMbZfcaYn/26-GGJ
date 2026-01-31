using InputNamespace;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 movement = InputManager.Movement;
        rb.linearVelocity = movement * moveSpeed;
    }
}
