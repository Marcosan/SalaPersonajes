using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class JsonManager : MonoBehaviour
{
    /* Este script siempre se va a cargar al iniciar el juego ya que esta vinculado a MainCamera, siempre se puede contar con el
     * archivo Ajustes.json y su instancia para almacenar lo necesario para el juego. 
     * Este juego tiene plantillas para guardar ya sea por archivo binario o json, o ejemplos para guardar utilizando PlayerPrefs
     * Dependiendo del gusto, comodidad, o cual es mejor para el trabajo se pueden utilizar cualquiera de estos ejemplos.
     * Aunque el uso de json y PlayerPrefs son parecidos por el uso de llaves y valores, la ventaja de json es que no se limita
     * solo a valores de tipo string, int o float. Tambien soporta guardar arreglos o booleanos, etc.
     */

    // Variables para el mapa de Kokoa y Fanpol
    private static MapKokoaClass newMapDarkClear;

    // Para el uso con JSON
    static string filePath;
    static string jsonString;

    // Instanciamos un objeto de clase a la que vamos a utilizar
    public static GlobalSettings gsettings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Para leer el archivo mientras carga el juego
    private void Awake()
    {
        newMapDarkClear = new MapKokoaClass();
        filePath = SaveSystem.MainPath + SaveSystem.NameJson;

        if (!File.Exists(filePath))
        {
            gsettings = new GlobalSettings("New Player", "LOBBY", "LOBBY", "Lobby", true, newMapDarkClear);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write("{" + gsettings.ToString() + "}");
                }
            }
        }

        jsonString = File.ReadAllText(filePath);

        // Se instancia el objeto con los datos del archivo
        gsettings = JsonUtility.FromJson<GlobalSettings>(jsonString);

        // Para imprimir y verificar los valores que estan guardados hasta este momento
        //print(gsettings);

        /* Para cambiar valores del json lo que hay que hacer es cambiar los valores del objeto que se instancia
         * y luego mandarlo al archivo mediante el JsonUtility, por ejemplo:
         */
        //gsettings.namePlayer = "Marco";

        // De esta forma se los guarda
        //SerializeSettings();

        /* Si se hace al objeto y sus variables estaticas se las puede editar desde cualquier parte del programa
         * Aunque el ejemplo se encuentra en awake, no hay problema con utilizarlo desde cualquier script
         */

    }

    /* Metodo para serializar los cambios en las configuraciones, no es necesario uno de deserializacion ya que siempre se 
     * los deserializa al iniciar el juego.
     */
    public static void SerializeSettings() {
        jsonString = JsonUtility.ToJson(gsettings);
        print(jsonString);
        File.WriteAllText(filePath, jsonString);
    }

    public static void setNamePlayer(string str) {
        gsettings.namePlayer = str;
        print("El nuevo nombre del jugador es: " + str);
    }

    public static void setLastMap(string str)
    {
        gsettings.lastMapName = str;
        print("El mapa actual es: " + str);
    }

    public static void setLastInitialMap(string str) {
        gsettings.lastInitialMap = str;
    }

    public static void SetLastScene(string strScene) {
        gsettings.lastScene = strScene;
    }
    
    // Sets para el mapa de Kokoa
    public static void setMapDarkK(bool clear) {
        gsettings.mapsDarkClear.mapaK = clear;
        checkMapaDarkIsClear();
    }
    
    public static void setMapDarkO(bool clear) {
        gsettings.mapsDarkClear.mapaO = clear;
    }
    
    public static void setMapDarkA(bool clear) {
        gsettings.mapsDarkClear.mapaA = clear;
    }

    private static void checkMapaDarkIsClear() {
        if (isKokoaLevelClear()) {
            gsettings.mapsDarkClear.MapaKokoaCompleto = true;
        }
    }

    private static bool isKokoaLevelClear(){
        if (!gsettings.mapsDarkClear.mapaK)
            return false;
        else if (!gsettings.mapsDarkClear.mapaO)
            return false;
        else if (!gsettings.mapsDarkClear.mapaA)
            return false;

        return true;
    }

    public static void SetInitialDataJson() {
        newMapDarkClear = new MapKokoaClass();
        gsettings.namePlayer = "New Player";
        gsettings.lastInitialMap = "LOBBY";
        gsettings.lastMapName = "LOBBY";
        gsettings.lastScene = "Lobby";
        gsettings.newGame = true;
        gsettings.mapsDarkClear = newMapDarkClear;

        SaveSystem.newGame = true;

    }

}

[System.Serializable]
public class GlobalSettings
{
    // Aqui van los datos declarados exactamente igual a como estan sus keys en el archivo json
    public string namePlayer;
    public string lastMapName;
    public string lastInitialMap;
    public string lastScene;
    public bool newGame;
    public MapKokoaClass mapsDarkClear;

    public GlobalSettings(string name, string linitMap, string lMap, string lScene, bool nGame, //Datos Generales
                            MapKokoaClass mDarkClear) //Datos de Room de Marco
    {
        namePlayer = name;
        lastInitialMap = linitMap;
        lastMapName = lMap;
        lastScene = lScene;
        newGame = nGame;
        mapsDarkClear = mDarkClear;
    }

    public override string ToString()
    {
        return string.Format(" \"namePlayer\" : {0} , \"lastMapName\" : {1} , \"lastInitialMap\" : {2} , \"lastScene\" : {3} , \"newGame\" : {4} , \"mapsDarkClear\" : {5}", namePlayer, lastMapName, lastInitialMap, lastScene, newGame, mapsDarkClear);
    }

}

[System.Serializable]
public class MapKokoaClass
{
    // Aqui van los datos declarados exactamente igual a como estan sus keys en el archivo json
    public bool mapaK;
    public bool mapaO;
    public bool mapaA;
    public bool MapaKokoaCompleto;

    public MapKokoaClass(){
        mapaK = false;
        mapaO = false;
        mapaA = false;
        MapaKokoaCompleto = false;
    }

    public override string ToString(){
        return string.Format(" \"namePlayer\" : {0} , \"lastMapTag\" : {1} ", mapaK, MapaKokoaCompleto);
    }

}