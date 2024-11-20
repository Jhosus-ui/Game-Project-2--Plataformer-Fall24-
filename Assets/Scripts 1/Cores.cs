using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cores : MonoBehaviour
{
    [SerializeField] private GameObject efecto; // Efecto visual o part�culas al recoger el �tem
    [SerializeField] private float cantidadPuntos; // Cantidad de puntos que este �tem da
    [SerializeField] private Puntaje puntaje; // Referencia al script de puntaje

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (efecto != null)
            {
                Instantiate(efecto, transform.position, Quaternion.identity); // Instancia efecto visual
            }

            puntaje.SumarPuntos(cantidadPuntos); // A�ade puntos
            Destroy(gameObject); // Destruye el �tem
        }
    }
}
