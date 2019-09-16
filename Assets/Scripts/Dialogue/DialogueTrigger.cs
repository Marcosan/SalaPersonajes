using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
    //Clase para llamar a DialogueManager, que ya tiene vinculado el respectivo texto del nombre y oraciones del dialogo
    public Dialogue dialogue;
    Vector2 npcPosition;
    SpriteRenderer interCollider;

    private void Start() {
        interCollider = transform.GetChild(0).GetComponent<SpriteRenderer>();
        interCollider.enabled = false;
    }

    void Update() {
        npcPosition = transform.position;
    }

    // Detectamos la colisión con una corrutina
    void OnTriggerEnter2D(Collider2D col) {
        // Si es un ataque
        if (col.tag == "Action") {
            //print("Boton de accion a " + col.gameObject.name);
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, npcPosition);
            SoundManager.SetClip("I");
        }

        if (col.tag == "Interact") {
            interCollider.enabled = true;
        }
        changer.StartDialog();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Interact") {
            interCollider.enabled = false;
            changer.DisableButton();
        }
    }
}
