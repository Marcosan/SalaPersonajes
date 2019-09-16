using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour{

    // Para controlar si empieza o no la transición
    bool start = false;
    // Para controlar si la transición es de entrada o salida
    bool isFadeIn = false;
    // Opacidad inicial del cuadrado de transición
    float alpha = 0;
    // Transición de 1 segundo
    float fadeTime = 1f;

    public string nameScene;

    GameObject area;

    private void Awake(){
        //Para esconder las imagenes del warp
        GetComponent<SpriteRenderer>().enabled = false;
        //transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        //Se busca el area con el texto (UI)
        area = GameObject.FindGameObjectWithTag("Area");
    }

    IEnumerator OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Player"){

            SoundManager.SetClip("W");

            col.GetComponent<Animator>().enabled = false;
            col.GetComponent<Player>().enabled = false;

            FadeIn();

            yield return new WaitForSeconds(fadeTime);

            SceneManager.LoadScene(nameScene);
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

    }

    // Método para activar la transición de entrada
    void FadeIn(){
        start = true;
        isFadeIn = true;
    }    
}
