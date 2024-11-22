using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("Level Settings")]
    public int currentLevelIndex; // �ndice del nivel actual
    public int nextLevelIndex; // �ndice del siguiente nivel

    [Header("Completion Settings")]
    public bool requiresItemToComplete = false; // Si necesita un �tem para completarse
    public string requiredItemTag = "Item"; // Etiqueta del �tem necesario (si aplica)

    private bool levelCompleted = false; // Evitar m�ltiples ejecuciones

    // M�todo para completar el nivel
    public void CompleteLevel()
    {
        if (levelCompleted) return; // Evitar m�ltiples llamadas

        levelCompleted = true;

        // Desbloquear el siguiente nivel
        LevelManager.UnlockLevel(nextLevelIndex);

        // Cambiar a la pantalla de selecci�n de niveles o cargar la pr�xima escena
        SceneManager.LoadScene("LevelMenu"); // Cambia a tu men� de niveles
    }

    // Detectar si el jugador recoge el �tem necesario (si aplica)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (requiresItemToComplete && collision.CompareTag(requiredItemTag))
        {
            CompleteLevel();
        }
    }

    // M�todo para forzar la finalizaci�n (por ejemplo, al llegar a una meta)
    public void ForceComplete()
    {
        CompleteLevel();
    }
}
