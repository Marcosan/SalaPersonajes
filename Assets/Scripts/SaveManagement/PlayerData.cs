using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // Datos relacionados principalmente con el estado jugador

    float[] LastMov;
    float[] Posicion;

    public PlayerData(Player player) {
        
        // Guarda la uktima posicion en la que se encontraba
        Posicion = new float[3];
        Posicion[0] = player.transform.position.x;
        Posicion[1] = player.transform.position.y;
        Posicion[2] = player.transform.position.z;

        // Guarda el ultimo vector de direccion del personaje (Hacia donde estaba mirando)
        LastMov = new float[2];
        LastMov[0] = player.getLastMov().x;
        LastMov[1] = player.getLastMov().y;

    }

    public float GetX() {
        return Posicion[0];
    }

    public float GetY()
    {
        return Posicion[1];
    }

    public float GetZ()
    {
        return Posicion[2];
    }

    public Vector2 GetMovement() {
        return new Vector2(LastMov[0],LastMov[1]);
    }
    
}
