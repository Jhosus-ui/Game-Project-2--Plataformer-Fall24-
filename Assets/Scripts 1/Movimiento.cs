using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Velocidad de movimiento horizontal
    public float jumpForce = 10f;       // Fuerza del salto
    private Rigidbody2D rb;             // Referencia al Rigidbody2D del personaje
    private bool isGrounded = false;    // Indica si el personaje está en el suelo
    private bool canDoubleJump = false; // Indica si el personaje puede hacer un doble salto
 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movimiento horizontal
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        


        // Saltar cuando el personaje está en el suelo o puede hacer doble salto
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
                canDoubleJump = true; // Permitir el doble salto después del primer salto
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false; // Desactivar el doble salto después de usarlo
            }
        }
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
}
