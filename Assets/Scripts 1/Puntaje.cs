using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puntaje : MonoBehaviour
{
    private float puntos; // Puntaje actual
    private TextMeshProUGUI textMesh; // Referencia al componente UI de texto

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>(); // Obt�n el componente de texto
        ActualizarTexto(); // Inicializa el texto en pantalla
    }

    // Este m�todo se llama cuando se recolecta un �tem
    public void SumarPuntos(float puntosEntrada)
    {
        puntos += puntosEntrada; // Incrementa el puntaje
        ActualizarTexto(); // Actualiza el texto de la UI
    }

    // M�todo para actualizar el texto en pantalla
    private void ActualizarTexto()
    {
        textMesh.text = puntos.ToString("0"); // Muestra el puntaje sin decimales
    }
}
