using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockUnlockSpace : MonoBehaviour
{

    TextMeshProUGUI newBlockText;
    Vector2 lastPosition;

    void Awake() {
        newBlockText = GetComponentInChildren<TextMeshProUGUI>();
        lastPosition = GetComponentInChildren<SelectionBlock>().gameObject.GetComponent<RectTransform>().position;
        Destroy(GetComponentInChildren<SelectionBlock>().gameObject);
    }

    public void displayBlock(SelectionBlock selectionBlock) {
        newBlockText.text=selectionBlock.gameObject.name;
        GameObject newBlock = Instantiate(selectionBlock.gameObject);
        newBlock.transform.SetParent(transform);
        RectTransform newRectTransform = newBlock.GetComponent<RectTransform>();
        newRectTransform.anchorMin = new Vector2(1, 0);
        newRectTransform.anchorMax = new Vector2(0, 1);
        newRectTransform.pivot = new Vector2(0.5f, 0.5f);
        newRectTransform.position = lastPosition;
    }

    public void clearInfo() {
        newBlockText.text = "";
        lastPosition = GetComponentInChildren<SelectionBlock>().gameObject.GetComponent<RectTransform>().position;
        Destroy(GetComponentInChildren<SelectionBlock>().gameObject);
    }
}
