using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    [SerializeField] private int coresNecesarios = 10; // Cantidad m�nima de cores para desbloquear la puerta
    [SerializeField] private string triggerDesbloqueo = "Desbloqueado"; // Nombre del trigger en el Animator
    [SerializeField] private BoxCollider2D colliderBloqueo; // Collider que bloquea el paso

    private Animator animator; // Referencia autom�tica al Animator
    private bool jugadorCerca = false; // Indica si el jugador est� cerca de la puerta

    private void Start()
    {
        // Intentar obtener autom�ticamente el Animator del mismo objeto
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("No se encontr� un Animator en el objeto de la puerta. Aseg�rate de que el Animator est� configurado.");
        }

        // Verificar que el colliderBloqueo est� asignado
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
        // Verificar si el jugador tiene suficientes cores
        if (CoreManager.instance.ObtenerTotalCores() >= coresNecesarios)
        {
            animator.SetTrigger(triggerDesbloqueo); // Activar la animaci�n de desbloqueo
            DesactivarColliderBloqueo(); // Desactivar el collider de bloqueo
            Debug.Log("�Puerta desbloqueada!");
        }
        else
        {
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
        // Detectar si el jugador entra en el �rea de la puerta
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Detectar si el jugador sale del �rea de la puerta
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}
