using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelInfoScript : MonoBehaviour
{
    GameObject levelUpGameObject;
    HealthBar  levelUpBar;
    GameObject levelsToGoGameObject;

    void Awake(){
        levelUpGameObject=transform.Find("Level Up Text").gameObject;
        levelUpBar=GetComponentInChildren<HealthBar>();
        levelsToGoGameObject=transform.Find("Remaining Level Text").gameObject;
    }

    void OnEnable(){
        gameObject.SetActive(true);
    }

    void OnDisable(){
        gameObject.SetActive(false);
    }

    public void levelUp(int levelCounter,int maxLevels){
        levelsToGoGameObject.GetComponent<TextMeshProUGUI>().text=(maxLevels-levelCounter).ToString()+" levels to go";
        levelUpBar.updateHealthBar((float)((float)levelCounter/(float)maxLevels),1f);
    }

    public void nextLevel(){
        GameObject.FindGameObjectWithTag("Main").GetComponent<Main>().nextLevel();
    }
}
