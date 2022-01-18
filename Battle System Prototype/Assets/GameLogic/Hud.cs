using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hud : MonoBehaviour
{
    TextMeshProUGUI nameText;
    TextMeshProUGUI healthText;
    TextMeshProUGUI slashText;
    TextMeshProUGUI maxHealthText;

    void Awake(){
        nameText=transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
        healthText=transform.Find("Health").gameObject.GetComponent<TextMeshProUGUI>();
        slashText=transform.Find("Health Slash").gameObject.GetComponent<TextMeshProUGUI>();
        maxHealthText=transform.Find("Max Health").gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void updateName(string name){
        nameText.text=name;
    }

    public void updateHealth(float health,float maxHealth){
        healthText.text=health.ToString();
        maxHealthText.text=maxHealth.ToString();
    }
}
