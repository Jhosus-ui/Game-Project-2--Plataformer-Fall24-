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

        // Mostrar efecto de da�o o retroceso aqu�, si lo deseas

        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        Debug.Log("Jugador Muerto");
        // Aqu� puedes manejar la l�gica de Game Over o reiniciar la escena
    }
}
