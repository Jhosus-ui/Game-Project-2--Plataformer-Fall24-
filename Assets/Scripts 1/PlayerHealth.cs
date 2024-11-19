using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float vidaMaxima = 100f; // Vida m�xima del jugador
    [SerializeField] private float vidaActual; // Vida actual del jugador

    [SerializeField] private float tiempoGameOver = 2f; // Tiempo despu�s de la animaci�n para Game Over

    private Animator animator;
    private PlayerMovement playerMovement; // Referencia al script PlayerMovement
    private bool estaMuerto = false; // Controla si el jugador ya muri�

    private void Start()
    {
        vidaActual = vidaMaxima; // Inicializa la vida actual al m�ximo
        animator = GetComponent<Animator>(); // Obtiene el componente Animator del jugador
        playerMovement = GetComponent<PlayerMovement>(); // Obtiene el componente PlayerMovement
    }

    public void TomarDano(float dano)
    {
        if (estaMuerto) return; // Si ya est� muerto, no hacer nada

        vidaActual -= dano;

        // Mostrar el da�o recibido (opcional)
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

        // Llamar a PlayerMovement para desactivar el movimiento
        if (playerMovement != null)
        {
            playerMovement.ActivarMuerte();
        }

        // Invocar el Game Over despu�s de un tiempo
        Invoke(nameof(GameOver), tiempoGameOver);
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        // Aqu� puedes implementar la l�gica para reiniciar la escena, cargar una pantalla de Game Over, etc.
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Ejemplo: Cargar la escena inicial
    }

    public float GetVidaActual()
    {
        return vidaActual; // Retorna la vida actual (por ejemplo, para mostrar en una UI)
    }
}
