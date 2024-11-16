using System.Collections;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float danoGolpe;
    [SerializeField] private float tiempoDeAtaque;

    private float tiempoSiguienteAtaque;
    private Animator animator;
    private SpriteRenderer spriteRenderer; // Para verificar la dirección del personaje
    private Vector3 offsetInicial; // Guarda la posición inicial del controladorGolpe relativa al jugador

    // Variable para verificar si el personaje está saltando
    private bool isJumping = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        offsetInicial = controladorGolpe.localPosition; // Posición relativa inicial
    }

    private void Update()
    {
        // Reducir cooldown del ataque
        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }

        // Ajustar posición del controladorGolpe al frente del personaje
        controladorGolpe.localPosition = spriteRenderer.flipX
            ? new Vector3(-offsetInicial.x, offsetInicial.y, offsetInicial.z) // A la izquierda
            : offsetInicial; // A la derecha

        // Verificar si está saltando, si está saltando no se permite atacar
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

    private void OnDrawGizmos()
    {
        if (controladorGolpe != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
        }
    }
}
