using UnityEngine;
using UnityEngine.UI;

public class HitEffect : MonoBehaviour
{
    [Header("Configuraci�n de la Imagen")]
    [SerializeField] private Image golpeImagen; // Imagen que aparecer� al recibir da�o
    [SerializeField] private float duracionEfecto = 0.2f; // Duraci�n en segundos que la imagen ser� visible

    private void Start()
    {
        if (golpeImagen != null)
        {
            golpeImagen.enabled = false; 
        }
    }
    public void MostrarEfectoGolpe()
    {
        if (golpeImagen != null)
        {
            golpeImagen.enabled = true; // Mostrar la imagen
            Invoke(nameof(OcultarEfectoGolpe), duracionEfecto); // Ocultar despu�s de un tiempo
        }
    }

    /// <summary>
    /// Desactiva el efecto de golpe.
    /// </summary>
    private void OcultarEfectoGolpe()
    {
        if (golpeImagen != null)
        {
            golpeImagen.enabled = false; // Ocultar la imagen
        }
    }
}
