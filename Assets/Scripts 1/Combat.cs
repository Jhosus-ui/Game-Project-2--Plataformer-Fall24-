using System.Collections;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [Header("Ataque Ligero")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float danoGolpeLigero;
    [SerializeField] private float tiempoDeAtaqueLigero;
    [SerializeField] private float costoEstaminaLigero = 10f; // Costo de estamina para ataque ligero

    [Header("Ataque Pesado")]
    [SerializeField] private float danoGolpePesado;
    [SerializeField] private float tiempoDeAtaquePesado;
    [SerializeField] private float retrasoGolpePesado; // Retraso antes de aplicar daño (por animación)
    [SerializeField] private float costoEstaminaPesado = 20f; // Costo de estamina para ataque pesado

    [Header("Sonidos")]
    [SerializeField] private AudioClip sonidoGolpeLigero; // Sonido del ataque ligero
    [SerializeField] private AudioClip sonidoGolpePesado; // Sonido del ataque pesado

    private AudioSource audioSource;

    private float tiempoSiguienteAtaqueLigero;
    private float tiempoSiguienteAtaquePesado;

    private Animator animator;
    private SpriteRenderer spriteRenderer; // Para verificar la dirección del personaje
    private Vector3 offsetInicial; // Guarda la posición inicial del controladorGolpe relativa al jugador
    private StaminaManager staminaManager; // Referencia al script de estamina

    // Variables de estado
    private bool isDead = false; // Verifica si el personaje está muerto

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        offsetInicial = controladorGolpe.localPosition; // Posición relativa inicial
        audioSource = GetComponent<AudioSource>(); // Obtener AudioSource del objeto
        staminaManager = GetComponent<StaminaManager>(); // Obtener referencia al script de estamina

        if (staminaManager == null)
        {
            Debug.LogError("StaminaManager no encontrado. Asegúrate de que el script está en el mismo GameObject.");
        }
    }

    private void Update()
    {
        // Si el personaje está muerto, desactivar controles de ataque
        if (isDead)
        {
            return; // No realizar ninguna acción
        }

        // Reducir cooldown de ataques
        if (tiempoSiguienteAtaqueLigero > 0)
        {
            tiempoSiguienteAtaqueLigero -= Time.deltaTime;
        }
        if (tiempoSiguienteAtaquePesado > 0)
        {
            tiempoSiguienteAtaquePesado -= Time.deltaTime;
        }

        // Ajustar posición del controladorGolpe al frente del personaje
        controladorGolpe.localPosition = spriteRenderer.flipX
            ? new Vector3(-offsetInicial.x, offsetInicial.y, offsetInicial.z) // A la izquierda
            : offsetInicial; // A la derecha

        // Ejecutar ataque ligero si el cooldown permite
        if (Input.GetButtonDown("Fire1") && tiempoSiguienteAtaqueLigero <= 0)
        {
            if (staminaManager.ConsumeStamina(costoEstaminaLigero)) // Consumir estamina
            {
                AtaqueLigero();
                tiempoSiguienteAtaqueLigero = tiempoDeAtaqueLigero;
            }
        }

        // Ejecutar ataque pesado si el cooldown permite
        if (Input.GetButtonDown("Fire2") && tiempoSiguienteAtaquePesado <= 0)
        {
            if (staminaManager.ConsumeStamina(costoEstaminaPesado)) // Consumir estamina
            {
                StartCoroutine(AtaquePesado());
                tiempoSiguienteAtaquePesado = tiempoDeAtaquePesado;
            }
        }
    }

    private void AtaqueLigero()
    {
        animator.SetTrigger("AtaqueLigero");

        // Reproducir sonido del ataque ligero
        if (sonidoGolpeLigero != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoGolpeLigero);
        }

        // Detectar colisiones en el radio de ataque
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemigo"))
            {
                Enemigo enemigo = colisionador.GetComponent<Enemigo>();
                if (enemigo != null)
                {
                    enemigo.TomarDano(danoGolpeLigero);
                }
            }
        }
    }

    private IEnumerator AtaquePesado()
    {
        animator.SetTrigger("AtaquePesado");

        // Reproducir sonido del ataque pesado
        if (sonidoGolpePesado != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoGolpePesado);
        }

        // Esperar el retraso para sincronizar con la animación
        yield return new WaitForSeconds(retrasoGolpePesado);

        // Detectar colisiones en el radio de ataque
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemigo"))
            {
                Enemigo enemigo = colisionador.GetComponent<Enemigo>();
                if (enemigo != null)
                {
                    enemigo.TomarDano(danoGolpePesado);
                }
            }
        }
    }

    // Método para desactivar el combate cuando el personaje muere o hay Game Over
    public void DesactivarCombate()
    {
        isDead = true; // Marcar como muerto para bloquear controles
        Debug.Log("Combate desactivado: el jugador está muerto o en Game Over.");
    }

    private void OnDrawGizmos()
    {
        if (controladorGolpe != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
        }
    }
}
