using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMotor : MonoBehaviour
{
    Vector2 direction;
    private Rigidbody2D rb;
    private Animator _animator;

    private bool canDash = true;
    private bool isDashing = false; 
    private float lastDirectionX = 1;

    private int jumpCount = 0;
    public int maxJumps = 2;
    public float maxSpeed = 10;
    public float stoppingForce = 5;
    public float speed = 10;
    public float jumpForce = 10;

    public float dashForce = 20;
    public float dashDuration = 0.2f; 
    public float dashCooldown = 1f;

    private float initXScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        initXScale = transform.localScale.x;
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        if (direction.x != 0) lastDirectionX = Mathf.Sign(direction.x);

        rb.AddForce(new Vector2(direction.x * speed, 0));

        rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -maxSpeed, maxSpeed);

        // Hamowanie
        if (direction.x == 0 && rb.linearVelocityX != 0)
        {
            rb.AddForce(new Vector2(-rb.linearVelocityX * stoppingForce, 0));
        }
        if (direction.x != 0)
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(initXScale, transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-initXScale, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    private void OnJump()
    {
        if (jumpCount < maxJumps)
        {
            rb.linearVelocityY = 0; 
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    private void OnDash()
    {
        if (canDash)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float dashDir = direction.x != 0 ? Mathf.Sign(direction.x) : lastDirectionX;

        rb.linearVelocity = new Vector2(dashDir * dashForce, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;

        // Cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpCount = 0;
    }
}