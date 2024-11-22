using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("Level Settings")]
    public int currentLevelIndex; // Índice del nivel actual
    public int nextLevelIndex; // Índice del siguiente nivel

    [Header("Completion Settings")]
    public bool requiresItemToComplete = false; // Si necesita un ítem para completarse
    public string requiredItemTag = "Item"; // Etiqueta del ítem necesario (si aplica)

    private bool levelCompleted = false; // Evitar múltiples ejecuciones

    // Método para completar el nivel
    public void CompleteLevel()
    {
        if (levelCompleted) return; // Evitar múltiples llamadas

        levelCompleted = true;

        // Desbloquear el siguiente nivel
        LevelManager.UnlockLevel(nextLevelIndex);

        // Cambiar a la pantalla de selección de niveles o cargar la próxima escena
        SceneManager.LoadScene("LevelMenu"); // Cambia a tu menú de niveles
    }

    // Detectar si el jugador recoge el ítem necesario (si aplica)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (requiresItemToComplete && collision.CompareTag(requiredItemTag))
        {
            CompleteLevel();
        }
    }

    // Método para forzar la finalización (por ejemplo, al llegar a una meta)
    public void ForceComplete()
    {
        CompleteLevel();
    }
}
