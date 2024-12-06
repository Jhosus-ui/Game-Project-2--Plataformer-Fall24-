using UnityEngine;

public class HealingItem : MonoBehaviour
{
    [Header("Healing Settings")]
    public float healingAmount = 20f; // Cantidad de vida que cura al jugador.

    [Header("Movement Settings")]
    public float movementSpeed = 1f; // Velocidad del movimiento vertical.
    public float movementRange = 0.5f; // Rango del movimiento (de arriba hacia abajo).

    [Header("Sound Settings")]
    public AudioClip pickupSound; // Sonido al recoger el ítem.

    private AudioSource audioSource; // Fuente de audio para reproducir el sonido.
    private Vector3 startPosition; // Posición inicial del ítem.

    private void Start()
    {
        // Guardar la posición inicial del ítem
        startPosition = transform.position;

        // Agregar un componente AudioSource si no existe
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Evitar que el sonido se reproduzca al inicio
    }

    private void Update()
    {
        // Movimiento de arriba hacia abajo usando Mathf.Sin para hacer el efecto suave
        float newY = startPosition.y + Mathf.Sin(Time.time * movementSpeed) * movementRange;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si el objeto que colisiona es el jugador
        if (collision.CompareTag("Player"))
        {
            // Curar al jugador si tiene un script de vida
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Curar(healingAmount); // Curar al jugador
                Debug.Log("Jugador curado por: " + healingAmount);
            }

            // Reproducir el sonido de recogida
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Destruir el ítem después de curar
            Destroy(gameObject);
        }
    }
}
