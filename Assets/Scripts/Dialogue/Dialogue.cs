using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue{

    public string name;

    [TextArea(3, 10)]
    public List <string> sentences = new List<string>();

    public void AddSentence(string dialogo) {
        sentences.Add(dialogo);
    }

    public void AddName(string nombre) {
        name = nombre;
    }
}
