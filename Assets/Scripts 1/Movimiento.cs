using System;
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

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Combat combat;

    private bool isGrounded = false;
    private bool hasJumpedFromGround = false;
    private bool canSingleJumpOnFall = true;
    private bool isDead = false;

    private float coyoteTimeCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        combat = GetComponent<Combat>();
    }

    private void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleJump();
        HandleFall();

        combat?.SetIsJumping(!isGrounded);
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

        float xVelocity = Mathf.Abs(rb.velocity.x);
        if (xVelocity < movementThreshold) xVelocity = 0;

        animator.SetFloat("XVelocity", xVelocity);

        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            hasJumpedFromGround = false;
            canSingleJumpOnFall = true;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (coyoteTimeCounter > 0f && canSingleJumpOnFall && !hasJumpedFromGround))
            {
                Jump();
                canSingleJumpOnFall = false;
                animator.SetBool("isJumping", true);
            }
            else if (!isGrounded && !hasJumpedFromGround)
            {
                Jump();
                hasJumpedFromGround = true;
                animator.SetBool("isJumping", true);
            }
        }
    }

    private void HandleFall()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        float yVelocity = rb.velocity.y;
        if (Mathf.Abs(yVelocity) < movementThreshold) yVelocity = 0;

        animator.SetFloat("YVelocity", yVelocity);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
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
