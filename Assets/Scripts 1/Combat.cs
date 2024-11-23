using System.Collections;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float danoGolpe;
    [SerializeField] private float tiempoDeAtaque;

    [SerializeField] private AudioClip sonidoGolpe; // Sonido del ataque
    private AudioSource audioSource;

    private float tiempoSiguienteAtaque;
    private Animator animator;
    private SpriteRenderer spriteRenderer; // Para verificar la dirección del personaje
    private Vector3 offsetInicial; // Guarda la posición inicial del controladorGolpe relativa al jugador

    // Variables de estado
    private bool isJumping = false; // Verifica si el personaje está saltando
    private bool isDead = false;    // Verifica si el personaje está muerto

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        offsetInicial = controladorGolpe.localPosition; // Posición relativa inicial
        audioSource = GetComponent<AudioSource>(); // Obtener AudioSource del objeto
    }

    private void Update()
    {
        // Si el personaje está muerto, desactivar controles de ataque
        if (isDead)
        {
            return; // No realizar ninguna acción
        }

        // Reducir cooldown del ataque
        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }

        // Ajustar posición del controladorGolpe al frente del personaje
        controladorGolpe.localPosition = spriteRenderer.flipX
            ? new Vector3(-offsetInicial.x, offsetInicial.y, offsetInicial.z) // A la izquierda
            : offsetInicial; // A la derecha

        // Verificar si está saltando; si está saltando, no se permite atacar
        if (isJumping)
        {
            return; // No hacer nada si está saltando
        }

        // Ejecutar ataque si el cooldown permite
        if (Input.GetButtonDown("Fire1") && tiempoSiguienteAtaque <= 0)
        {
            Golpe();
            tiempoSiguienteAtaque = tiempoDeAtaque;
        }
    }

    private void Golpe()
    {
        animator.SetTrigger("Golpe");

        // Reproducir sonido del golpe
        if (sonidoGolpe != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoGolpe);
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
                    enemigo.TomarDano(danoGolpe);
                }
            }
        }
    }

    // Método para actualizar el estado de saltar
    public void SetIsJumping(bool jumpingState)
    {
        isJumping = jumpingState;
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
