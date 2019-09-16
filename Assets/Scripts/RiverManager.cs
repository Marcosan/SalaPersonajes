using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverManager : MonoBehaviour {

    public Transform puntoInicio;
    public float speed = 4;
    public string direction;
    
    void Update(){
        if(direction.Equals("r"))
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        if (direction.Equals("d"))
            transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == "Points"){
            if (direction.Equals("r"))
                transform.position = new Vector2(puntoInicio.position.x, transform.position.y);
            if (direction.Equals("d"))
                transform.position = new Vector2(transform.position.x, puntoInicio.position.y);
        }
    }
}
