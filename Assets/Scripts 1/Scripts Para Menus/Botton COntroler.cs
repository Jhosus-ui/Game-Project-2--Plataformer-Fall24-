using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    // Funci�n para cargar una escena espec�fica
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Funci�n para salir del juego
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited."); // Solo se ve en el editor.
    }


}
