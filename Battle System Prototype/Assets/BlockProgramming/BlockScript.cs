using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable(){
        Canvas[] canvases=(Canvas[])GetComponentsInChildren<Canvas>();
        foreach(Canvas canvas in canvases){canvas.enabled=true;}
    }

    void OnDisable(){
        Canvas[] canvases=(Canvas[])GetComponentsInChildren<Canvas>();
        foreach(Canvas canvas in canvases){canvas.enabled=false;}
    }
}
