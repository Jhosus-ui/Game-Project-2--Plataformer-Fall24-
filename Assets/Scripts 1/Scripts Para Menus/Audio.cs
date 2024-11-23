using UnityEngine;

public class DelayedAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip; // El audio que se reproducirá
    [SerializeField] private float delayInSeconds = 3f; // Tiempo en segundos antes de reproducir el audio

    private AudioSource audioSource;

    private void Start()
    {
        // Configurar el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = audioClip; // Asignar el clip al AudioSource

        if (audioClip != null)
        {
            // Iniciar la reproducción después de X segundos
            Invoke(nameof(PlayAudio), delayInSeconds);
        }
        else
        {
            Debug.LogWarning("No se asignó un audioClip en " + gameObject.name);
        }
    }

    private void PlayAudio()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.Play();
        }
    }
}
