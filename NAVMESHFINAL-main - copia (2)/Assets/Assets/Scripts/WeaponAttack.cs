using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public float daño = 50f; // Daño que hace la espada
    public bool isAttacking = false; // Para saber si el jugador está atacando
    private Collider swordCollider; // El collider de la espada

    void Start()
    {
        swordCollider = GetComponent<Collider>(); // Obtener el collider de la espada
        swordCollider.enabled = false; // Desactivar el collider al inicio
    }

    // Método que se activa al colisionar con el collider del jefe
    void OnTriggerEnter(Collider coll)
    {
        if (isAttacking && coll.CompareTag("Boss")) // Verificar si el jugador está atacando
        {
            // Obtener el componente del jefe y aplicar el daño
            BossController boss = coll.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(daño); // Pasar el daño al jefe
                Debug.Log("¡Daño aplicado al jefe!");
            }
        }
    }

    // Método para iniciar el ataque (lo puedes llamar desde otro script cuando se inicia el ataque)
    public void StartAttack()
    {
        isAttacking = true;
        swordCollider.enabled = true; // Activar el collider cuando el ataque comience
    }

    // Método para finalizar el ataque (puedes usarlo para cuando se termine el ataque de la espada)
    public void EndAttack()
    {
        isAttacking = false;
        swordCollider.enabled = false; // Desactivar el collider cuando el ataque termine
    }
}
