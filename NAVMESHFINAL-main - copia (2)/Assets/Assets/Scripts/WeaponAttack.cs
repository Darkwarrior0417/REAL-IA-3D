using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public float da�o = 50f; // Da�o que hace la espada
    public bool isAttacking = false; // Para saber si el jugador est� atacando
    private Collider swordCollider; // El collider de la espada

    void Start()
    {
        swordCollider = GetComponent<Collider>(); // Obtener el collider de la espada
        swordCollider.enabled = false; // Desactivar el collider al inicio
    }

    // M�todo que se activa al colisionar con el collider del jefe
    void OnTriggerEnter(Collider coll)
    {
        if (isAttacking && coll.CompareTag("Boss")) // Verificar si el jugador est� atacando
        {
            // Obtener el componente del jefe y aplicar el da�o
            BossController boss = coll.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(da�o); // Pasar el da�o al jefe
                Debug.Log("�Da�o aplicado al jefe!");
            }
        }
    }

    // M�todo para iniciar el ataque (lo puedes llamar desde otro script cuando se inicia el ataque)
    public void StartAttack()
    {
        isAttacking = true;
        swordCollider.enabled = true; // Activar el collider cuando el ataque comience
    }

    // M�todo para finalizar el ataque (puedes usarlo para cuando se termine el ataque de la espada)
    public void EndAttack()
    {
        isAttacking = false;
        swordCollider.enabled = false; // Desactivar el collider cuando el ataque termine
    }
}
