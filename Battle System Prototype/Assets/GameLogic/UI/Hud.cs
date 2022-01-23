using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hud : MonoBehaviour
{
    TextMeshProUGUI nameText;
    HealthBar healthBar;
    TextMeshProUGUI healthText;
    TextMeshProUGUI slashText;
    TextMeshProUGUI maxHealthText;

    void Awake(){
        nameText=transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
        healthBar=GetComponentInChildren<HealthBar>();
        healthText=transform.Find("Health").gameObject.GetComponent<TextMeshProUGUI>();
        slashText=transform.Find("Health Slash").gameObject.GetComponent<TextMeshProUGUI>();
        maxHealthText=transform.Find("Max Health").gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void updateName(string name){
        nameText.text=name;
    }

    public void updateHealth(float health,float maxHealth){
        healthText.text=Mathf.Round(health).ToString();
        maxHealthText.text=Mathf.Round(maxHealth).ToString();
        healthBar.updateHealthBar(health/maxHealth);
    }

    public IEnumerator shakeHealth(){
        yield return StartCoroutine(shakeObject(healthText.gameObject,0.2f,5f));
    }

    public IEnumerator shakeMaxHealth(){
        yield return StartCoroutine(shakeObject(maxHealthText.gameObject,0.2f,5f));
    }

    private IEnumerator shakeObject(GameObject gameObject,
                                    float shakeTime,
                                    float shakeDistance){
        Vector3 initialPosition=gameObject.GetComponent<RectTransform>().position;
        float elapsedShakeTime=0f;
        while(elapsedShakeTime<shakeTime){
            elapsedShakeTime+=Time.deltaTime;
            gameObject.GetComponent<RectTransform>().position=initialPosition+Random.insideUnitSphere*shakeDistance;
            yield return null;
        }
        gameObject.GetComponent<RectTransform>().position=initialPosition;
    }
}
