using UnityEngine;

public class SeguirJugadorSuelo : MonoBehaviour
{
    [Header("Área de Búsqueda")]
    public Vector2 areaBusqueda = new Vector2(5f, 2f); // Dimensiones del área de búsqueda
    public Vector2 offsetBusqueda = Vector2.zero; // Offset del área de búsqueda desde el enemigo

    [Header("Área de Movimiento")]
    public Vector2 areaMovimiento = new Vector2(10f, 4f); // Dimensiones del área de movimiento permitido
    public Vector2 offsetMovimiento = Vector2.zero; // Offset del área de movimiento

    public LayerMask capaJugador;
    public Transform transformJugador;
    public float velocidadMovimiento = 2f;
    public float distanciaMinima = 1f; // Distancia mínima desde el jugador para detenerse

    private Vector3 puntoInicial;
    public bool mirandoDerecha;
    public EstadosMovimiento estadoActual;

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
        puntoInicial = transform.position; // Guardar la posición inicial del enemigo
        enemigo = GetComponent<Enemigo>(); // Obtener referencia al script Enemigo
    }

    private void Update()
    {
        // Detener movimiento si la vida es menor o igual a 0
        if (enemigo != null && enemigo.Vida <= 0)
        {
            estadoActual = EstadosMovimiento.Esperando; // Cambiar estado a Esperando
            rb2D.velocity = Vector2.zero; // Detener movimiento
            animator.SetBool("Corriendo", false); // Detener animación de correr
            return; // Detener ejecución de Update
        }

        // Cambiar estados según la lógica
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
        // Usar OverlapBox para detectar al jugador en el área de búsqueda
        Collider2D jugadorCollider = Physics2D.OverlapBox(
            (Vector2)transform.position + offsetBusqueda,
            areaBusqueda,
            0f,
            capaJugador
        );

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

        // Si está dentro de la distancia mínima, detenerse
        if (Vector2.Distance(transform.position, transformJugador.position) <= distanciaMinima)
        {
            rb2D.velocity = Vector2.zero;
            animator.SetBool("Corriendo", false);
            return; // Salir de la lógica de seguimiento
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

        // Cambiar estado a Volviendo si el jugador está fuera del área de movimiento
        Vector3 posicionMovimiento = puntoInicial + (Vector3)offsetMovimiento;
        if (!DentroDeAreaMovimiento(transform.position, posicionMovimiento, areaMovimiento))
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

    private bool DentroDeAreaMovimiento(Vector3 posicion, Vector3 centro, Vector2 dimensiones)
    {
        return posicion.x > centro.x - dimensiones.x / 2 &&
               posicion.x < centro.x + dimensiones.x / 2 &&
               posicion.y > centro.y - dimensiones.y / 2 &&
               posicion.y < centro.y + dimensiones.y / 2;
    }

    private void OnDrawGizmos()
    {
        // Área de búsqueda
        Gizmos.color = Color.red;
        Vector3 posicionBusqueda = transform.position + (Vector3)offsetBusqueda;
        Gizmos.DrawWireCube(posicionBusqueda, new Vector3(areaBusqueda.x, areaBusqueda.y, 1f));

        // Área de movimiento
        Gizmos.color = Color.blue;
        Vector3 posicionMovimiento = puntoInicial + (Vector3)offsetMovimiento;
        Gizmos.DrawWireCube(posicionMovimiento, new Vector3(areaMovimiento.x, areaMovimiento.y, 1f));
    }
}
