using UnityEngine;

public class SmoothDisappear : MonoBehaviour
{
    [Header("Timing Settings")]
    public float disappearDelay = 5f; // Tiempo antes de que el objeto comience a desaparecer.

    [Header("Fade Settings")]
    public float fadeOutSpeed = 1f; // Velocidad para desaparecer.

    [Header("Opacity Settings")]
    [Range(0f, 1f)] public float minOpacity = 0f; // Opacidad mínima (0 completamente invisible).
    [Range(0f, 1f)] public float maxOpacity = 1f; // Opacidad inicial (1 completamente visible).

    private SpriteRenderer spriteRenderer; // Renderer del sprite.
    private float currentOpacity; // Opacidad actual.

    private void Start()
    {
        // Obtener el SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("El objeto no tiene un SpriteRenderer adjunto.");
            return;
        }

        // Configurar el objeto como completamente visible al inicio
        currentOpacity = maxOpacity;
        SetOpacity(currentOpacity);

        // Comenzar el desvanecimiento después de un retraso
        Invoke(nameof(StartFadeOut), disappearDelay);
    }

    private void StartFadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private System.Collections.IEnumerator FadeOutCoroutine()
    {
        while (currentOpacity > minOpacity)
        {
            // Reducir la opacidad suavemente  // Aplicarte Tecnicas De procesamiento.
            currentOpacity = Mathf.MoveTowards(currentOpacity, minOpacity, fadeOutSpeed * Time.deltaTime);
            SetOpacity(currentOpacity);
            yield return null;
        }

        // Una vez invisible, puedes desactivar el objeto si lo deseas
        gameObject.SetActive(false);
    }

    private void SetOpacity(float opacity)
    {
        Color color = spriteRenderer.color;
        color.a = opacity;
        spriteRenderer.color = color;
    }
}
