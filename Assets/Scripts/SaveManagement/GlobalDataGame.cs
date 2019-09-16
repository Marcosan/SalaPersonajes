using UnityEngine;

// Siempre agregar esta linea para que el sistema sepa que es un script que se serializa
[System.Serializable]
public class GlobalDataGame 
{

    // Datos relacionados a la escena
    string LastScene;

    // Datos relacionados con los niveles


    /** Poner de argumentos las clases que estados de niveles o cualquier tipo de parametros de los que se quiere guardar y cargar
     * al cerrar e iniciar partida nuevamente.
     * */
    public GlobalDataGame(Player pl) {
        
        // Guarda la ultima escena en la que estaba al momento de guardar
        LastScene = pl.GetLastScene();
    }

    public string GetLastScene()
    {
        return LastScene;
    }

}
