using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fondo : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimiento;

    private Vector2 offset;

    private Material material;

    private Rigidbody2D jugador;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        offset = (jugador.velocity.x * 0.1f) * velocidadMovimiento * Time.deltaTime; // determinar times 
        material.mainTextureOffset += offset; // examinar materiales y movilidad

    }

    // Don't give this script much importance, I'm just learning a little bit :)
}