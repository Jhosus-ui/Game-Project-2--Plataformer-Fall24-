using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirJugadorSuelo : MonoBehaviour
{

    public float radioBusqueda;

    public LayerMask capaJugador;

    public Transform transformJugador;

    public EstadosMovimiento estadoActual;

    public enum EstadosMovimiento
    {
        Esperando,

        Siguiendo,

        Volviendo,
    }


    private void Update()
    {

        switch (estadoActual)
        {
            case EstadosMovimiento.Esperando:
                break;
            case EstadosMovimiento.Siguiendo:
                break;
            case EstadosMovimiento.Volviendo:
                break;
        }





        Collider2D jugadorCollider = Physics2D.OverlapCircle(transformJugador.position, radioBusqueda, capaJugador);


        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transformJugador.position, radioBusqueda);

    }
}
