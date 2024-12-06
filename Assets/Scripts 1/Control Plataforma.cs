using System.Collections;
using UnityEngine;

public class ControlPlataforma : MonoBehaviour
{
    PlatformEffector2D pE2D;
    private Animator playerAnimator; // Referencia al Animator del jugador
    private bool playerOnPlatform = false; // Verificar si el jugador está en la plataforma
    public bool LeftPlatform;

    void Start()
    {
        pE2D = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        // Solo permite presionar "S" si el jugador está en la plataforma
        if (Input.GetKeyDown("s") && !LeftPlatform && playerOnPlatform)
        {
            // Rotar la plataforma para permitir caer
            pE2D.rotationalOffset = 180;
            LeftPlatform = true;

            // Activar la animación de caída del jugador
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isFalling", true); // Activar "isFalling"
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto en contacto es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;

            // Obtener el Animator del jugador
            playerAnimator = collision.gameObject.GetComponent<Animator>();

            if (playerAnimator != null)
            {
                Debug.Log("Jugador tocó la plataforma, Animator asignado.");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Restablece el estado cuando el jugador sale de la plataforma
        if (collision.gameObject.CompareTag("Player"))
        {
            pE2D.rotationalOffset = 0;
            LeftPlatform = false;
            playerOnPlatform = false;

            // Desactivar la animación de caída al salir de la plataforma
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isFalling", false); // Desactivar "isFalling"
            }
        }
    }
}
