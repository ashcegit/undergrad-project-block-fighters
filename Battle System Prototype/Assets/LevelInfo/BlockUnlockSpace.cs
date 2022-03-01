using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockUnlockSpace : MonoBehaviour
{

    TextMeshProUGUI newBlockText;

    void Awake() {
        newBlockText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void displayBlock(SelectionBlock selectionBlock) {
        newBlockText.text=selectionBlock.gameObject.name;
        Vector2 lastPosition = GetComponentInChildren<SelectionBlock>().gameObject.GetComponent<RectTransform>().position;
        Destroy(GetComponentInChildren<SelectionBlock>().gameObject);
        GameObject newBlock = Instantiate(selectionBlock.gameObject);
        newBlock.transform.SetParent(transform);
        RectTransform newRectTransform = newBlock.GetComponent<RectTransform>();
        newRectTransform.anchorMin = new Vector2(1, 0);
        newRectTransform.anchorMax = new Vector2(0, 1);
        newRectTransform.pivot = new Vector2(0.5f, 0.5f);
        newRectTransform.position = lastPosition;
    }
}
