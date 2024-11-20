using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    [SerializeField] private Slider slider;  // Referencia al slider de la UI
    [SerializeField] private PlayerHealth playerHealth;  // Referencia al script PlayerHealth

    private void Start()
    {
        // Verifica que el slider est� configurado
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        // Obt�n la referencia al script PlayerHealth autom�ticamente si no se configur�
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }

        // Inicializa la barra de vida con la vida m�xima del jugador
        if (playerHealth != null)
        {
            InicializarBarraDeVida(playerHealth.GetVidaMaxima());
        }
    }

    private void Update()
    {
        // Si hay una referencia v�lida a PlayerHealth, actualiza la barra en cada frame
        if (playerHealth != null)
        {
            CambiarVidaActual(playerHealth.GetVidaActual());
        }
    }

    /// <summary>
    /// Configura la vida m�xima en el slider
    /// </summary>
    public void CambiarVidaMaxima(float vidaMaxima)
    {
        slider.maxValue = vidaMaxima;
    }

    /// <summary>
    /// Cambia la vida actual en el slider
    /// </summary>
    public void CambiarVidaActual(float vidaActual)
    {
        slider.value = vidaActual;
    }

    /// <summary>
    /// Inicializa la barra con la vida m�xima y actual
    /// </summary>
    public void InicializarBarraDeVida(float cantidadVida)
    {
        CambiarVidaMaxima(cantidadVida);
        CambiarVidaActual(cantidadVida);
    }
}
