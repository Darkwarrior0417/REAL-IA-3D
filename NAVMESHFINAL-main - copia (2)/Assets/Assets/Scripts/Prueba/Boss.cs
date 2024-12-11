using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  // Necesario para el NavMesh
using UnityEngine.UI;  // Añade esto para acceder a la clase Image

public class Boss : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public float time_rutinas;
    public Animator ani;
    public Quaternion angulo;
    public float grado;
    public GameObject target;
    public bool atacando;
    public RangoBoss rango;
    public float speed = 2.0f;
    public GameObject[] hit;
    public int hit_Select;

    public bool lanzallamas;
    public List<GameObject> pool = new List<GameObject>();
    public GameObject fire;
    public GameObject cabeza;
    public float cronometro2;

    public float jump_distance;
    public bool direction_skill;

    public GameObject fire_ball;
    public GameObject point;
    public List<GameObject> pool2 = new List<GameObject>();

    public int fase = 1;
    public float HP_Min;
    public float HP_Max;
    public Image barra;
    public bool muerto;

    private NavMeshAgent agent;  // Agregamos el NavMeshAgent

    // Rango de visión del jefe
    public float rangoVision = 15f;  // Distancia máxima para que el jefe detecte al jugador

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");

        // Comprobación de null
        if (ani == null)
            Debug.LogError("El Animator no está asignado.");

        if (target == null)
            Debug.LogError("El objeto con tag 'Player' no está en la escena.");

        // Obtener el NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("El NavMeshAgent no está asignado.");
        }
    }

    public void Comportamiento_Boss()
    {
        if (target != null && rango != null)
        {
            // Comprobamos si el jugador está dentro del rango de visión
            float distanciaJugador = Vector3.Distance(transform.position, target.transform.position);

            if (distanciaJugador < rangoVision)
            {
                // Si está dentro del rango de visión, empieza a contar para las rutinas
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                point.transform.LookAt(target.transform.position);

                if (distanciaJugador > 1 && !atacando)
                {
                    switch (rutina)
                    {
                        case 0:
                            // Usamos el NavMeshAgent para mover al Boss
                            agent.SetDestination(target.transform.position); // Mover al jugador
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                            ani.SetBool("Walk", true);
                            ani.SetBool("Run", false);
                            ani.SetBool("Attack", false);

                            cronometro += 1 * Time.deltaTime;
                            if (cronometro > time_rutinas)
                            {
                                rutina = Random.Range(1, 5);
                                cronometro = 0;
                            }
                            break;

                        case 1: // Run
                            agent.SetDestination(target.transform.position); // Mover al jugador
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                            ani.SetBool("Walk", false);
                            ani.SetBool("Run", true);
                            ani.SetBool("Attack", false);
                            cronometro += 1 * Time.deltaTime;
                            if (cronometro > time_rutinas / 2)  // Reduce el tiempo de "dash" aquí
                            {
                                rutina = Random.Range(1, 5);  // Cambia a una nueva rutina
                                cronometro = 0;
                            }
                            break;

                        case 2: // Ataque de llamas
                            ani.SetBool("Walk", false);
                            ani.SetBool("Run", false);
                            ani.SetBool("Attack", true);
                            ani.SetFloat("Skill", 0.8f);
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                            if (rango != null)
                                rango.GetComponent<CapsuleCollider>().enabled = false;
                            break;

                        case 3: // Ataque de salto
                            if (fase == 2)
                            {
                                jump_distance += 1 * Time.deltaTime;
                                ani.SetBool("Walk", false);
                                ani.SetBool("Run", false);
                                ani.SetBool("Attack", true);
                                ani.SetFloat("Skill", 0.6f);
                                hit_Select = 3;
                                if (rango != null)
                                    rango.GetComponent<CapsuleCollider>().enabled = false;

                                if (direction_skill)
                                {
                                    if (jump_distance < 1f)
                                    {
                                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                                    }

                                    transform.Translate(Vector3.forward * 8 * Time.deltaTime);
                                }
                                else
                                {
                                    // Lógica de aterrizaje
                                    if (transform.position.y <= 0.5f) // Ajusta este valor según la altura del salto
                                    {
                                        // El jefe ha aterrizado
                                        ani.SetBool("Attack", false); // Detén la animación de ataque de salto
                                        rutina = 0; // Cambia a la siguiente rutina
                                        cronometro = 0; // Reinicia el cronómetro
                                    }
                                }
                            }
                            else
                            {
                                rutina = 0;
                                cronometro = 0;
                            }
                            break;

                        case 4: // Lanzar bolas de fuego
                            if (fase == 2)
                            {
                                ani.SetBool("Walk", false);
                                ani.SetBool("Run", false);
                                ani.SetBool("Attack", true);
                                ani.SetFloat("Skill", 1f);
                                if (rango != null)
                                    rango.GetComponent<CapsuleCollider>().enabled = false;
                                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 0.5f);
                            }
                            else
                            {
                                rutina = 0;
                                cronometro = 0;
                            }
                            break;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("target o rango no están asignados correctamente.");
        }
    }

    public void Final_Ani()
    {
        rutina = 0;
        ani.SetBool("Attack", false);
        atacando = false;
        if (rango != null)
        rango.GetComponent<CapsuleCollider>().enabled = true;
        lanzallamas = false;
        jump_distance = 0;
        direction_skill = false;
    }

    public void Direction_Attack_Start()
    {
        direction_skill = true;
    }

    public void Direction_Attack_Final()
    {
        direction_skill = false;
    }

    public void ColliderWeaponTrue()
    {
        if (hit_Select >= 0 && hit_Select < hit.Length)
            hit[hit_Select].GetComponent<SphereCollider>().enabled = true;
    }

    public void ColliderWeaponFalse()
    {
        if (hit_Select >= 0 && hit_Select < hit.Length)
            hit[hit_Select].GetComponent<SphereCollider>().enabled = false;
    }

    public GameObject GetBala()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        GameObject obj = Instantiate(fire, cabeza.transform.position, cabeza.transform.rotation) as GameObject;
        pool.Add(obj);
        return obj;
    }

    public void LanzaLlamas_Skill()
    {
        cronometro2 += 1 * Time.deltaTime;
        if (cronometro2 > 0.1f)
        {
            GameObject obj = GetBala();
            obj.transform.position = cabeza.transform.position;
            obj.transform.rotation = cabeza.transform.rotation;
            cronometro2 = 0;
        }
    }

    public void Start_Fire()
    {
        lanzallamas = true;
    }

    public void Stop_Fire()
    {
        lanzallamas = false;
    }

    public GameObject Get_Fire_Ball()
    {
        for (int i = 0; i < pool2.Count; i++)
        {
            if (!pool2[i].activeInHierarchy)
            {
                pool2[i].SetActive(true);
                return pool2[i];
            }
        }
        GameObject obj = Instantiate(fire_ball, point.transform.position, point.transform.rotation) as GameObject;
        pool2.Add(obj);
        return obj;
    }

    public void Fire_Ball_Skill()
    {
        GameObject obj = Get_Fire_Ball();
        obj.transform.position = point.transform.position;
        obj.transform.position = point.transform.position;
    }

    public void Vivo()
    {
        if (HP_Min < 500)
        {
            fase = 2;
            time_rutinas = 1;
        }

        Comportamiento_Boss();

        if (lanzallamas)
        {
            LanzaLlamas_Skill();
        }
    }

    void Update()
    {
        if (barra != null)
            barra.fillAmount = HP_Min / HP_Max;

        if (HP_Min > 0)
        {
            Vivo();
        }
        else if (!muerto)
            { 
            ani.SetTrigger("Dead");
            muerto = true;
            }
        
    }
}
