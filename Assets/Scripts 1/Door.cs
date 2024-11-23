using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    [SerializeField] private int coresNecesarios = 10; // Cantidad m�nima de cores para desbloquear la puerta
    [SerializeField] private string triggerDesbloqueo = "Desbloqueado"; // Nombre del trigger en el Animator
    [SerializeField] private BoxCollider2D colliderBloqueo; // Collider que bloquea el paso
    [SerializeField] private AudioClip sonidoBloqueado; // Sonido cuando la puerta est� bloqueada
    [SerializeField] private AudioClip sonidoDesbloqueo; // Sonido cuando se desbloquea la puerta

    private Animator animator; // Referencia autom�tica al Animator
    private AudioSource audioSource; // Fuente de audio para reproducir sonidos
    private bool jugadorCerca = false; // Indica si el jugador est� cerca de la puerta

    private void Start()
    {
        // Obtener componentes autom�ticamente
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null)
        {
            Debug.LogError("No se encontr� un Animator en el objeto de la puerta. Aseg�rate de que el Animator est� configurado.");
        }

        if (audioSource == null)
        {
            Debug.LogError("No se encontr� un AudioSource en el objeto de la puerta. Aseg�rate de que est� configurado.");
        }

        if (colliderBloqueo == null)
        {
            Debug.LogError("No se asign� el BoxCollider2D de bloqueo. Asigna el collider desde el inspector.");
        }
    }

    private void Update()
    {
        // Si el jugador est� cerca y presiona la tecla "E"
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            VerificarDesbloqueo();
        }
    }

    private void VerificarDesbloqueo()
    {
        if (CoreManager.instance.ObtenerTotalCores() >= coresNecesarios)
        {
            animator.SetTrigger(triggerDesbloqueo); // Activar la animaci�n de desbloqueo

            // Reproducir sonido de desbloqueo
            if (sonidoDesbloqueo != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoDesbloqueo);
            }

            // Desactivar el collider despu�s de que termine la animaci�n
            Invoke(nameof(DesactivarColliderBloqueo), animator.GetCurrentAnimatorStateInfo(0).length);
            Debug.Log("�Puerta desbloqueada!");
        }
        else
        {
            // Reproducir sonido de puerta bloqueada
            if (sonidoBloqueado != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoBloqueado);
            }

            Debug.Log("�No tienes suficientes cores para desbloquear esta puerta!");
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
