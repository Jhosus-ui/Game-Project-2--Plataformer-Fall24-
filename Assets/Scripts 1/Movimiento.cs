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

    private float coyoteTimeCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

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
    // Movimiento horizontal
    float xVelocity = Mathf.Abs(rb.velocity.x);

    if (xVelocity < movementThreshold)
    {
        xVelocity = 0f; // Redondear a 0 si la velocidad es demasiado baja
    }

    animator.SetFloat("XVelocity", xVelocity);

    // Movimiento vertical
    float yVelocity = rb.velocity.y;

    if (Mathf.Abs(yVelocity) < verticalThreshold)
    {
        yVelocity = 0f; // Redondear a 0 si la velocidad es demasiado baja
    }

    animator.SetFloat("YVelocity", yVelocity);

    // Detectar si está cayendo, pero solo si "isFalling" no fue activado por otra acción
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
        // Verificar si el jugador tocó el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isFalling", false);
        }

        // Verificar si el jugador aterrizó correctamente en la plataforma
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Verificar si el contacto es desde arriba usando la normal de colisión
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) // Contacto desde arriba
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
}
