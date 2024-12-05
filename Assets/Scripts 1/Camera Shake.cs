using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShakeCinemachine : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin noise; // Referencia al componente de ruido
    private CinemachineVirtualCamera virtualCamera; // Referencia a la c�mara virtual

    private void Start()
    {
        // Obtener la c�mara virtual
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // Obtener el componente de ruido
        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    public IEnumerator Shake(float duration, float amplitude, float frequency)
    {
        if (noise != null)
        {
            // Configurar la intensidad y frecuencia del temblor
            noise.m_AmplitudeGain = amplitude;
            noise.m_FrequencyGain = frequency;

            yield return new WaitForSeconds(duration);

            // Restaurar los valores a 0 despu�s del temblor
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
    }
}
