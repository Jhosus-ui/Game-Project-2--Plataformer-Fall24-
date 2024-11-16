using System.Collections;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] public float vida;
    [SerializeField] private Transform controladorGolpe; // Referencia al punto de ataque
    [SerializeField] private float radioGolpe = 1f;      // Radio del �rea de ataque
    [SerializeField] private float danoAtaque = 20f;     // Da�o que inflige el ataque
    [SerializeField] private float cooldownAtaque = 1f;  // Tiempo entre ataques

    private Transform jugador; // Referencia al jugador
    private bool puedeAtacar = true; // Controla si el enemigo puede atacar
    private Animator animator;
    private SpriteRenderer spriteRenderer; // Para determinar la direcci�n del sprite
    private Vector3 posicionInicialGolpe; // Guarda la posici�n inicial del `controladorGolpe`

    public float Vida // Propiedad p�blica para leer la vida
    {
        get { return vida; }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform; // Busca al jugador por tag
        posicionInicialGolpe = controladorGolpe.localPosition; // Guarda la posici�n relativa inicial del controlador de golpe
    }

    private void Update()
    {
        // Actualizar la posici�n del controlador de ataque seg�n la direcci�n del enemigo
        ActualizarControladorGolpe();

        // Si el jugador est� en rango y el enemigo puede atacar
        if (jugador != null && Vector2.Distance(transform.position, jugador.position) <= radioGolpe && puedeAtacar)
        {
            StartCoroutine(Atacar());
        }
    }

    private void ActualizarControladorGolpe()
    {
        // Cambiar la posici�n del controlador de ataque seg�n la direcci�n del sprite
        if (spriteRenderer.flipX) // Si el sprite est� mirando hacia la izquierda
        {
            controladorGolpe.localPosition = new Vector3(-posicionInicialGolpe.x, posicionInicialGolpe.y, posicionInicialGolpe.z);
        }
        else // Si el sprite est� mirando hacia la derecha
        {
            controladorGolpe.localPosition = posicionInicialGolpe;
        }
    }

    private IEnumerator Atacar()
    {
        puedeAtacar = false; // Deshabilitar ataques mientras est� en cooldown
        animator.SetTrigger("Atacar"); // Activar animaci�n de ataque

        // Esperar a que la animaci�n haga contacto (ajusta seg�n tu animaci�n)
        yield return new WaitForSeconds(0.54f);

        // Detectar colisiones en el radio de ataque
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Player"))
            {
                PlayerHealth playerHealth = colisionador.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TomarDano(danoAtaque); // Reducir vida del jugador
                }
            }
        }

        // Esperar el cooldown antes del pr�ximo ataque
        yield return new WaitForSeconds(cooldownAtaque);

        puedeAtacar = true; // Permitir otro ataque
    }

    private void OnDrawGizmosSelected()
    {
        if (controladorGolpe != null)
        {
            Gizmos.color = Color.red; // Color del Gizmo
            Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe); // Dibujar el rango de ataque
        }
    }

    public void TomarDano(float dano)
    {
        vida -= dano;

        if (vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        animator.SetTrigger("Muerte");
        Destroy(gameObject, 1f); // Destruir enemigo despu�s de la animaci�n
    }
}
