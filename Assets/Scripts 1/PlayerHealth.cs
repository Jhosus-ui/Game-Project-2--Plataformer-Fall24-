using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float vidaMaxima = 100f; // Vida m�xima del jugador
    [SerializeField] private float vidaActual; // Vida actual del jugador
    [SerializeField] private float tiempoGameOver = 2f; // Tiempo despu�s de la animaci�n para Game Over
    [SerializeField] private float intervaloRegeneracion = 2f; // Tiempo en segundos para regenerar vida
    [SerializeField] private float cantidadRegeneracion = 2f; // Cantidad de vida que se regenera
    [SerializeField] private AudioClip sonidoDano; // Sonido que se reproduce al recibir da�o
    [SerializeField] private AudioClip sonidoMuerte; // Sonido que se reproduce al morir

    private Animator animator;
    private PlayerMovement playerMovement; // Referencia al script PlayerMovement
    private AudioSource audioSource; // Fuente de audio para reproducir sonidos
    private bool estaMuerto = false; // Controla si el jugador ya muri�
    private bool esInvulnerable = false; // Controla si el jugador es invulnerable

    private void Start()
    {
        vidaActual = vidaMaxima; // Inicializa la vida actual al m�ximo
        animator = GetComponent<Animator>(); // Obtiene el componente Animator del jugador
        playerMovement = GetComponent<PlayerMovement>(); // Obtiene el componente PlayerMovement
        audioSource = GetComponent<AudioSource>(); // Obtiene el componente AudioSource

        if (audioSource == null)
        {
            Debug.LogError("No se encontr� un AudioSource en el jugador. Agrega uno para reproducir sonidos.");
        }

        // Inicia la regeneraci�n de vida en un intervalo
        InvokeRepeating(nameof(RegenerarVida), intervaloRegeneracion, intervaloRegeneracion);
    }

    public void TomarDano(float dano)
    {
        if (estaMuerto || esInvulnerable) return; // Si ya est� muerto o es invulnerable, no hacer nada

        vidaActual -= dano;

        // Reproducir sonido de da�o sin retrasos
        if (sonidoDano != null && audioSource != null)
        {
            audioSource.clip = sonidoDano;
            audioSource.Play(); // Reproducir inmediatamente
        }

        Debug.Log("Jugador recibi� da�o. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            vidaActual = 0; // Asegurarse de que no sea negativa
            Muerte();
        }
    }

    private void Muerte()
    {
        estaMuerto = true;
        animator.SetTrigger("Muerte"); // Activar la animaci�n de muerte

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
            fadingObject.DetenerMusica(); // Detener la m�sica de ambientaci�n
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

        Invoke(nameof(GameOver), tiempoGameOver); // Invocar Game Over despu�s de un tiempo
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // Cargar escena inicial
    }

    private void RegenerarVida()
    {
        // Solo regenera si el jugador no est� muerto y la vida no est� al m�ximo
        if (!estaMuerto && vidaActual < vidaMaxima)
        {
            vidaActual += cantidadRegeneracion;
            if (vidaActual > vidaMaxima)
            {
                vidaActual = vidaMaxima; // Asegurarse de no exceder la vida m�xima
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
