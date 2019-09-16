using SuperTiled2Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour{

    Transform target;
    float tLX, tLY, bRX, bRY; //TopLeftX TopLeftY BottomRightX BottomRightY

    float posX, posY;
    Vector2 velocity;

    private bool isCameraControlling = true;

    // Para controlar si empieza o no la transición
    bool start = true;
    // Opacidad inicial del cuadrado de transición
    float alpha = 1;
    // Transición de 1 segundo
    float fadeTime = 1f;
    GameObject area;

    void Awake(){
        target = GameObject.FindGameObjectWithTag("Player").transform;
        area = GameObject.FindGameObjectWithTag("Area");
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, tLX, bRX),
            Mathf.Clamp(target.position.y, bRY, tLY),
            transform.position.z);
    }

    private void Start(){
        //Screen.SetResolution(800, 800, true);
        StartCoroutine(StartFade());
    }

    void LateUpdate(){
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, tLX, bRX),
            Mathf.Clamp(target.position.y, bRY, tLY),
            transform.position.z);
    }

    public void SetBound (GameObject map){
        //atributos del mapa tiled pasado como parametro map
        //Tiled2Unity.TiledMap config = map.GetComponent<Tiled2Unity.TiledMap>();
        //cantidad de celdas que tiene la camara, por lo general 5
        SuperMap config = map.GetComponent<SuperMap>();
        float cameraSize = Camera.main.orthographicSize;

        tLX = map.transform.position.x + cameraSize;
        tLY = map.transform.position.y - cameraSize;
        //bRX = map.transform.position.x + config.NumTilesWide - cameraSize;
        bRX = map.transform.position.x + config.m_Width - cameraSize;
        bRY = map.transform.position.y - config.m_Height + cameraSize;

        //FastMove();
    }

    public void FastMove(){
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );
        
    }

    IEnumerator StartFade() {
        target.GetComponent<Animator>().enabled = false;
        target.GetComponent<Player>().enabled = false;

        yield return new WaitForSeconds(fadeTime);

        target.GetComponent<Animator>().enabled = true;
        target.GetComponent<Player>().enabled = true;

        if(area != null)
            StartCoroutine(area.GetComponent<Area>().ShowArea("Temporal",1f));
    }

    // Dibujaremos un cuadrado con opacidad encima de la pantalla simulando una transición
    void OnGUI(){
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        // Creamos una textura temporal para rellenar la pantalla
        Texture2D tex;
        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();

        // Dibujamos la textura sobre toda la pantalla
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);

        // Le restamos opacidad
        alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);
        // Si la opacidad llega a 0 desactivamos la transición
        if (alpha < 0) start = false;

    }
    
    public bool getisCameraControlling() {
        return this.isCameraControlling;
    }

    public void setIsCameraControlling(bool control) {
        this.isCameraControlling = control;
    }
}
