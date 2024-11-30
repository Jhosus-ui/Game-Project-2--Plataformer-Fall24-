using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float acceleration = 0.1f;
    public float deceleration = 0.1f;
    public float fallMultiplier = 2.5f;
    public float coyoteTime = 0.2f;

    private const float movementThreshold = 0.1f; // Umbral mínimo para considerar movimiento
    private const float fallThreshold = -0.5f;    // Velocidad Y para activar animación de caída
    private const float verticalThreshold = 0.1f; // Umbral mínimo para considerar movimiento vertical

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isGrounded = false; // Indica si el personaje está en el suelo
    private bool hasDoubleJumped = false; // Detectar si ya realizó un doble salto
    private bool isDead = false;

    private float coyoteTimeCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
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
            hasDoubleJumped = false; // Restablecer el estado de doble salto
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
                DoubleJump(); // Realizar el doble salto
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
        animator.SetTrigger("Jump"); // Animación de salto normal
    }

    private void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("DoubleJump"); // Animación de doble salto
        hasDoubleJumped = true; // Marcar que el doble salto ya se usó
    }

    private void UpdateAnimations()
    {
        // Movimiento horizontal
        float xVelocity = Mathf.Abs(rb.velocity.x);

        // Filtrar valores pequeños para evitar fluctuaciones en XVelocity
        if (xVelocity < movementThreshold)
        {
            xVelocity = 0f; // Redondear a 0 si la velocidad es demasiado baja
        }

        animator.SetFloat("XVelocity", xVelocity);

        // Movimiento vertical
        float yVelocity = rb.velocity.y;

        // Filtrar valores pequeños para evitar fluctuaciones en YVelocity
        if (Mathf.Abs(yVelocity) < verticalThreshold)
        {
            yVelocity = 0f; // Redondear a 0 si la velocidad es demasiado baja
        }

        animator.SetFloat("YVelocity", yVelocity);

        // Detectar si está cayendo
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isFalling", false);

            // Forzar la transición al estado "Movement" si es necesario
            animator.Play("Movement");

            Debug.Log("Personaje tocó el suelo. isGrounded: " + isGrounded);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);

            Debug.Log("Personaje dejó de tocar el suelo. isGrounded: " + isGrounded);
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
