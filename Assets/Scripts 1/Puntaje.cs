using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class CoreManager : MonoBehaviour
{
    public static CoreManager instance; // Singleton para acceso global.
    private int totalCores = 0; // Cantidad total de núcleos recolectados.

    public TextMeshProUGUI coreText; // Referencia al texto UI donde se muestra la cantidad.

    private void Awake()
    {
        // Asegurar que solo haya una instancia del CoreManager.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ActualizarUI();
    }

    public void AñadirCores(int cantidad)
    {
        totalCores += cantidad; // Incrementar el total de núcleos.
        ActualizarUI(); // Actualizar la UI.
    }

    private void ActualizarUI()
    {
        if (coreText != null)
        {
            coreText.text = totalCores.ToString(); // Solo muestra el número de núcleos recolectados.
        }
    }

    public int ObtenerTotalCores()
    {
        return totalCores; // Devuelve la cantidad total (por si necesitas guardarla o mostrarla).
    }
}
