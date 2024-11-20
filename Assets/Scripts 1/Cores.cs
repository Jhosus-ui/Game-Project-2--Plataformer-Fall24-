using UnityEngine;

public class Core : MonoBehaviour
{
    public int valor = 1; // Cantidad de núcleos que este ítem añade al recolectarse.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Detecta si el jugador toca el núcleo.
        {
            CoreManager.instance.AñadirCores(valor); // Llama al método para añadir núcleos.
            Destroy(gameObject); // Destruye el núcleo recolectado.
        }
    }
}
