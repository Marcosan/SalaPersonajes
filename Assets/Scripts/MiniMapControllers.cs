using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapControllers : MonoBehaviour{

    string objectName;
    Camera miniMapCamera;
    float zoomRange = 1f;
    float minOrtho = 3.0f;
    float maxOrtho = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        miniMapCamera= GameObject.Find("/Cameras/MiniMapCamera").GetComponent<Camera>();
        //zoomIn = GameObject.Find("/Area/MiniMap/ZoomIn").transform;
        //zoomOut = GameObject.Find("/Area/MiniMap/ZoomOut").transform;
    }

    public void ZoomInOut() {
        objectName = transform.name;
        if (objectName.Equals("ZoomIn")) {
            miniMapCamera.orthographicSize -= zoomRange;
        }
        if (objectName.Equals("ZoomOut")) {
            miniMapCamera.orthographicSize += zoomRange;
        }
        miniMapCamera.orthographicSize = Mathf.Clamp(miniMapCamera.orthographicSize, minOrtho, maxOrtho);
        
    }

}
