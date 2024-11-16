using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float vidaMaxima = 100f;
    [SerializeField] private float vidaActual;

    private void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void TomarDano(float dano)
    {
        vidaActual -= dano;

        // Mostrar efecto de daño o retroceso aquí, si lo deseas

        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        Debug.Log("Jugador Muerto");
        // Aquí puedes manejar la lógica de Game Over o reiniciar la escena
    }
}
