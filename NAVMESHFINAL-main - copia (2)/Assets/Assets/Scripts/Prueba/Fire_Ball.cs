using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Ball : MonoBehaviour
{
    // Declarar cronometro como variable miembro
    private float cronometro;

    // Start is called before the first frame update
    void Start()
    {
        cronometro = 0; // Inicialización opcional
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 6 * Time.deltaTime);
        transform.localScale += new Vector3(3, 3, 3) * Time.deltaTime;

        cronometro += 1 * Time.deltaTime;

        if (cronometro > 1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
            gameObject.SetActive(false);
            cronometro = 0; // Reiniciar cronometro
        }
    }
}
