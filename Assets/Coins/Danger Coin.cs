using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageAmount = 10f; // Cantidad de vida que quita al jugador.

    [Header("Sound Settings")]
    public AudioClip damageSound; // Sonido que se reproduce al da�ar al jugador.

    private AudioSource audioSource; // Fuente de audio para reproducir el sonido.

    private void Start()
    {
        // Agregar un AudioSource al objeto si no existe
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Evitar que el sonido se reproduzca autom�ticamente
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si el objeto que colisiona es el jugador
        if (collision.CompareTag("Player"))
        {
            // Reducir la vida del jugador si tiene un script de vida
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TomarDano(damageAmount); // Reducir la vida del jugador
                Debug.Log($"Jugador recibi� {damageAmount} de da�o.");
            }

            // Reproducir el sonido de da�o
            if (damageSound != null)
            {
                AudioSource.PlayClipAtPoint(damageSound, transform.position);
            }

            // Opcional: Destruir el objeto despu�s de hacer da�o
            Destroy(gameObject);
        }
    }
}
