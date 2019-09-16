using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLightScript : MonoBehaviour {

    private Animator animacion;

    private void Start()
    {
        animacion = GetComponent<Animator>();
    }
    private void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            animacion.SetFloat("Entrada", 1);
            animacion.SetFloat("Salida", 0);


            // En este punto se activa el boton de Accion
            changer.StartAction();

        }
    }   

private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            animacion.SetFloat("Estadia", 1);


            // En este punto se activa el boton de Accion
            changer.StartAction();
        }
    }

 void OnTriggerExit2D(Collider2D collision)
 {
        if (collision.gameObject.tag == "Player")
        {
            
            animacion.SetFloat("Salida", 1);
            animacion.SetFloat("Entrada", 0);
            animacion.SetFloat("Estadia", 0);
            // En este punto se desactiva el boton de Accion
            changer.DisableButton();
        }
        
 }
}
