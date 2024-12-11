using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

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

    private NavMeshAgent agent;

    public float rangoVision = 15f;

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");

        if (ani == null)
            Debug.LogError("El Animator no está asignado.");
        if (target == null)
            Debug.LogError("El objeto con tag 'Player' no está en la escena.");

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError("El NavMeshAgent no está asignado.");
    }

    public void Comportamiento_Boss()
    {
        if (target != null && rango != null)
        {
            float distanciaJugador = Vector3.Distance(transform.position, target.transform.position);

            if (distanciaJugador < rangoVision)
            {
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                point.transform.LookAt(target.transform.position);

                if (distanciaJugador > 1 && !atacando)
                {
                    switch (rutina)
                    {
                        case 0:
                            agent.SetDestination(target.transform.position);
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

                        case 1:
                            agent.SetDestination(target.transform.position);
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                            ani.SetBool("Walk", false);
                            ani.SetBool("Run", true);
                            ani.SetBool("Attack", false);

                            cronometro += 1 * Time.deltaTime;
                            if (cronometro > time_rutinas / 2)
                            {
                                rutina = Random.Range(1, 5);
                                cronometro = 0;
                            }
                            break;

                        case 2:
                            ani.SetBool("Walk", false);
                            ani.SetBool("Run", false);
                            ani.SetBool("Attack", true);
                            ani.SetFloat("Skill", 0.8f);
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                            rango.GetComponent<CapsuleCollider>().enabled = false;
                            break;

                        case 3:
                            if (fase == 2)
                            {
                                jump_distance += 1 * Time.deltaTime;
                                ani.SetBool("Walk", false);
                                ani.SetBool("Run", false);
                                ani.SetBool("Attack", true);
                                ani.SetFloat("Skill", 0.6f);
                                hit_Select = 3;
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
                                    if (transform.position.y <= 0.5f)
                                    {
                                        ani.SetBool("Attack", false);
                                        rutina = 0;
                                        cronometro = 0;
                                    }
                                }
                            }
                            else
                            {
                                rutina = 0;
                                cronometro = 0;
                            }
                            break;

                        case 4:
                            if (fase == 2)
                            {
                                ani.SetBool("Walk", false);
                                ani.SetBool("Run", false);
                                ani.SetBool("Attack", true);
                                ani.SetFloat("Skill", 1f);
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

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("P3"))
        {
            WeaponAttack weapon = coll.GetComponent<WeaponAttack>();
            if (weapon != null && weapon.isAttacking)
            {
                HP_Min -= weapon.daño; // Aplicar daño
                if (HP_Min <= 0)
                {
                    HP_Min = 0; // Asegurar que la vida no sea negativa
                    ani.SetTrigger("Dead"); // Activar animación de muerte
                    muerto = true; // Cambiar el estado de muerte a verdadero
                    Debug.Log("¡Jefe derrotado!");
                }
                Debug.Log("¡Daño recibido por la espada!");
            }
        }
    }
}