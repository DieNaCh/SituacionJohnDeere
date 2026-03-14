using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage = 10f; // Puedes ponerle un valor por defecto aquí

    // Ya no necesitamos la variable "public tractorHelath pHealth" aquí arriba

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // 1. Verificamos si el objeto con el que chocamos tiene el tag "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // 2. Extraemos el script "tractorHelath" directamente del tractor
            tractorHelath pHealth = other.gameObject.GetComponent<tractorHelath>();

            // 3. Si el script existe, le restamos el daño
            if (pHealth != null)
            {
                pHealth.health -= damage;
            }
        }
    }
}