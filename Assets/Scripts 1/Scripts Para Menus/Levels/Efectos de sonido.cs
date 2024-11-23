using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonSoundManager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip sonidoHover; // Sonido al pasar el cursor
    [SerializeField] private AudioClip sonidoClick; // Sonido al hacer clic

    private AudioSource audioSource;
    private bool clickProcesado = false; // Evita m�ltiples clics antes de completar el sonido

    private void Awake()
    {
        // Aseg�rate de que el bot�n tenga un AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Reproducir sonido al pasar el cursor por encima
        if (sonidoHover != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoHover);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickProcesado) return; // Evita m�ltiples clics
        clickProcesado = true;

        if (sonidoClick != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoClick);
            StartCoroutine(RealizarAccionDespuesDelAudio(sonidoClick.length));
        }
        else
        {
            RealizarAccion(); // Si no hay sonido, realiza la acci�n de inmediato
        }
    }

    private System.Collections.IEnumerator RealizarAccionDespuesDelAudio(float delay)
    {
        yield return new WaitForSeconds(delay);
        RealizarAccion();
    }

    private void RealizarAccion()
    {
        // Aqu� puedes configurar lo que debe hacer el bot�n al hacer clic
        // Ejemplo: cargar una escena espec�fica
        Debug.Log("Acci�n realizada despu�s del sonido.");
    }
}
