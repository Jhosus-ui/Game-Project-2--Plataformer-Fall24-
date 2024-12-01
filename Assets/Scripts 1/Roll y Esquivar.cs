using UnityEngine;

public class PlayerDodgeRoll : MonoBehaviour
{
    [Header("Roll Settings")]
    public float rollDistance = 3f; // Distancia del roll hacia adelante
    public float rollCooldown = 0.5f; // Tiempo m�nimo entre rolls consecutivos
    public float rollDuration = 0.3f; // Duraci�n del roll

    [Header("Dodge Settings")]
    public float dodgeDistance = 2f; // Distancia del esquive hacia atr�s

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isRolling = false; // Controla si el jugador est� en estado de roll
    private float lastRollTime = 0; // Tiempo del �ltimo roll realizado
    private int shiftPressCount = 0; // Contador para doble presionar Shift
    private float lastShiftPressTime = 0; // Tiempo de la �ltima presi�n de Shift

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HandleRollAndDodge();
    }

    private void HandleRollAndDodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            float currentTime = Time.time;

            // Verificar si es una segunda presi�n r�pida para realizar el roll
            if (currentTime - lastShiftPressTime < 0.3f) // Doble presi�n detectada
            {
                shiftPressCount++;
                if (shiftPressCount == 2)
                {
                    Roll(); // Realizar el roll
                    shiftPressCount = 0; // Reiniciar el contador
                }
            }
            else
            {
                shiftPressCount = 1; // Primera presi�n
                Invoke(nameof(Dodge), 0.3f); // Si no hay segunda presi�n en 0.3 segundos, se esquiva
            }

            lastShiftPressTime = currentTime; // Actualizar el tiempo de la �ltima presi�n
        }
    }

    private void Roll()
    {
        if (Time.time - lastRollTime < rollCooldown) return; // Enfriamiento del roll

        isRolling = true;
        animator.SetTrigger("Roll"); // Activar animaci�n de rodar

        // Mover al jugador hacia adelante en la direcci�n actual
        float rollDirection = spriteRenderer.flipX ? -1 : 1; // Determinar la direcci�n
        rb.velocity = new Vector2(rollDirection * rollDistance / rollDuration, rb.velocity.y);

        lastRollTime = Time.time; // Registrar el tiempo del �ltimo roll
        Invoke(nameof(StopRoll), rollDuration); // Detener el roll despu�s del tiempo especificado
    }

    private void Dodge()
    {
        if (isRolling || shiftPressCount == 2) return; // No esquivar si est� rodando o si se detect� un roll

        animator.SetTrigger("Dodge"); // Activar animaci�n de esquivar

        // Mover al jugador hacia atr�s
        float dodgeDirection = spriteRenderer.flipX ? 1 : -1; // Direcci�n contraria al roll
        rb.velocity = new Vector2(dodgeDirection * dodgeDistance / rollDuration, rb.velocity.y);

        Invoke(nameof(StopDodge), rollDuration); // Detener el movimiento despu�s de un tiempo corto
    }

    private void StopRoll()
    {
        isRolling = false;
        rb.velocity = Vector2.zero; // Detener movimiento
    }

    private void StopDodge()
    {
        rb.velocity = Vector2.zero; // Detener movimiento despu�s del esquive
    }
}
