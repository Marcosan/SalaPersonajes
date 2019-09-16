using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Controlador de animación del portal de uso general, indica las señales de entrada, salida, estadía
*/

public class PortalScript : MonoBehaviour {
/*El primer void Start solo toma el componente de Animator referente al objeto, en este caso, el portal
*/
    private Animator animacion;
    private void Start()
    {
        animacion = GetComponent<Animator>();
    }
    private void Update()
    {
    }
    /*TriggerEnter verifica si el objeto que entra es el jugador, y cambia las variables de Entrada 
     * y Salida, usadas para el Animator*/
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animacion.SetFloat("Entrada", 1);
            animacion.SetFloat("Salida", 0);
           // Debug.Log("Esta entrando!");
        }
}
    /*TriggerStay verifica si el objeto que está dentro del campo es el jugador, 
     * y cambia las variables de Estadía */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animacion.SetFloat("Estadia", 1);
            //Debug.Log("Permanece!");
        }
    }
    /*Exit se asegura que el objeto que sale sea el jugador y sólo con él se ejecute la animacion
     respectiva del portal*/
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animacion.SetFloat("Salida", 1);
            animacion.SetFloat("Entrada", 0);
            animacion.SetFloat("Estadia", 0);
           // Debug.Log("Se va!!");

        }
    }
    
    



}
