using System.Collections; // Importa el espacio de nombres para corrutinas
using UnityEngine;

public class Core : MonoBehaviour
{
    public int valor = 1; // Cantidad de n�cleos que este �tem a�ade al recolectarse.
    [SerializeField] private AudioClip sonidoRecolecta; // Sonido que se reproduce al recolectar
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Detecta si el jugador toca el n�cleo.
        {
            CoreManager.instance.A�adirCores(valor); // Llama al m�todo para a�adir n�cleos.

            if (sonidoRecolecta != null && audioSource != null)
            {
                // Reproduce el sonido y luego destruye el objeto
                audioSource.PlayOneShot(sonidoRecolecta);
                StartCoroutine(DestruirDespuesDeSonido());
            }
            else
            {
                // Si no hay sonido, destruir inmediatamente
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator DestruirDespuesDeSonido()
    {
        // Espera a que el sonido termine
        yield return new WaitForSeconds(sonidoRecolecta.length);
        Destroy(gameObject); // Destruye el n�cleo recolectado
    }
}
