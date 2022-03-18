using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerInSelectionArea : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    bool pointerIn;

    void Awake(){
    }

    void OnEnable(){
    }

    void OnDisable(){
    }

    public void OnPointerEnter(PointerEventData data){
        pointerIn=true;
    }  

    public void OnPointerExit(PointerEventData data){
        pointerIn=false;
    }

    public bool getPointerIn(){return pointerIn;}

}
