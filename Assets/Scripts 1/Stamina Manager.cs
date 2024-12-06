using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f; // Cantidad m�xima de estamina
    public float currentStamina; // Estamina actual
    public float regenRate = 10f; // Cantidad de estamina que se regenera por segundo
    public float delayBeforeRegen = 1f; // Tiempo de espera antes de comenzar a regenerar

    [Header("UI Settings")]
    public Slider staminaSlider; // Referencia al Slider de estamina en el UI

    private bool isRegenerating = false; // Controla si la regeneraci�n est� activa
    private float regenCooldown; // Temporizador para la regeneraci�n

    private void Start()
    {
        currentStamina = maxStamina; // Inicializa la estamina al m�ximo

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    private void Update()
    {
        // Manejar la regeneraci�n de estamina
        if (!isRegenerating && currentStamina < maxStamina)
        {
            regenCooldown -= Time.deltaTime;
            if (regenCooldown <= 0)
            {
                isRegenerating = true;
            }
        }

        if (isRegenerating && currentStamina < maxStamina)
        {
            RegenerateStamina();
        }

        // Actualizar la UI
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }

    public bool ConsumeStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            isRegenerating = false; // Detener regeneraci�n temporalmente
            regenCooldown = delayBeforeRegen; // Reiniciar el temporizador de regeneraci�n
            return true; // Indica que la acci�n fue posible
        }
        return false; // No hay suficiente estamina
    }

    private void RegenerateStamina()
    {
        currentStamina += regenRate * Time.deltaTime;
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina; // No exceder el m�ximo
            isRegenerating = false; // Detener regeneraci�n si est� llena
        }
    }
}
