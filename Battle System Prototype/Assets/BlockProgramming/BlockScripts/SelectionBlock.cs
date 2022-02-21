using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionBlock : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject prefabBlock;
    private GameObject rolloverInfo;
    public string description;

    void Awake(){

    }

    void OnEnable(){

    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        rolloverInfo = Instantiate((GameObject)Resources.Load("Prefabs/UI/RolloverBlockInfo"));
        rolloverInfo.transform.SetParent(GameObject.FindGameObjectWithTag("TooltipCanvas").transform);
        rolloverInfo.GetComponent<RectTransform>().position = (Vector2)pointerEventData.position;
        rolloverInfo.GetComponent<RolloverInfo>().setRolloverText(description);
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
        Destroy(rolloverInfo);
    }
}
