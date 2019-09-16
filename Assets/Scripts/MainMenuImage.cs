using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuImage : MonoBehaviour
{

    public Sprite[] images;
    
    void Start()
    {
        int indice = (Random.Range(0, images.Length)) / 1;
        print(indice);
        this.transform.GetComponent<UnityEngine.UI.Image>().sprite = images[indice];
    }
    
}
