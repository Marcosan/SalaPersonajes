using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    /** ATENCION!
     * Para que la implementacion se de correctamente en cada escena hay que agregar en la raiz al GameObject AudioCanvas que se encuentra en Prefabs.
     * *
     * Este script se encarga de asignarle audios de musica de fondo, audios para efectos (como interacciones).
     * *
     * Hay dos Audio Source para trabajos especificos ya que es mejor no tocar al que se encarga del audio de fondo para ponerle efectos y viceversa.
     * *
     * Percatarse de utilizar siempre PlayOneShot() en los efectos para que no se sobreponga la reproduccion del audio de fondo, en el caso
     * de hacerlo se notara que el audio de efecto sera el unico en reproduccion y estara en loop.
     * *
     * En AudioCanvas se encuentra el complemento Audio Source vacio, no es necesario rellenarlo ya que el script se encarga de configurarlo. Como ponerlo 
     * en loop, la intensidad del volumen, activarlo, etc.
     * *
     * Si se lo llena igualmente se van a sobreescribir los valores por los indicados en este script al iniciar.
     * *
     * En caso de no haber asignado una musica de fondo en especifico para una escena hay una implementacion de un audio por defecto, la misma escena puede
     * tener su propio audio agregando una condicion en SetBackground(). Pero Antes debe asegurarse de cambiar la longitud del Array desde el Prefabs de
     * AudioManager y arrastrar el audio deseado, a la escena en el caso de haberla creado debe ir al editor de unity y en File>Build Settings agregar la
     * escena nueva a la lista de escenas existentes. Desde el mismo lugar se indica a la derecha el numero de indice al que pertenecera en el array Escenas.
     **/


    public static AudioSource BackgroundMusic;      // Audio Source para referenciar a la musica de fondo.
    public static AudioSource efxSound;             // Audio Source para referenciar el sonido de fondo.

    public static SoundManager instance = null;    // Allows other scripts to call functions from SoundManager.     
    
    public AudioClip InteractSound;                 // Audio para interaccion
    public AudioClip ActionSound;                   // Audio para Accion
    public AudioClip WarpSound;                     // Audio para el Warp
    public AudioClip SaveSound;                     // Audio para el guardado
    public AudioClip MainButtonsSound;              // Audio para los botones del inicio

    public AudioClip[] musicList;                   // Array de AudioClips para agregar musica de fondo, se llaman desde AudioCanvas
    public string[] Escenas;                        // Array que almacena los nombres de todas las escenas existentes

    // Start is called before the first frame update
    void Start()
    {
        BackgroundMusic = GetComponent<AudioSource>();
        efxSound = GetComponent<AudioSource>();
        
        BackgroundMusic.volume = 0.4F;             // Volumen de 0.0 a 1.0 para la musica de fondo
        BackgroundMusic.loop = true;                // Para que se repita el audio
        BackgroundMusic.enabled = true;             // Para que se active de ser necesario
        SetBackground();                            // Para asignarle un audio dependiendo de la escena en la que se encuentra
        BackgroundMusic.mute = true;                // Para que no se note el pequeño margen de error cuando se ajusta el audio
    }

    private void Update()
    {
        // Si la musica no se esta reproduciendo desde aqui se verifica, asigna un audio, y reproduce nuevamente
        if (!BackgroundMusic.isPlaying)
        {
            SetBackground();
            if (BackgroundMusic.clip != null)
            {
                BackgroundMusic.Play();
            }
        }
        else {
        }
    }

    void Awake()
    {
        // Check if there is already an instance of SoundManager
        if (instance == null)
            // if not, set it to this.
            instance = this;
        // If instance already exists:
        else if (instance != this)
            // Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        // Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);

        // Carga el nombre de todas las escenas existentes, el mismo orden esta en File > Build Settings (Se ingresa desde el editor de unity).
        CargarEscenas();

    }

    //Used to play single sound clips.
    public static void PlaySingle(AudioClip clip)
    {
        /** // Asignas un clip al Audio Source de efectos.
        efxSound.clip = clip;

        // Reemplaza y reproduce el sonido de fondo.
        efxSound.Play(); **/

        // Reproducir una vez.
        //efxSound.PlayOneShot(clip);

        // Reproducir una vez con un volumen determinado de 0.0 a 1.0
        efxSound.PlayOneShot(clip, 0.0F);
        efxSound.PlayOneShot(clip, 0.45F);

    }
    
    // Asignar un audio para los efectos dependiendo de la accion
    public static void SetClip(string str) {
        // "I" para Interactuar
        if (str.Equals("I"))
        {
            PlaySingle(SoundManager.instance.InteractSound);
        }
        // "A" para Accion
        else if (str.Equals("A")) {
            PlaySingle(SoundManager.instance.ActionSound);
        }
        else if (str.Equals("W"))
        {
            PlaySingle(SoundManager.instance.WarpSound);
        }
        // "S" para guardar
        else if (str.Equals("S"))
        {
            PlaySingle(SoundManager.instance.SaveSound);
        }
        // "B" para botones del menu principal
        else if (str.Equals("M"))
        {
            PlaySingle(SoundManager.instance.MainButtonsSound);
        }
    }

    public void PlayButton() {
        PlaySingle(SoundManager.instance.MainButtonsSound);
    }

    // Asignar un audio de fondo dependiendo del escenario.
    public void SetBackground() {
        // Orden de las escenas en el array se ven desde File>Build Settings
        for ( int i = 0 ; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++ ) {
            if (SceneManager.GetActiveScene().name.Equals("Scen01") || SceneManager.GetActiveScene().name.Equals("Lobby"))
            {
                BackgroundMusic.clip = musicList[0];
            }
            else if (SceneManager.GetActiveScene().name.Equals("MenuScene"))
            {
                BackgroundMusic.clip = musicList[3];
            }
            else if (SceneManager.GetActiveScene().name.Equals("RoomMarco"))
            {
                BackgroundMusic.clip = musicList[1];
            }
            else
            {
                BackgroundMusic.clip = musicList[2];
            }
        }
    }

    // Para cuando se cambie de escena: se para la musica de fondo, asigna nulo al clip de fondo,
    // y se quita el modo de mute al fondo.
    // Automaticamente el update() reconocera que no hay musica reproduciendose y se encargara de
    // asignarle un audio y de reproducirlo.
    public static void ChangeMusic()
    {
        if (BackgroundMusic.clip != null || BackgroundMusic.isPlaying ) { 
            BackgroundMusic.Stop();
            BackgroundMusic.clip = null;
            BackgroundMusic.mute = false;
        }
    }

    // Para que se cargue una lista con todas las escenas que se encuentran agregadas en Build Settings
    // Deben agregarse ahi siempre que se crean nuevas escenas, ahi esta el orden de como se cargaran.
    public void CargarEscenas() {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        Escenas = new string[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            Escenas[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
        }
    }

}
