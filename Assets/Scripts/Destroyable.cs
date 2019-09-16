using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script para crear un objeto destruible
public class Destroyable : MonoBehaviour {

    // Variable para guardar el nombre del estado de destruccion
    public string destroyState;
    // Variable con los segundos a esperar antes de desactivar la colisión
    public float timeForDisable;
    // Variable con el rango de vision
    public float visionRadius;

    // Animador para controlar la animación
    Animator anim;

    // Variable para guardar al jugador
    GameObject player;

    Vector3 forward;
    RaycastHit2D hit;

    void Start () {
        anim = GetComponent<Animator>();

        // Para recuperar al player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Detectamos la colisión con una corrutina
    IEnumerator OnTriggerEnter2D (Collider2D col) {

        // Si es un ataque
        if (col.tag == "Action")
        {
            SoundManager.SetClip("A");

            // Reproducimos la animación de destrucción y esperamos
            anim.Play(destroyState);
            yield return new WaitForSeconds(timeForDisable);

            // Pasados los segundos de espera desactivamos los colliders 2D
            foreach (Collider2D c in GetComponents<Collider2D>())
            {
                c.enabled = false;
            }

        }

    }

    // Podemos dibujar el radio de visión y ataque sobre la escena dibujando una esfera
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }

    private void OnDestroy()
    {
        changer.DisableButton();
    }

    void Update () {
        
        // "Destruir" el objeto al finalizar la animación de destrucción
        // El estado debe tener el atributo 'loop' a false para no repetirse
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        
        //stateInfo.normalizedTime >= 1 porque significa que termino la animacion, de [0,1]
        if (stateInfo.IsName(destroyState) && stateInfo.normalizedTime >= 1) {
            Destroy(gameObject);
            // En el futuro podríamos almacenar la instancia y su transform
            // para crearlos de nuevo después de un tiempo
        }

        // Raycast desde el jarron hasta el player
        hit = Physics2D.Raycast(
            transform.position,
            player.transform.position - transform.position,
            visionRadius,
            1 << LayerMask.NameToLayer("Default")
            // Poner el propio Enemy en una layer distinta a Default para evitar el raycast
            // También poner al objeto Attack y al Prefab Slash una Layer Attack 
            // Sino los detectará como entorno y se mueve atrás al hacer ataques
            );
       
         // Aquí podemos debugear el Raycast
         forward = transform.TransformDirection(player.transform.position - transform.position);
         Debug.DrawRay(transform.position, forward, Color.red);

         // Si el Raycast encuentra al jugador habilitamos el boton
         if (hit.collider != null)
         {
             if (hit.collider.tag == player.tag) {
                changer.StartAction();
             }
         }
         else
         {
            if (!changer.GetBusyBtn()) {
                changer.DisableButton();
            }
         }

    }
    
}