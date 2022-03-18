using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image innerHealthBar;

    void Awake(){
        innerHealthBar=transform.GetChild(0).GetComponent<Image>();
    }

    public void updateHealthBar(float healthPercentage,float speed){
        StopAllCoroutines();
        StartCoroutine(updateHealthBarCoroutine(healthPercentage,speed));
    }

    IEnumerator updateHealthBarCoroutine(float healthPercentage,float speed){
        float time=0f;
        float initialAmount=innerHealthBar.fillAmount;
        while(innerHealthBar.fillAmount!=healthPercentage){
            innerHealthBar.fillAmount=Mathf.Lerp(initialAmount,healthPercentage,time);
            time+=Time.deltaTime*speed;
            yield return null;
        }
        yield return null;
    }
}
