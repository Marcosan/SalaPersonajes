using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    // Variables para gestionar el radio de visión, el de ataque y la velocidad
    public float visionRadius;
    public float attackRadius;
    public float speed;

    // Variable para guardar al jugador
    GameObject player;


    // Variable para guardar la posición inicial del enemigo 
    Vector3 initialPosition;

    // Animador y cuerpo cinemático(importante por que dinamico se choca con todo y estatico no se mueve)
    //con la rotación en Z congelada por que sino se pone a dar vueltas 
    Animator anim;
    Rigidbody2D rb2d;

    //variables que vamos a utilizar en diferentes funciones por eso son globales
    Vector3 target;
    RaycastHit2D hit;
    Vector3 forward;
    float distance;
    Vector3 dir;

    //Variables utilizadas para el attack
    //prefeab del proyectil 
    public GameObject proyectilPrefab;
    //volocidad del ataque 
    public float attackSpeed = 2f;
    bool attacking;


    //variables para saber si huye o persigue
    public bool isChaser = false;

    // Start is called before the first frame update
    void Start()
    {
        //Se inicializa el player recuperandolo con el tag
        player = GameObject.FindGameObjectWithTag("Player");

        // Guardamos nuestra posición inicial
        initialPosition = transform.position;

        //recuperamos la animation y el cuerpo cinemático del enemigo
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    

    }

    // Update is called once per frame
    void Update()
    {
        UpdateRayCast();
    }


    void ChaseMode() {

        // Calculamos la distancia que hay entre el enemy y el player para poder saber cuanto y a donde movernos
        //desde el target(position inical) o la position del player si entramos en lo de hit hasta position actual
        distance = Vector3.Distance(target, transform.position);
        //guardamos la direction a la que está mirando para luego darle direcion al rigidbody pero lo normalizamos
        dir = (target - transform.position).normalized;

        // Si es el enemigo está en rango de ataque nos paramos y le atacamos
        // target != initialPosition es por que el traget ya es el player
        // distance < attackRadius distancia calculada menor que el radio de ataque 
        if (target != initialPosition && distance < attackRadius)
        {
            // Aquí le atacaríamos, pero por ahora simplemente cambiamos la animación
            anim.SetFloat("moveX", dir.x);
            anim.SetFloat("moveY", dir.y);
            anim.Play("NpcWalk", -1, 0);  // Congela la animación de andar, velocidad de animacion 0

            //Comenzamos con el attack (Poner el Layer en attack por el raycast)
            Debug.Log(attacking);
            if (!attacking)
                //llamamos a nustra corrutina y le pasamos la velocidad de ataque
                StartCoroutine(Attack(attackSpeed));

        }
        else
        {
            //movemos a nuestro enemy donde el player poniendole la dir que fue el vector de direccion calculado
            rb2d.MovePosition(transform.position + dir * speed * Time.deltaTime);
            // Al movernos establecemos la animación de movimiento, velocidad de animacion se vuelve a 1
            anim.speed = 1;
            anim.SetFloat("moveX", dir.x);
            anim.SetFloat("moveY", dir.y);
            anim.SetBool("walking", true);
        }

        // Una última comprobación para evitar bugs forzando la posición inicial
        if (target == initialPosition && distance < 0.02f)
        {
            transform.position = initialPosition;
            // Y cambiamos la animación de nuevo a Idle
            anim.SetBool("walking", false);
            //orderPoint();
        }

        if (transform.position != initialPosition && distance > visionRadius)
        {
            // Y cambiamos la animación de nuevo a Idle
            anim.SetBool("walking", false);
            //orderPoint();
        }


        // Y un debug optativo con una línea hasta el target
        Debug.DrawLine(transform.position, target, Color.green);


    }

        void UpdateRayCast(){
        // Por defecto nuestro target siempre será nuestra posición inicial
        target = initialPosition;

        // Comprobamos un Raycast del enemigo hasta el jugador
        hit = Physics2D.Raycast(
            transform.position,
            player.transform.position - transform.position,
            visionRadius,
            //necesitamos esto para que el enemigo no tenga problemas sino solamente detecte al player 
            1 << LayerMask.NameToLayer("Default")
            );
        // Poner el propio Enemy en una layer distinta a Default para evitar el raycast
        // También poner al objeto Attack y al Prefab Slash una Layer Attack 
        // Sino los detectará como entorno y se mueve atrás al hacer ataques

        // forward longitud de un vector que es rango de mirada hacia el jugador
        forward = transform.TransformDirection(player.transform.position - transform.position);
        // posicion desde la position del enemy hasta el player  
        Debug.DrawRay(transform.position, forward, Color.red);

        // hit collader es un gameObject por lo que para detectar una colision 
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                //Se convierte en perseguidor
                if (isChaser == true) { 
                 // persigue al player
                target = player.transform.position;
                    ChaseMode();
                 }
                
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    //se llamara cada 2 segundos osea genera un ataque cada 2 seconds
    IEnumerator Attack(float segundos)
    {
        attacking = true;//activamos el ataque

        if ( target != initialPosition && proyectilPrefab != null) {
            Instantiate(proyectilPrefab, transform.position, transform.rotation);
            // esperamos antes de hacer otro ataque
            yield return new WaitForSeconds(segundos);

        }
        attacking = false;
    }




}
