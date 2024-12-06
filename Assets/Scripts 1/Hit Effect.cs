using UnityEngine;
using UnityEngine.UI;

public class HitEffect : MonoBehaviour
{
    [Header("Configuración de la Imagen")]
    [SerializeField] private Image golpeImagen; // Imagen que aparecerá al recibir daño
    [SerializeField] private float duracionEfecto = 0.2f; // Duración en segundos que la imagen será visible

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
            Invoke(nameof(OcultarEfectoGolpe), duracionEfecto); // Ocultar después de un tiempo
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
