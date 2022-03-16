using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RolloverInfo : MonoBehaviour
{
    TextMeshProUGUI rolloverText;
    RectTransform rectTransform;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        rolloverText = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    void OnGUI() {
        rectTransform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Vector3[] fourCornerArray = { new Vector3(),new Vector3(),new Vector3(),new Vector3() };
        rectTransform.GetWorldCorners(fourCornerArray);
        Debug.Log(fourCornerArray);
        Vector2 minCorner = fourCornerArray[0];
        Vector2 maxCorner = fourCornerArray[3];

        if (maxCorner.x > Screen.width) {
            rectTransform.position = new Vector2(rectTransform.position.x
                                    - (maxCorner.x-Screen.width),
                                    rectTransform.position.y);
        }

        if (maxCorner.y < 0) {
            rectTransform.position = new Vector2(rectTransform.position.x,rectTransform.position.y
                                    - maxCorner.y  );
        }
    }

    public void setRolloverText(string rolloverText) {
        this.rolloverText.text = rolloverText;
    }
}
