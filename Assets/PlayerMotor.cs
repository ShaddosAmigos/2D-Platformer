using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMotor : MonoBehaviour
{
    Vector2 direction;
    private Rigidbody2D rb; // Skrócona nazwa dla wygody

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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Jeœli dashujemy, wychodzimy z FixedUpdate, ¿eby nie nak³adaæ si³ ruchu
        if (isDashing) return;

        // Logika zapamiêtywania ostatniego kierunku (do dasha w miejscu)
        if (direction.x != 0) lastDirectionX = Mathf.Sign(direction.x);

        // Standardowe poruszanie
        rb.AddForce(new Vector2(direction.x * speed, 0));

        // Clamp prêdkoœci
        rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -maxSpeed, maxSpeed);

        // Hamowanie
        if (direction.x == 0 && rb.linearVelocityX != 0)
        {
            rb.AddForce(new Vector2(-rb.linearVelocityX * stoppingForce, 0));
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
            rb.linearVelocityY = 0; // Opcjonalne: reset prêdkoœci Y dla lepszego feelingu double jumpa
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

        // Zapamiêtujemy oryginaln¹ grawitacjê, by j¹ przywróciæ
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Wybieramy kierunek
        float dashDir = direction.x != 0 ? Mathf.Sign(direction.x) : lastDirectionX;

        // Zamiast AddForce, ustawiamy prêdkoœæ bezpoœrednio, by mieæ pe³n¹ kontrolê
        rb.linearVelocity = new Vector2(dashDir * dashForce, 0f);

        // Czekamy a¿ dash siê skoñczy
        yield return new WaitForSeconds(dashDuration);

        // Przywracamy stan sprzed dasha
        rb.gravityScale = originalGravity;
        isDashing = false;

        // Cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Prosta logika resetu skoku na kolizji
        jumpCount = 0;
    }
}