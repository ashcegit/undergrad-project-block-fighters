using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaminaText : MonoBehaviour
{
    TextMeshProUGUI textMesh;

    void Awake() {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void changeText(string text) { textMesh.text = "Stamina: "+text; }
}
