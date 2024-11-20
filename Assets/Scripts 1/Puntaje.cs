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
        textMesh = GetComponent<TextMeshProUGUI>(); // Obtén el componente de texto
        ActualizarTexto(); // Inicializa el texto en pantalla
    }

    // Este método se llama cuando se recolecta un ítem
    public void SumarPuntos(float puntosEntrada)
    {
        puntos += puntosEntrada; // Incrementa el puntaje
        ActualizarTexto(); // Actualiza el texto de la UI
    }

    // Método para actualizar el texto en pantalla
    private void ActualizarTexto()
    {
        textMesh.text = puntos.ToString("0"); // Muestra el puntaje sin decimales
    }
}
