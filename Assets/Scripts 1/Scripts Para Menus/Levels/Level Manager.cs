using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Buttons")]
    public GameObject[] levelButtons; // Botones o imágenes de cada nivel.

    private void Start()
    {
        // Verificar el estado de cada nivel
        for (int i = 0; i < levelButtons.Length; i++)
        {
            // Comprobar si el nivel está desbloqueado
            if (PlayerPrefs.GetInt($"Level_{i + 1}_Unlocked", i == 0 ? 1 : 0) == 1)
            {
                levelButtons[i].SetActive(true); // Mostrar botón del nivel desbloqueado
            }
            else
            {
                levelButtons[i].SetActive(false); // Ocultar botón del nivel bloqueado
            }
        }
    }

    // Método para cargar un nivel
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene($"Level_{levelIndex}"); // Cambia "Level_" al nombre de tus escenas
    }

    // Método para desbloquear un nivel (llamar desde otro script)
    public static void UnlockLevel(int levelIndex)
    {
        PlayerPrefs.SetInt($"Level_{levelIndex}_Unlocked", 1);
        PlayerPrefs.Save();
    }

    // Método para reiniciar el progreso (opcional para pruebas)
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
