using UnityEngine;

public class SmoothAppearance : MonoBehaviour
{
    [Header("Timing Settings")]
    public float initialDelay = 2f; // Tiempo antes de que comience a aparecer.
    public float visibleDuration = 3f; // Tiempo que permanece completamente visible.

    [Header("Fade Settings")]
    public float fadeInSpeed = 1f; // Velocidad para aparecer.
    public float fadeOutSpeed = 1f; // Velocidad para desaparecer.

    [Header("Opacity Settings")]
    [Range(0f, 1f)] public float minOpacity = 0f; // Opacidad inicial (0 completamente invisible).
    [Range(0f, 1f)] public float maxOpacity = 1f; // Opacidad final (1 completamente visible).

    private SpriteRenderer spriteRenderer; // Renderer del sprite.
    private float currentOpacity = 0f; // Opacidad actual.
    private bool isFadingIn = false; // Estado de transición.

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("El objeto no tiene un SpriteRenderer adjunto.");
            return;
        }

        // Configurar el objeto como invisible al inicio
        currentOpacity = minOpacity;
        SetOpacity(currentOpacity);

        // Iniciar el ciclo de aparición
        Invoke(nameof(StartFadeIn), initialDelay);
    }

    private void Update()
    {
        if (spriteRenderer == null) return;

        if (isFadingIn)
        {
            // Aumentar la opacidad suavemente
            currentOpacity = Mathf.MoveTowards(currentOpacity, maxOpacity, fadeInSpeed * Time.deltaTime);
            SetOpacity(currentOpacity);

            if (Mathf.Approximately(currentOpacity, maxOpacity))
            {
                // Cuando alcance la opacidad máxima, esperar antes de desaparecer
                isFadingIn = false;
                Invoke(nameof(StartFadeOut), visibleDuration);
            }
        }
    }

    private void StartFadeIn()
    {
        isFadingIn = true;
    }

    private void StartFadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private System.Collections.IEnumerator FadeOutCoroutine()
    {
        while (currentOpacity > minOpacity)
        {
            currentOpacity = Mathf.MoveTowards(currentOpacity, minOpacity, fadeOutSpeed * Time.deltaTime);
            SetOpacity(currentOpacity);
            yield return null;
        }
    }

    private void SetOpacity(float opacity)
    {
        Color color = spriteRenderer.color;
        color.a = opacity;
        spriteRenderer.color = color;
    }
}
