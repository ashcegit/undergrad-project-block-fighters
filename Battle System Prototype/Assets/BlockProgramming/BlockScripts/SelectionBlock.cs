using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionBlock : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject prefabBlock;
    public GameObject rolloverInfo;

    void Awake(){
    }

    void OnEnable(){
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        Debug.Log("SelectionBlock entered");
        rolloverInfo = Instantiate((GameObject)Resources.Load("Prefabs/UI/RolloverBlockInfo"));
        rolloverInfo.transform.SetParent(GetComponentInParent <BlockProgrammerScript> ().transform.Find("Canvas Rollover Tooltip"));
        rolloverInfo.GetComponent<RectTransform>().position = (Vector2)pointerEventData.position;
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
        Debug.Log("SelectionBlock exited");
        Destroy(rolloverInfo);
    }
}
