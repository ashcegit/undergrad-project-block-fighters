using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RolloverInfo : MonoBehaviour
{
    TextMeshProUGUI rolloverText;

    void Awake() {
        rolloverText = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    void OnGUI() {
        GetComponent<RectTransform>().position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void setRolloverText(string rolloverText) {
        this.rolloverText.text = rolloverText;
    }
}
