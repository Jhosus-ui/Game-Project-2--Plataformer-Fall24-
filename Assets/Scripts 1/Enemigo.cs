using System.Collections;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float vida = 100f; // Vida del enemigo
    [SerializeField] private Transform controladorGolpe; // Punto de ataque
    [SerializeField] private float radioGolpe = 1f; // Radio de ataque
    [SerializeField] private float danoAtaque = 20f; // Daño que inflige el enemigo
    [SerializeField] private float cooldownAtaque = 1f; // Tiempo entre ataques
    [SerializeField] private float frameAtaque = 1f; // Momento de contacto durante la animación de ataque
    [SerializeField] private AudioClip sonidoAtaque; // Sonido de ataque
    [SerializeField] private AudioClip sonidoMuerte; // Sonido de muerte

    private Transform jugador; // Referencia al jugador
    private bool puedeAtacar = true; // Controla si el enemigo puede atacar
    private bool estaMuerto = false; // Controla si el enemigo ya está en la animación de muerte
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 posicionInicialGolpe; // Posición relativa inicial del controlador de golpe
    private AudioSource audioSource;

    public float Vida => vida; // Propiedad pública para obtener la vida del enemigo

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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

    private void ActualizarControladorGolpe()
    {
        controladorGolpe.localPosition = spriteRenderer.flipX
            ? new Vector3(-posicionInicialGolpe.x, posicionInicialGolpe.y, posicionInicialGolpe.z)
            : posicionInicialGolpe;
    }

    private IEnumerator Atacar()
    {
        puedeAtacar = false;
        animator.SetTrigger("Atacar");

        // Reproducir sonido de ataque
        if (sonidoAtaque != null && audioSource != null)
        {
            audioSource.clip = sonidoAtaque; // Asignar el clip de sonido
            audioSource.Play(); // Reproducir el sonido de ataque
        }

        yield return new WaitForSeconds(frameAtaque);

        if (estaMuerto) yield break; // Si el enemigo murió antes del contacto, cancelar el ataque

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

        yield return new WaitForSeconds(cooldownAtaque);
        puedeAtacar = true;
    }

    public void TomarDano(float dano)
    {
        if (estaMuerto) return; // Si ya está muerto, ignorar daño

        vida -= dano;

        if (vida <= 0)
        {
            vida = 0; // Asegurarse de que no sea negativa
            StartCoroutine(Muerte());
        }
    }

    private IEnumerator Muerte()
    {
        estaMuerto = true; // Marcar al enemigo como muerto
        animator.SetTrigger("Muerte"); // Activar la animación de muerte
        controladorGolpe.gameObject.SetActive(false); // Desactivar el controlador de golpe para evitar daño

        // Detener cualquier sonido en reproducción
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Reproducir sonido de muerte
        if (sonidoMuerte != null && audioSource != null)
        {
            audioSource.clip = sonidoMuerte;
            audioSource.Play();
            yield return new WaitForSeconds(sonidoMuerte.length); // Esperar a que termine el audio
        }

        Destroy(gameObject); // Destruir al enemigo después de que el audio termine
    }

    private void OnDrawGizmosSelected()
    {
        if (controladorGolpe != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
        }
    }
}
