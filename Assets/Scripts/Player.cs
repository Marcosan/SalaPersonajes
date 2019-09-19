using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour{
    Vector2 mov;
    Vector2 lastMov;
    Rigidbody2D rb2d;
    public float speed = 4f;
    private float originalSpeed;
    Animator anim;
    private bool CanMove = true;
    private int numChildren = 0;
    private static PlayerData PlData;
    //private static GlobalDataGame GmData;

    string ActiveScene;

    public GameObject initialMap;

    CircleCollider2D actionCollider;
    CircleCollider2D interCollider;

    bool interacting = false;
    private bool isActionButton = false;

    float offsetCeilingX = 0;
    float offsetCeilingY = 0;

    Text tittleMiniMap;

    // Joistick
    public Joystick joystick;

    /* Desde el player se carga la partida y los otros componentes necesarios, asi que por eso esta como objeto en el menu principal
    * de juego, para que no salgan errores por la falta de joystick y etc pondremos un booleano
    */
    public bool isMainMenu = false;

    private bool isMovingAlone = false;
    private float speedTmp = 4f;
    private Vector3 destinyAlone;

    private void Awake(){
        Assert.IsNotNull(initialMap);

        /* Para evitar mas carga desde el inicio puse esta instancia en Awake ya que de preferencia es
         * mejor dejar cargando los archivos mas pesados de a poco en vez de golpe como seria en start.
         */
        // Carga los datos guardados la ultima vez
        PlData = SaveSystem.LoadPlayer();
    }

    // Start is called before the first frame update
    void Start(){
        foreach (Transform t in transform){
            numChildren++;
        }

        originalSpeed = speed;

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        actionCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        interCollider = transform.GetChild(1).GetComponent<CircleCollider2D>();

        actionCollider.offset = interCollider.offset;
        actionCollider.radius = interCollider.radius;

        actionCollider.enabled = false;

        // Implementacion para el audio en el cambio de escena
        print("La escena actual es: " + SceneManager.GetActiveScene().name.ToString());
        SoundManager.ChangeMusic();

        // Para que la posicion inicial este como se guardo la ultima vez
        if (SaveSystem.wasLoaded && !SaveSystem.newGame)
        {
            /* Para cambiar de mapa a la posicion del jugador a la ultima guardada
            * Siempre que se carga la escena se destruyen los objetos que estaban anteriormente y se carga
            * como se configuro por defecto (A menos que se haga que se carguen desde un archivo JSON o etc,
            * pero eso hay que personalizar cada detalle que se quiera conservar de cada escenario o mapa y 
            * y como cargarlo). Para evitar este problema, esta funcion GameObject.Fint(string name) busca un
            * GameObject de la escena que tenga le nombre que se le mande. En este caso esta cargando el ultimo 
            * nombre guardado en Ajustes.json, el cual se actualiza cada que se cambia de mapa y se guarda al 
            * darle al boton de guardado. Por lo que no importa cuantas veces se destruya ya que siempre buscara
            * al que tenga ese nombre.
            */

            GameObject newTarget;

            if (GameObject.Find(JsonManager.gsettings.lastMapName))
            {
                newTarget = GameObject.Find(JsonManager.gsettings.lastMapName);
                //Para cambiar el titulo del minimapa
                SingletonVars.Instance.nameCurrentMap = newTarget.name;

                // Actualizamos la cámara con el mapa de la ultima vez antes de guardar
                Camera.main.GetComponent<MainCamera>().SetBound(newTarget);
            }
            else
            {
                //Para cambiar el titulo del minimapa
                SingletonVars.Instance.nameCurrentMap = initialMap.name;

                // Actualizamos la cámara con el mapa de la ultima vez antes de guardar
                Camera.main.GetComponent<MainCamera>().SetBound(initialMap);
            }

            // Para colocar al player en la ultima posicion guardada
            setPosition(PlData.GetX(), PlData.GetY(), PlData.GetZ());
            setPlayerDirection(PlData.GetMovement());

            SaveSystem.wasLoaded = false;

            JsonManager.setLastInitialMap(initialMap.name);

        }
        else if (isMainMenu) {

        }
        else {
            if(initialMap != null) { 
                //Para cambiar el titulo del minimapa
                SingletonVars.Instance.nameCurrentMap = initialMap.name;
                //tittleMiniMap = GameObject.Find("/Area/MiniMap/TitleMap/TitleText").transform.GetComponent<Text>();
                //tittleMiniMap.text = SingletonVars.Instance.SetNameCurrentMap(initialMap.name);
                //MainCamera es el script, se llama a la funcion SetBound creada allí, se pasa el mapa inicial
                Camera.main.GetComponent<MainCamera>().SetBound(initialMap);
            }
        }
        
    }

    // Update is called once per frame
    void Update(){

        if (!isMainMenu && CanMove)
        {
            if (isActionButton)
            {
                interacting = true;
            }
            else
            {
                interacting = false;
                actionCollider.enabled = false;
            }
            Movements();
            //MoveMentsJoyStick();

            Animations(mov);

            Interact();
        }
        if (isMovingAlone) {
            if (transform.position == destinyAlone){
                Debug.Log("Llegp al punto");
                GetComponent<BoxCollider2D>().enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
                MovePlayer(true);
                isMovingAlone = false;
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, destinyAlone, speedTmp * Time.deltaTime);
            
        }
    }
    
    private void FixedUpdate(){
        MoveCharacter();
    }

    void MoveMentsJoyStick() {
        if ((joystick.Horizontal > .2f || joystick.Horizontal < -.2f) ||
            (joystick.Vertical > .2f || joystick.Vertical < -.2f))
        {
            lastMov = new Vector2(joystick.Horizontal, joystick.Vertical);
            int angulo = (int)Angle(joystick.Direction);
            if (angulo >= 345 || angulo <= 15)
                angulo = 0;
            else if (angulo > 15 && angulo < 75)
                angulo = 60;
            else if (angulo >= 75 && angulo <= 105)
                angulo = 90;
            else if (angulo > 105 && angulo < 165)
                angulo = 120;
            else if (angulo >= 165 && angulo <= 195)
                angulo = 180;
            else if (angulo > 195 && angulo < 255)
                angulo = 240;
            else if (angulo >= 255 && angulo <= 285)
                angulo = 270;
            else if (angulo > 285 && angulo < 345)
                angulo = 300;

            float dirMagnitud = joystick.Direction.magnitude;
            double radians = Math.Round(angulo * (Math.PI / 180), 2, MidpointRounding.ToEven);
            float dirHorizontal = Convert.ToSingle(Math.Round(dirMagnitud * Math.Sin(radians), 2, MidpointRounding.ToEven));
            float dirVertical = Convert.ToSingle(Math.Round(dirMagnitud * Math.Cos(radians), 2, MidpointRounding.ToEven));
            mov = new Vector2(dirHorizontal, dirVertical);
        }
        else
            mov = Vector2.zero;
    }

    //Angulo con respecto a la vertical y en sentido horario
    public static float Angle(Vector2 p_vector2) {
        if (p_vector2.x < 0) {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        } else {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    void Movements(){
        //Devuelve 1 o -1 segun la direccion de las flechas, 0 si no se mueve uno de los ejes
        mov = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void Animations(Vector2 direction)
    {
        //Para dejar el la direccion a la que camine
        if (direction != Vector2.zero){ //Si el vector de movimiento es distinto de cero
            anim.SetFloat("moveX", direction.x);
            anim.SetFloat("moveY", direction.y);
            anim.SetBool("walking", true);
        } else{
            anim.SetBool("walking", false);
        }
    }

    void Interact(){
        //mov da 1 o -1, por lo que hay que dividir para 2 para el offset
        if (mov != Vector2.zero){
            offsetCeilingX = 0;
            offsetCeilingY = 0;
            float deltaOffset = .2f;
            if (mov.x < -deltaOffset) offsetCeilingX = ((float)Math.Floor(mov.x)) / 2;
            if (mov.y < -deltaOffset) offsetCeilingY = ((float)Math.Floor(mov.y)) / 2;

            if (mov.x > deltaOffset) offsetCeilingX = ((float)Math.Ceiling(mov.x)) / 2;
            if (mov.y > deltaOffset) offsetCeilingY = ((float)Math.Ceiling(mov.y)) / 2;
            
            actionCollider.offset = new Vector2(offsetCeilingX, offsetCeilingY);
            interCollider.offset = new Vector2(offsetCeilingX, offsetCeilingY);
        }

        if (interacting){
            actionCollider.enabled = true;
        }
    }
    
    public Vector2 getMov(){
        return this.mov;
    }

    public Vector2 getLastMov()
    {
        return this.lastMov;
    }

    public Animator getAnimator()
    {
        return this.anim;
    }

    public void setMov(Vector2 m) {
        this.mov = m;
    }

    // Obtener la ultima escena en la que estuvo el player
    public string GetLastScene() {
        return this.ActiveScene;
    }

    // Cambiar el valor booleano al boton
    // Ir a Assets/Scripts/ActionBtn/Changer.cs para ver implementacion.
    public void setIsActionButton(bool action) {
        this.isActionButton = action;
    }

    // Mover al jugador
    void MoveCharacter(){
        rb2d.MovePosition(rb2d.position + getMov() * speed * Time.deltaTime);
    }

    // Asignar al player la posicion indicada
    public void setPosition(float posX, float posY, float posZ) {

        Vector3 positionPlayer;
        positionPlayer.x = posX;
        positionPlayer.y = posY;
        positionPlayer.z = posZ;

        this.transform.position = positionPlayer;
    }

    // Asignar al player una orientacion (Hacia donde esta mirando)
    public void setPlayerDirection(Vector2 dir) {
        if (dir != Vector2.zero)
        {
            anim.SetFloat("moveX", dir.x);
            anim.SetFloat("moveY", dir.y);
        }

    }

    // Para guardar la partida
    public void SavePlayer() {
        // Asigna a una variable la escena antes de guardar.
        ActiveScene = SceneManager.GetActiveScene().name;
        
        //SaveSystem.SaveGame(this);

        SaveLastScene();

        // Guarda al player con los parametros actuales.
        SaveSystem.SavePlayer(this);

        // Serializa los cambios en JsonManager
        JsonManager.SerializeSettings();

        // Reproducir sonido de guardado
        SoundManager.SetClip("S");
    }

    public void LoadPlayer()
    {
        /* Por si en algun momento se lo vuelve a utilizar */
        //GetTheData();

        LoadLastScene();

        // Para cargar correctamente las escenas tienen que estar registradas en File>Build Settings
        // desde el editor de unity.

        // Nunca usar, hace que se mezclen escenas y los elementos no se eliminan y se crean unos sobre  otros
        //SceneManager.LoadScene(SaveSystem.LastScene,LoadSceneMode.Additive);

        // Forma correcta de cargar una escena asegurandose de eliminar escenas viejas con sus objetos anteriores
        SceneManager.LoadScene(SaveSystem.LastScene, LoadSceneMode.Single);

        // No estoy seguro si elimina las escenas anteriores y solo hace el cambio
        //SceneManager.LoadScene(SaveSystem.LastScene);

        // Para indicar que se acaba de cargar una escena
        SaveSystem.wasLoaded = true;

        // Para evitar que se noten los cambios de audio bruscamente
        SoundManager.BackgroundMusic.mute = true;

    }

    public void NewPlayer()
    {
        PlayerPrefs.DeleteAll();

        JsonManager.SetInitialDataJson();
        JsonManager.SerializeSettings();

        SaveSystem.LastScene = "Lobby";

        SceneManager.LoadScene(SaveSystem.LastScene, LoadSceneMode.Single);

        SaveSystem.wasLoaded = true;

        // Para evitar que se noten los cambios de audio bruscamente
        SoundManager.BackgroundMusic.mute = true;

    }

    public void LoadAboutUs()
    {

        SceneManager.LoadScene("AboutUs", LoadSceneMode.Single);
        SoundManager.BackgroundMusic.mute = true;

    }

    // No preguntes por que esta esto aqui, el editor me lo dio como solucion y no se por que no coge sin ponerlo en metodo
    // Trata de mandar todos los datos que estaban guardados en el GlobalDataGame en una variable de datos temporales en
    // Save System para evitar que se eliminen o sobreescriban datos que no quieres
    // Por ejemplo, que ya hayas utilizado una variable antes y al guardar como no lo cambiaste se haga nulo
    // >:v Solo hazlo no preguntes
    private static void GetTheData()
    {
        /* Obsoleto, pero se lo dejara como plantilla 
         * Esta Obsoleto nomas el valor que se guarda y carga en GlobalDataGame, se lo dejara como plantilla al igual que el PlayerData
         * (Que si esta en uso) para futuros usos. No esta de mas utilizar una serializacion, todo dependera siempre de los tipos de
         * datos que se requieran guardar. Ahora se empleara el uso de PlayerPrefs en caso de que los valores que se desean guardar
         * y cargar sean de datos como cadenas, enteros o flotantes
         */
        // Carga los datos guardados la ultima vez
        //GmData = SaveSystem.LoadGameData();
        //SaveSystem.LastScene = GmData.GetLastScene();
    }

    /* Se puede usar una clase llamada PlayerPrefs que guarda en un fichero a parte ciertos datos deseados, pero la desventaja es
     * que solo se aplica con tipos de datos simples como strings, enteros o flotantes.
     * De aqui se puede ver como en python, como si fueran diccionarios. El primer argumento es un key y el segundo un value
     * Por lo menos en los Sets
     * En los Gets el primer argumento sigue siendo un key, pero el segundo (Es opcional) sirve para poner un valor por defecto
     * en caso de no existir uno guardado previamente
     * Este tipo de guardado preferiblemente se deberia de utilizar para configuraciones, avances en los niveles, marcadores de puntos,
     * o en este caso se guarda y carga una cadena que seria el nombre que tiene la ultima escena en la que estaba el jugador. Como no 
     * guardare un objeto me conviene utilizarlo para mas facilidad.
     * Al contrario de la serializacion que sirve para guardar objetos masivos
     */
    void SaveLastScene() {
        // Para guardar en PlayerPrefs
        PlayerPrefs.SetString("lastScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        // Para guardar en JSON
        JsonManager.SetLastScene(SceneManager.GetActiveScene().name);
    }

    void LoadLastScene() {
        //SaveSystem.LastScene = PlayerPrefs.GetString("lastScene", SceneManager.GetActiveScene().name);
        SaveSystem.LastScene = JsonManager.gsettings.lastScene;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void PlayButton() {
        // Reproducir sonido del boton
        SoundManager.SetClip("M");
    }

    public void MovePlayer(bool move) {
        mov = new Vector2(0f,0f);
        this.CanMove = move;
    }

    public void MainMenu() {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    public void MoveAlone(Vector3 destiny, float speed) {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        MovePlayer(false);
        destinyAlone = destiny;
        speedTmp = speed;
        isMovingAlone = true;
    }

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }

    public void ResetToNormalSpeed() {
        speed = originalSpeed;
    }

    public void ApplyPhysicsPlatform(float moveForce, float maxSpeed) {
        GetComponent<Rigidbody2D>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, maxSpeed);
        GetComponent<Rigidbody2D>().AddForce(mov.normalized * moveForce);
    }

}
