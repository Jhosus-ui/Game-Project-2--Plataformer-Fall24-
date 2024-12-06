using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float acceleration = 0.1f;
    public float deceleration = 0.1f;
    public float fallMultiplier = 2.5f;
    public float coyoteTime = 0.2f;

    private const float movementThreshold = 0.1f;
    private const float fallThreshold = -0.5f;
    private const float verticalThreshold = 0.1f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isGrounded = false;
    private bool hasDoubleJumped = false;
    private bool isDead = false;
    private bool isControlsActive = true; // Nuevo: Controla si los controles están activos

    private float coyoteTimeCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead || !isControlsActive) return; // Desactivar controles si está muerto o deshabilitado

        HandleMovement();
        HandleJump();
        HandleFall();
        UpdateAnimations();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        float targetSpeed = moveInput * moveSpeed;

        if (moveInput != 0)
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, targetSpeed, acceleration), rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, deceleration), rb.velocity.y);
        }

        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            hasDoubleJumped = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || coyoteTimeCounter > 0f)
            {
                Jump();
            }
            else if (!isGrounded && !hasDoubleJumped)
            {
                DoubleJump();
            }
        }
    }

    private void HandleFall()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("isJumping");
    }

    private void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("DoubleJump");
        hasDoubleJumped = true;
    }

    private void UpdateAnimations()
    {
        float xVelocity = Mathf.Abs(rb.velocity.x);

        if (xVelocity < movementThreshold)
        {
            xVelocity = 0f;
        }

        animator.SetFloat("XVelocity", xVelocity);

        float yVelocity = rb.velocity.y;

        if (Mathf.Abs(yVelocity) < verticalThreshold)
        {
            yVelocity = 0f;
        }

        animator.SetFloat("YVelocity", yVelocity);

        if (!animator.GetBool("isFalling"))
        {
            if (yVelocity < fallThreshold && !isGrounded)
            {
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
            }
            else if (isGrounded)
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", false);
            }
            else if (yVelocity > 0 && !isGrounded)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isFalling", false);
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    animator.SetBool("isGrounded", true);
                    animator.SetBool("isFalling", false);
                    return;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }

    public void ActivarMuerte()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        rb.simulated = false;
        animator.SetTrigger("Muerte");
    }

    // Nuevo: Métodos para activar y desactivar controles
    public void DesactivarControles()
    {
        isControlsActive = false;
        rb.velocity = Vector2.zero; // Opcional: detener al jugador al desactivar controles
    }

    public void ActivarControles()
    {
        isControlsActive = true;
    }
}
