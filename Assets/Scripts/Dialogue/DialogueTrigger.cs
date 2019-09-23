using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
    //Clase para llamar a DialogueManager, que ya tiene vinculado el respectivo texto del nombre y oraciones del dialogo
    public GameObject globoGo;
    Vector2 npcPosition;
    //SpriteRenderer interCollider;
    public SpriteRenderer spriteGlobo;

    public MeshRenderer dialogoMeshNombre;
    public MeshRenderer dialogoMeshDesc;
    public TextMeshPro dialogoTextNombre;
    public TextMeshPro dialogoTextDesc;

    private void Start() {
        //interCollider = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //interCollider.enabled = false;
        //dialogoTextNombre = NombreGo.GetComponent<TextMeshPro>();
        //dialogoTextDesc = DescripcionGo.GetComponent<TextMeshPro>();

        dialogoMeshNombre.sortingLayerName = "Alert";
        dialogoMeshNombre.sortingOrder = 10;
        dialogoMeshDesc.sortingLayerName = "Alert";
        dialogoMeshDesc.sortingOrder = 10;
        globoGo.SetActive(false);
    }

    void Update() {
        npcPosition = transform.position;
    }

    // Detectamos la colisión con una corrutina
    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Interact") {
            globoGo.SetActive(true);
        }
        //changer.StartDialog();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Interact") {
            globoGo.SetActive(false);
            //interCollider.enabled = false;
            changer.DisableButton();
        }
    }

    public void AddSentence(string nombre, string dialogo) {
        //dialogue.AddSentence(dialogo);
        //dialogue.AddName(nombre);
        //nombreIGN.text = nombre + "";
        dialogoTextNombre.text = nombre + "";
        dialogoTextDesc.text = dialogo + "";
    }
}
