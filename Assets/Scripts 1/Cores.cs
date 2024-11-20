using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cores : MonoBehaviour
{
    [SerializeField] private GameObject efecto; // Efecto visual o partículas al recoger el ítem
    [SerializeField] private float cantidadPuntos; // Cantidad de puntos que este ítem da
    [SerializeField] private Puntaje puntaje; // Referencia al script de puntaje

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (efecto != null)
            {
                Instantiate(efecto, transform.position, Quaternion.identity); // Instancia efecto visual
            }

            puntaje.SumarPuntos(cantidadPuntos); // Añade puntos
            Destroy(gameObject); // Destruye el ítem
        }
    }
}
