using UnityEngine;

public class Core : MonoBehaviour
{
    public int valor = 1; // Cantidad de n�cleos que este �tem a�ade al recolectarse.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Detecta si el jugador toca el n�cleo.
        {
            CoreManager.instance.A�adirCores(valor); // Llama al m�todo para a�adir n�cleos.
            Destroy(gameObject); // Destruye el n�cleo recolectado.
        }
    }
}
