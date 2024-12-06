using System.Collections;
using UnityEngine;
using TMPro;

public class FloatingText3D : MonoBehaviour
{
    [Header("Text Settings")]
    public TextMeshPro textMesh; // Referencia al componente TextMeshPro.
    public float fadeDuration = 0.5f; // Duración del desvanecimiento.
    public float floatSpeed = 1f; // Velocidad del movimiento flotante.
    public float floatRange = 0.5f; // Rango del movimiento flotante (arriba y abajo).

    private Vector3 initialPosition; // Posición inicial del texto.
    private Coroutine fadeCoroutine; // Controlador para el fade in/out.

    private void Start()
    {
        // Guardar la posición inicial
        initialPosition = transform.position;

        // Asegurarse de que el TextMeshPro está asignado
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        if (textMesh == null)
        {
            Debug.LogError("No se encontró un componente TextMeshPro en este objeto.");
        }

        // Asegurarse de que el texto comienza invisible
        SetAlpha(0f);
    }

    private void Update()
    {
        // Movimiento de arriba hacia abajo
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Verificar si el jugador entra al trigger.
        {
            ShowText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Verificar si el jugador sale del trigger.
        {
            HideText();
        }
    }

    private void ShowText()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeText(1f)); // Desvanecer hacia visible
    }

    private void HideText()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeText(0f)); // Desvanecer hacia invisible
    }

    private IEnumerator FadeText(float targetAlpha)
    {
        float startAlpha = textMesh.color.a; // Obtener la opacidad actual
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            SetAlpha(newAlpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        // Cambiar la opacidad del texto
        if (textMesh != null)
        {
            Color color = textMesh.color;
            textMesh.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
}
