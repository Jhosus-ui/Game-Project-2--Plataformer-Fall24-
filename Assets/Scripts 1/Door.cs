using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    [SerializeField] private int coresNecesarios = 10; // Cantidad mínima de cores para desbloquear la puerta
    [SerializeField] private string triggerDesbloqueo = "Desbloqueado"; // Nombre del trigger en el Animator
    [SerializeField] private BoxCollider2D colliderBloqueo; // Collider que bloquea el paso

    private Animator animator; // Referencia automática al Animator
    private bool jugadorCerca = false; // Indica si el jugador está cerca de la puerta

    private void Start()
    {
        // Intentar obtener automáticamente el Animator del mismo objeto
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("No se encontró un Animator en el objeto de la puerta. Asegúrate de que el Animator está configurado.");
        }

        // Verificar que el colliderBloqueo está asignado
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
        // Verificar si el jugador tiene suficientes cores
        if (CoreManager.instance.ObtenerTotalCores() >= coresNecesarios)
        {
            animator.SetTrigger(triggerDesbloqueo); // Activar la animación de desbloqueo
            DesactivarColliderBloqueo(); // Desactivar el collider de bloqueo
            Debug.Log("¡Puerta desbloqueada!");
        }
        else
        {
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
        // Detectar si el jugador entra en el área de la puerta
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Detectar si el jugador sale del área de la puerta
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}
