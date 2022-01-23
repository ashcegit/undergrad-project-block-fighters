using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image innerHealthBar;

    void Awake(){
        innerHealthBar=GetComponentInChildren<Image>();
    }

    public void updateHealthBar(float healthPercentage){
        innerHealthBar.fillAmount=healthPercentage;
    }
}
