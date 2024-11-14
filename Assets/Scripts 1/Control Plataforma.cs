using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlataforma : MonoBehaviour
{
    PlatformEffector2D pE2D;
    public bool LeftPlatform;
    private bool playerOnPlatform = false; // Variable para verificar si el jugador está en la plataforma

    void Start()
    {
        pE2D = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        // Solo permite presionar "S" si el jugador está en la plataforma
        if (Input.GetKeyDown("s") && !LeftPlatform && playerOnPlatform)
        {
            pE2D.rotationalOffset = 180;
            LeftPlatform = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto en contacto es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
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
        }
    }
}
