using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    private Camera thisCamera;

    private float CameraSize = 0;
    // Start is called before the first frame update
    void Start()
    {
        thisCamera = this.GetComponent<Camera>();
        CameraSize = thisCamera.orthographicSize;

        float x = 750.0f / 1334.0f;

        float hight = Screen.height;
        float width  = Screen.width;
        float tmp = width / hight;
        float newSize = CameraSize * x / tmp;

        newSize = newSize < 6.7f ? 6.7f : newSize;
        thisCamera.orthographicSize = newSize;
    }
    
}
