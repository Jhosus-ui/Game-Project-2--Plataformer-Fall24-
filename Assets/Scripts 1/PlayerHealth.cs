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

        // Mostrar el daño recibido (opcional)
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

        // Llamar a PlayerMovement para desactivar el movimiento
        if (playerMovement != null)
        {
            playerMovement.ActivarMuerte();
        }

        // Invocar el Game Over después de un tiempo
        Invoke(nameof(GameOver), tiempoGameOver);
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        // Aquí puedes implementar la lógica para reiniciar la escena, cargar una pantalla de Game Over, etc.
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Ejemplo: Cargar la escena inicial
    }

    public float GetVidaActual()
    {
        return vidaActual; // Retorna la vida actual (por ejemplo, para mostrar en una UI)
    }
}
