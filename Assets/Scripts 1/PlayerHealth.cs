using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float vidaMaxima = 100f; // Vida máxima del jugador
    [SerializeField] private float vidaActual; // Vida actual del jugador

    [SerializeField] private float tiempoGameOver = 2f; // Tiempo después de la animación para Game Over

    private Animator animator;
    private PlayerMovement playerMovement; // Referencia al script PlayerMovement
    private bool estaMuerto = false; // Controla si el jugador ya murió

    private void Start()
    {
        vidaActual = vidaMaxima; // Inicializa la vida actual al máximo
        animator = GetComponent<Animator>(); // Obtiene el componente Animator del jugador
        playerMovement = GetComponent<PlayerMovement>(); // Obtiene el componente PlayerMovement
    }

    public void TomarDano(float dano)
    {
        if (estaMuerto) return; // Si ya está muerto, no hacer nada

        vidaActual -= dano;

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

        if (playerMovement != null)
        {
            playerMovement.ActivarMuerte(); // Desactivar controles de movimiento
        }

        Invoke(nameof(GameOver), tiempoGameOver); // Invocar Game Over después de un tiempo
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Cargar escena inicial
    }

    public bool IsDead()
    {
        return estaMuerto; // Retorna el estado de muerte
    }

    public float GetVidaActual()
    {
        return vidaActual; // Retorna la vida actual
    }
}
