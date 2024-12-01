using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirJugadorSuelo : MonoBehaviour
{
    public float radioBusqueda;
    public LayerMask capaJugador;
    public Transform transformJugador;
    public float velocidadMovimiento;
    public float distanciaMaxima;
    public Vector3 puntoInicial;
    public float distanciaMinima = 1f; // Distancia m�nima desde el jugador para detenerse

    public EstadosMovimiento estadoActual;

    public bool mirandoDerecha;

    public Rigidbody2D rb2D;
    public Animator animator;

    private Enemigo enemigo; // Referencia al script Enemigo

    public enum EstadosMovimiento
    {
        Esperando,
        Siguiendo,
        Volviendo,
    }

    private void Start()
    {
        puntoInicial = transform.position;
        enemigo = GetComponent<Enemigo>(); // Obtener el componente Enemigo
    }

    private void Update()
    {
        // Detener movimiento si la vida es menor o igual a 0
        if (enemigo.Vida <= 0) // Accede a la propiedad o m�todo Vida en el script Enemigo
        {
            estadoActual = EstadosMovimiento.Esperando; // Cambiar estado a Esperando
            rb2D.velocity = Vector2.zero; // Detener movimiento
            animator.SetBool("Corriendo", false); // Detener animaci�n de correr
            return; // Detener ejecuci�n de Update
        }

        // L�gica del estado actual
        switch (estadoActual)
        {
            case EstadosMovimiento.Esperando:
                EstadoEsperando();
                break;
            case EstadosMovimiento.Siguiendo:
                EstadoSiguiendo();
                break;
            case EstadosMovimiento.Volviendo:
                EstadoVolviendo();
                break;
        }
    }

    private void EstadoEsperando()
    {
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBusqueda, capaJugador);

        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;
            estadoActual = EstadosMovimiento.Siguiendo;
        }
    }

    private void EstadoSiguiendo()
    {
        animator.SetBool("Corriendo", true);

        if (transformJugador == null)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            return;
        }

        // Si est� dentro de la distancia m�nima, detenerse
        if (Vector2.Distance(transform.position, transformJugador.position) <= distanciaMinima)
        {
            rb2D.velocity = Vector2.zero;
            animator.SetBool("Corriendo", false); // Detener la animaci�n de correr
            return; // Salir de la l�gica de seguimiento
        }

        // Mover hacia el jugador
        if (transform.position.x < transformJugador.position.x)
        {
            rb2D.velocity = new Vector2(velocidadMovimiento, rb2D.velocity.y);
        }
        else
        {
            rb2D.velocity = new Vector2(-velocidadMovimiento, rb2D.velocity.y);
        }

        GirarAObjetivo(transformJugador.position);

        // Cambiar estado a Volviendo si se excede la distancia m�xima
        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMaxima ||
            Vector2.Distance(transform.position, transformJugador.position) > distanciaMaxima)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
        }
    }

    private void EstadoVolviendo()
    {
        // Mover hacia el punto inicial
        if (transform.position.x < puntoInicial.x)
        {
            rb2D.velocity = new Vector2(velocidadMovimiento, rb2D.velocity.y);
        }
        else
        {
            rb2D.velocity = new Vector2(-velocidadMovimiento, rb2D.velocity.y);
        }

        GirarAObjetivo(puntoInicial);

        // Cambiar estado a Esperando si ha regresado al punto inicial
        if (Vector2.Distance(transform.position, puntoInicial) < 0.1f)
        {
            rb2D.velocity = Vector2.zero;
            animator.SetBool("Corriendo", false);
            estadoActual = EstadosMovimiento.Esperando;
        }
    }

    private void GirarAObjetivo(Vector3 objetivo)
    {
        if (objetivo.x > transform.position.x && !mirandoDerecha)
        {
            Girar();
        }
        else if (objetivo.x < transform.position.x && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanciaMinima); // Representa la distancia m�nima
        Gizmos.DrawWireSphere(puntoInicial, distanciaMaxima);
    }
}
