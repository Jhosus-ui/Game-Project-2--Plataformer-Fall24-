using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float vidaMaxima = 100f; // Vida máxima del jugador
    [SerializeField] private float vidaActual; // Vida actual del jugador
    [SerializeField] private float tiempoGameOver = 2f; // Tiempo después de la animación para Game Over
    [SerializeField] private float intervaloRegeneracion = 2f; // Tiempo en segundos para regenerar vida
    [SerializeField] private float cantidadRegeneracion = 2f; // Cantidad de vida que se regenera
    [SerializeField] private AudioClip sonidoDano; // Sonido que se reproduce al recibir daño
    [SerializeField] private AudioClip sonidoMuerte; // Sonido que se reproduce al morir

    private Animator animator;
    private PlayerMovement playerMovement; // Referencia al script PlayerMovement
    private AudioSource audioSource; // Fuente de audio para reproducir sonidos
    private bool estaMuerto = false; // Controla si el jugador ya murió
    private bool esInvulnerable = false; // Controla si el jugador es invulnerable

    private void Start()
    {
        vidaActual = vidaMaxima; // Inicializa la vida actual al máximo
        animator = GetComponent<Animator>(); // Obtiene el componente Animator del jugador
        playerMovement = GetComponent<PlayerMovement>(); // Obtiene el componente PlayerMovement
        audioSource = GetComponent<AudioSource>(); // Obtiene el componente AudioSource

        if (audioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en el jugador. Agrega uno para reproducir sonidos.");
        }

        // Inicia la regeneración de vida en un intervalo
        InvokeRepeating(nameof(RegenerarVida), intervaloRegeneracion, intervaloRegeneracion);
    }

    public void TomarDano(float dano)
    {
        if (estaMuerto || esInvulnerable) return; // Si ya está muerto o es invulnerable, no hacer nada

        vidaActual -= dano;

        // Reproducir sonido de daño sin retrasos
        if (sonidoDano != null && audioSource != null)
        {
            audioSource.clip = sonidoDano;
            audioSource.Play(); // Reproducir inmediatamente
        }

        Debug.Log("Jugador recibió daño. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            vidaActual = 0; // Asegurarse de que no sea negativa
            Muerte();
        }
    }

    private void Muerte()
    {
        estaMuerto = true;
        animator.SetTrigger("Muerte"); // Activar la animación de muerte

        // Reproducir sonido de muerte
        if (sonidoMuerte != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoMuerte);
        }

        if (playerMovement != null)
        {
            playerMovement.ActivarMuerte(); // Desactivar controles de movimiento
        }

        FadingObject fadingObject = FindObjectOfType<FadingObject>();
        if (fadingObject != null)
        {
            fadingObject.IniciarFade();
            fadingObject.DetenerMusica(); // Detener la música de ambientación
        }

        if (playerMovement != null)
        {
            playerMovement.ActivarMuerte(); // Desactivar controles de movimiento
        }

        Combat combat = GetComponent<Combat>();
        if (combat != null)
        {
            combat.DesactivarCombate();
        }

        if (playerMovement != null)
        {
            playerMovement.ActivarMuerte(); // Desactivar controles de movimiento
        }

        Invoke(nameof(GameOver), tiempoGameOver); // Invocar Game Over después de un tiempo
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // Cargar escena inicial
    }

    private void RegenerarVida()
    {
        // Solo regenera si el jugador no está muerto y la vida no está al máximo
        if (!estaMuerto && vidaActual < vidaMaxima)
        {
            vidaActual += cantidadRegeneracion;
            if (vidaActual > vidaMaxima)
            {
                vidaActual = vidaMaxima; // Asegurarse de no exceder la vida máxima
            }

            Debug.Log("Vida regenerada. Vida actual: " + vidaActual);
        }
    }

    public void ActivarInvulnerabilidad()
    {
        esInvulnerable = true; // Activa la invulnerabilidad
        Debug.Log("Invulnerabilidad activada.");
    }

    public void DesactivarInvulnerabilidad()
    {
        esInvulnerable = false; // Desactiva la invulnerabilidad
        Debug.Log("Invulnerabilidad desactivada.");
    }

    public bool IsDead()
    {
        return estaMuerto; // Retorna el estado de muerte
    }

    public float GetVidaActual()
    {
        return vidaActual; // Retorna la vida actual
    }

    public float GetVidaMaxima()
    {
        return vidaMaxima;
    }
}
