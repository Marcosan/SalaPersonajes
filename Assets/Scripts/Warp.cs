using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class Warp : MonoBehaviour{
    //Para almacenar el punto de destino
    public GameObject target;

    //Para almacenar el mapa de destino
    //Tener en cuenta que se seleccionan los GameObjects (instancias de los mapas), no los mapas en si
    public GameObject targetMap;

    /* Direccion hacia donde debe ver el jugador al pasar un Warp
    * 'U' UP
    * 'D' DOWN
    * 'L' LEFT
    * 'R' RIGHT
    */
    public char dir;
    
    // Para controlar si empieza o no la transición
    bool start = false;
    // Para controlar si la transición es de entrada o salida
    bool isFadeIn = false;
    // Opacidad inicial del cuadrado de transición
    float alpha = 0;
    // Transición de 1 segundo
    float fadeTime = 1f;

    Text tittleMiniMap;

    GameObject area;

    // Para poder usarlo externamente
    public static Warp instance = null;

    private void Awake(){
        //Para asegurar que el target se ha establecido o lanza un except
        Assert.IsNotNull(target);
        Assert.IsNotNull(targetMap);

        //Para esconder las imagenes del warp
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        //Se busca el area con el texto (UI)
        area = GameObject.FindGameObjectWithTag("Area");

        instance = this;

    }

    IEnumerator OnTriggerEnter2D(Collider2D col){

        if (col.tag == "Player"){

            SoundManager.SetClip("W");
            
            col.GetComponent<Animator>().enabled = false;
            col.GetComponent<Player>().enabled = false;
            
            FadeIn();

            yield return new WaitForSeconds(fadeTime);

            col.transform.position = target.transform.GetChild(0).transform.position;

            FadeOut();
            col.GetComponent<Animator>().enabled = true;
            col.GetComponent<Player>().enabled = true;

            StartCoroutine(area.GetComponent<Area>().ShowArea(targetMap.name,1f));

            //SingletonVars.Instance.nameCurrentMap = targetMap.name;
            //tittleMiniMap = GameObject.Find("/Area/MiniMap/TitleMap/TitleText").transform.GetComponent<Text>();
            //tittleMiniMap.text = SingletonVars.Instance.SetNameCurrentMap(targetMap.name);
            // Actualizamos la cámara
            Camera.main.GetComponent<MainCamera>().SetBound(targetMap);

            // Esto hara que apunte hacia una direccion luego de pasar a travez de un warp 
            setDirection(col.GetComponent<Animator>(), dir);
            
            JsonManager.setLastMap(targetMap.name);

        }

    }


    // Dibujaremos un cuadrado con opacidad encima de la pantalla simulando una transición
    void OnGUI(){

        // Si no empieza la transición salimos del evento directamente
        if (!start)
            return;

        // Si ha empezamos creamos un color con una opacidad inicial a 0
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        // Creamos una textura temporal para rellenar la pantalla
        Texture2D tex;
        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();

        // Dibujamos la textura sobre toda la pantalla
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);

        // Controlamos la transparencia
        if (isFadeIn){
            // Si es la de aparecer le sumamos opacidad
            alpha = Mathf.Lerp(alpha, 1.1f, fadeTime * Time.deltaTime);
        }
        else{
            // Si es la de desaparecer le restamos opacidad
            alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);
            // Si la opacidad llega a 0 desactivamos la transición
            if (alpha < 0) start = false;
        }

    }

    // Método para activar la transición de entrada
    void FadeIn(){
        start = true;
        isFadeIn = true;
    }

    // Método para activar la transición de salida
    void FadeOut(){
        isFadeIn = false;
    }

    void setDirection(Animator anim, char dir ) {
        // Este es por defecto
        if (dir.Equals(null)) {
            dir = 'U';
        }
        if ( dir.Equals('U') )
        {
            anim.SetFloat("moveY", +1);
        }
        else if ( dir.Equals('D') )
        {
            anim.SetFloat("moveY", -1);
        }
        else if ( dir.Equals('L') )
        {
            anim.SetFloat("moveX", -1);
        }
        else if ( dir.Equals('R') )
        {
            anim.SetFloat("moveX", +1);
        }
    }
}