using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    [SerializeField] private int coresNecesarios = 10; // Cantidad mínima de cores para desbloquear la puerta
    [SerializeField] private string triggerDesbloqueo = "Desbloqueado"; // Nombre del trigger en el Animator
    [SerializeField] private BoxCollider2D colliderBloqueo; // Collider que bloquea el paso
    [SerializeField] private AudioClip sonidoBloqueado; // Sonido cuando la puerta está bloqueada
    [SerializeField] private AudioClip sonidoDesbloqueo; // Sonido cuando se desbloquea la puerta

    private Animator animator; // Referencia automática al Animator
    private AudioSource audioSource; // Fuente de audio para reproducir sonidos
    private bool jugadorCerca = false; // Indica si el jugador está cerca de la puerta

    private void Start()
    {
        // Obtener componentes automáticamente
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null)
        {
            Debug.LogError("No se encontró un Animator en el objeto de la puerta. Asegúrate de que el Animator está configurado.");
        }

        if (audioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en el objeto de la puerta. Asegúrate de que está configurado.");
        }

        if (colliderBloqueo == null)
        {
            Debug.LogError("No se asignó el BoxCollider2D de bloqueo. Asigna el collider desde el inspector.");
        }
    }

    private void Update()
    {
        // Si el jugador está cerca y presiona la tecla "E"
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            VerificarDesbloqueo();
        }
    }

    private void VerificarDesbloqueo()
    {
        if (CoreManager.instance.ObtenerTotalCores() >= coresNecesarios)
        {
            animator.SetTrigger(triggerDesbloqueo); // Activar la animación de desbloqueo

            // Reproducir sonido de desbloqueo
            if (sonidoDesbloqueo != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoDesbloqueo);
            }

            // Desactivar el collider después de que termine la animación
            Invoke(nameof(DesactivarColliderBloqueo), animator.GetCurrentAnimatorStateInfo(0).length);
            Debug.Log("¡Puerta desbloqueada!");
        }
        else
        {
            // Reproducir sonido de puerta bloqueada
            if (sonidoBloqueado != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoBloqueado);
            }

            Debug.Log("¡No tienes suficientes cores para desbloquear esta puerta!");
        }
    }

    private void DesactivarColliderBloqueo()
    {
        if (colliderBloqueo != null)
        {
            colliderBloqueo.enabled = false; // Desactivar el collider
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}
