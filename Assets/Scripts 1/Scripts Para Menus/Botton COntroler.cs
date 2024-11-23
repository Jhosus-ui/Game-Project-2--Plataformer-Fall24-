using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    // Función para cargar una escena específica
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Función para salir del juego
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited."); // Solo se ve en el editor.
    }


}
