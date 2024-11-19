using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Velocidad de movimiento horizontal
    public float jumpForce = 10f;       // Fuerza del salto
    public float acceleration = 0.1f;   // Suaviza la aceleraci�n
    public float deceleration = 0.1f;   // Suaviza la desaceleraci�n
    public float fallMultiplier = 2.5f; // Multiplicador de gravedad para ca�da r�pida
    public float coyoteTime = 0.2f;     // Tiempo de gracia para un �nico salto cuando cae

    private const float movementThreshold = 0.1f; // Umbral para ignorar valores residuales

    private Rigidbody2D rb;             // Referencia al Rigidbody2D del personaje
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Combat combat;              // Referencia al script Combat

    private bool isGrounded = false;    // Indica si el personaje est� en el suelo
    private bool hasJumpedFromGround = false; // Indica si el personaje ha hecho un salto inicial desde el suelo
    private bool canSingleJumpOnFall = true;  // Permite un �nico salto al caer sin haber saltado
    private bool isDead = false;       // Controla si el personaje est� muerto

    private float coyoteTimeCounter;    // Temporizador de tiempo de gracia para el salto

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        combat = GetComponent<Combat>(); // Obtener referencia al script Combat
    }

    private void Update()
    {
        // Si el personaje est� muerto, detener todo
        if (isDead)
        {
            rb.velocity = Vector2.zero; // Detener cualquier movimiento
            return;
        }

        // Movimiento horizontal con suavizado de aceleraci�n/desaceleraci�n
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

        // Redondeo de las velocidades para ignorar valores peque�os
        float xVelocity = Mathf.Abs(rb.velocity.x);
        if (xVelocity < movementThreshold)
        {
            xVelocity = 0;
        }

        float yVelocity = rb.velocity.y;
        if (Mathf.Abs(yVelocity) < movementThreshold)
        {
            yVelocity = 0;
        }

        // Actualizar animaci�n
        animator.SetFloat("XVelocity", xVelocity);
        animator.SetFloat("YVelocity", yVelocity);

        // Voltear el personaje seg�n la direcci�n de movimiento
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }

        // Reducci�n del tiempo de gracia para salto (Coyote Time) cuando cae sin saltar
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            hasJumpedFromGround = false; // Resetear salto inicial desde el suelo
            canSingleJumpOnFall = true;  // Permitir un �nico salto al caer
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // L�gica de salto
        if (Input.GetButtonDown("Jump") && !isDead) // Evitar salto si est� muerto
        {
            if (isGrounded || (coyoteTimeCounter > 0f && canSingleJumpOnFall && !hasJumpedFromGround))
            {
                // Un �nico salto al caer sin haber saltado previamente
                Jump();
                canSingleJumpOnFall = false;
                animator.SetBool("isJumping", true); // Activar animaci�n de salto
            }
            else if (!isGrounded && !hasJumpedFromGround)
            {
                // Doble salto si ya salt� una vez desde el suelo
                Jump();
                hasJumpedFromGround = true; // Marcar como que ya hizo el doble salto
                animator.SetBool("isJumping", true); // Activar animaci�n de salto
            }
        }

        // Aplicar mayor gravedad si el personaje est� cayendo
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Actualizar estado de salto en Combat
        combat?.SetIsJumping(!isGrounded);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Aplicar fuerza de salto
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprobar si el personaje toca el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false); // Desactivar animaci�n de salto al tocar el suelo
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Detectar cuando el personaje deja de tocar el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // M�todo para activar la muerte del personaje
    public void ActivarMuerte()
    {
        isDead = true; // Marcar como muerto
        animator.SetTrigger("Muerte"); // Activar la animaci�n de muerte
        rb.velocity = Vector2.zero; // Detener movimiento por completo
        rb.isKinematic = true; // Desactivar la f�sica para evitar ca�das
        // Aqu� puedes a�adir efectos o transiciones
    }
}
