using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float damage; // Daño que causa el proyectil
    public float speed = 10f; // Velocidad del proyectil
    public float lifespan = 5f; // Tiempo de vida del proyectil

    private Vector3 direction;

    void Start()
    {
        // Aquí podrías establecer la dirección del proyectil hacia el jugador
        direction = transform.forward;
        Destroy(gameObject, lifespan); // Destruir el proyectil después de un tiempo
    }

    void Update()
    {
        // Mover el proyectil en la dirección indicada
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Método para establecer el daño del proyectil
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    // Método para detectar colisiones
    void OnCollisionEnter(Collision collision)
    {
        // Si el proyectil colisiona con el jugador, le causa daño
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Destruir el proyectil después de impactar
        }
    }
}
