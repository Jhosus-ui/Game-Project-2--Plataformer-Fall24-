using System.Collections;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float vida = 100f;        // Vida del enemigo
    [SerializeField] private Transform controladorGolpe; // Punto de ataque
    [SerializeField] private float radioGolpe = 1f;    // Radio de ataque
    [SerializeField] private float danoAtaque = 20f;  // Da�o que inflige el enemigo
    [SerializeField] private float cooldownAtaque = 1f; // Tiempo entre ataques
    [SerializeField] private float frameAtaque = 1f;  // Momento de contacto durante la animaci�n de ataque

    private Transform jugador; // Referencia al jugador
    private bool puedeAtacar = true; // Controla si el enemigo puede atacar
    private bool estaMuerto = false; // Controla si el enemigo ya est� en la animaci�n de muerte
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 posicionInicialGolpe; // Posici�n relativa inicial del controlador de golpe

    public float Vida => vida; // Propiedad p�blica para obtener la vida del enemigo

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform; // Busca al jugador por su tag
        posicionInicialGolpe = controladorGolpe.localPosition; // Guarda la posici�n inicial del controlador de golpe
    }

    private void Update()
    {
        if (estaMuerto) return; // Si el enemigo est� muerto, no hacer nada

        ActualizarControladorGolpe();

        // Verificar si el jugador est� en rango para atacar
        if (jugador != null && Vector2.Distance(transform.position, jugador.position) <= radioGolpe && puedeAtacar)
        {
            StartCoroutine(Atacar());
        }
    }

    /// <summary>
    /// Actualiza la posici�n del controlador de golpe seg�n la direcci�n del sprite
    /// </summary>
    private void ActualizarControladorGolpe()
    {
        controladorGolpe.localPosition = spriteRenderer.flipX
            ? new Vector3(-posicionInicialGolpe.x, posicionInicialGolpe.y, posicionInicialGolpe.z)
            : posicionInicialGolpe;
    }

    /// <summary>
    /// L�gica de ataque del enemigo
    /// </summary>
    private IEnumerator Atacar()
    {
        puedeAtacar = false;
        animator.SetTrigger("Atacar");

        // Esperar hasta el momento de contacto del ataque
        yield return new WaitForSeconds(frameAtaque);

        if (estaMuerto) yield break; // Si el enemigo muri� antes del contacto, cancelar el ataque

        // Detectar colisiones en el �rea de ataque
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Player"))
            {
                PlayerHealth playerHealth = colisionador.GetComponent<PlayerHealth>();
                if (playerHealth != null && !playerHealth.IsDead())
                {
                    playerHealth.TomarDano(danoAtaque); // Aplicar da�o al jugador
                }
            }
        }

        // Esperar el cooldown antes del pr�ximo ataque
        yield return new WaitForSeconds(cooldownAtaque);
        puedeAtacar = true;
    }

    /// <summary>
    /// L�gica para recibir da�o
    /// </summary>
    /// <param name="dano">Da�o recibido</param>
    public void TomarDano(float dano)
    {
        if (estaMuerto) return; // Si ya est� muerto, ignorar da�o

        vida -= dano;

        if (vida <= 0)
        {
            vida = 0; // Asegurarse de que no sea negativa
            Muerte();
        }
    }

    /// <summary>
    /// L�gica de muerte del enemigo
    /// </summary>
    private void Muerte()
    {
        estaMuerto = true; // Marcar al enemigo como muerto
        animator.SetTrigger("Muerte"); // Activar la animaci�n de muerte
        controladorGolpe.gameObject.SetActive(false); // Desactivar el controlador de golpe para evitar da�o
        Destroy(gameObject, 1f); // Destruir al enemigo despu�s de la animaci�n de muerte
    }

    /// <summary>
    /// Dibuja el �rea de ataque en la vista de escena
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (controladorGolpe != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
        }
    }
}
