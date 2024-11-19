using System.Collections;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float vida = 100f;        // Vida del enemigo
    [SerializeField] private Transform controladorGolpe; // Punto de ataque
    [SerializeField] private float radioGolpe = 1f;    // Radio de ataque
    [SerializeField] private float danoAtaque = 20f;  // Daño que inflige el enemigo
    [SerializeField] private float cooldownAtaque = 1f; // Tiempo entre ataques
    [SerializeField] private float frameAtaque = 1f;  // Momento de contacto durante la animación de ataque

    private Transform jugador; // Referencia al jugador
    private bool puedeAtacar = true; // Controla si el enemigo puede atacar
    private bool estaMuerto = false; // Controla si el enemigo ya está en la animación de muerte
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 posicionInicialGolpe; // Posición relativa inicial del controlador de golpe

    public float Vida => vida; // Propiedad pública para obtener la vida del enemigo

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform; // Busca al jugador por su tag
        posicionInicialGolpe = controladorGolpe.localPosition; // Guarda la posición inicial del controlador de golpe
    }

    private void Update()
    {
        if (estaMuerto) return; // Si el enemigo está muerto, no hacer nada

        ActualizarControladorGolpe();

        // Verificar si el jugador está en rango para atacar
        if (jugador != null && Vector2.Distance(transform.position, jugador.position) <= radioGolpe && puedeAtacar)
        {
            StartCoroutine(Atacar());
        }
    }

    /// <summary>
    /// Actualiza la posición del controlador de golpe según la dirección del sprite
    /// </summary>
    private void ActualizarControladorGolpe()
    {
        controladorGolpe.localPosition = spriteRenderer.flipX
            ? new Vector3(-posicionInicialGolpe.x, posicionInicialGolpe.y, posicionInicialGolpe.z)
            : posicionInicialGolpe;
    }

    /// <summary>
    /// Lógica de ataque del enemigo
    /// </summary>
    private IEnumerator Atacar()
    {
        puedeAtacar = false;
        animator.SetTrigger("Atacar");

        // Esperar hasta el momento de contacto del ataque
        yield return new WaitForSeconds(frameAtaque);

        if (estaMuerto) yield break; // Si el enemigo murió antes del contacto, cancelar el ataque

        // Detectar colisiones en el área de ataque
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Player"))
            {
                PlayerHealth playerHealth = colisionador.GetComponent<PlayerHealth>();
                if (playerHealth != null && !playerHealth.IsDead())
                {
                    playerHealth.TomarDano(danoAtaque); // Aplicar daño al jugador
                }
            }
        }

        // Esperar el cooldown antes del próximo ataque
        yield return new WaitForSeconds(cooldownAtaque);
        puedeAtacar = true;
    }

    /// <summary>
    /// Lógica para recibir daño
    /// </summary>
    /// <param name="dano">Daño recibido</param>
    public void TomarDano(float dano)
    {
        if (estaMuerto) return; // Si ya está muerto, ignorar daño

        vida -= dano;

        if (vida <= 0)
        {
            vida = 0; // Asegurarse de que no sea negativa
            Muerte();
        }
    }

    /// <summary>
    /// Lógica de muerte del enemigo
    /// </summary>
    private void Muerte()
    {
        estaMuerto = true; // Marcar al enemigo como muerto
        animator.SetTrigger("Muerte"); // Activar la animación de muerte
        controladorGolpe.gameObject.SetActive(false); // Desactivar el controlador de golpe para evitar daño
        Destroy(gameObject, 1f); // Destruir al enemigo después de la animación de muerte
    }

    /// <summary>
    /// Dibuja el área de ataque en la vista de escena
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
