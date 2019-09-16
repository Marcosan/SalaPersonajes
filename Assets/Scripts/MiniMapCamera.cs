using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour {

    public Transform originalMap, targetMap, mainCamera, playerMiniMap;
    public Joystick joystick;
    Vector2 distanceMap, mov;
    float angle;

    void Awake() {
        //miniMapCamera = GameObject.FindGameObjectWithTag("MiniMapSprite").transform;
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        distanceMap = targetMap.position - originalMap.position;
    }

    private void Start() {
        //Screen.SetResolution(800, 800, true);

    }

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(mainCamera.position.x + distanceMap.x, mainCamera.position.y + distanceMap.y, mainCamera.position.z);
        playerMiniMap.position = new Vector3(transform.position.x, transform.position.y, playerMiniMap.position.z);

        MoveMentsJoyStick();
        RotationPlayerMiniMap();
    }

    void MoveMentsJoyStick() {
        if((joystick.Horizontal > .2f || joystick.Horizontal < -.2f) ||
            (joystick.Vertical > .2f || joystick.Vertical< -.2f))
            mov = new Vector2(joystick.Horizontal, joystick.Vertical);
        else
            mov = Vector2.zero;
    }
    void RotationPlayerMiniMap() {
        angle = Mathf.Atan2(mov.y, mov.x) * Mathf.Rad2Deg;

        playerMiniMap.transform.rotation = Quaternion.Euler(0, 0, angle);
        //playerMiniMap.Rotate(0, 0, angle);
        
    }

}
