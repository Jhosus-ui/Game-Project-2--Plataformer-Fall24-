using UnityEngine;

public class FadingObject : MonoBehaviour
{
    [SerializeField] private float velocidadFade = 1f; // Velocidad de transición de la opacidad
    [SerializeField] private AudioClip ambientacionSonido; // Sonido opcional para ambientación

    private SpriteRenderer spriteRenderer; // Para controlar la opacidad del objeto
    private AudioSource audioSource; // Fuente de audio para el sonido de ambientación
    private bool comenzarFade = false; // Controla cuándo comenzar el fade

    private void Start()
    {
        // Inicializar el SpriteRenderer y establecer opacidad inicial
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 0f; // Opacidad 0
            spriteRenderer.color = color;
        }

        // Configurar el AudioSource para el sonido de ambientación
        audioSource = GetComponent<AudioSource>();
        if (ambientacionSonido != null)
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = ambientacionSonido;
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.Play();
        }
    }

    private void Update()
    {
        // Si debe comenzar el fade, incrementar la opacidad
        if (comenzarFade && spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a += Time.deltaTime * velocidadFade; // Incrementar opacidad
            color.a = Mathf.Clamp01(color.a); // Asegurarse de que no supere 1
            spriteRenderer.color = color;
        }
    }

    public void IniciarFade()
    {
        comenzarFade = true; // Activar el fade
    }

    public void DetenerMusica()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Detener el sonido de ambientación
        }
    }
}
