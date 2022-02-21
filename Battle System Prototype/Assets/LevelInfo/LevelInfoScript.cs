using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelInfoScript : MonoBehaviour
{
    GameObject levelUpGameObject;
    HealthBar  levelUpBar;
    GameObject levelsToGoGameObject;
    NewBlockInfoScript newBlockInfoScript;

    void Awake(){
        levelUpGameObject=transform.Find("Level Up Text").gameObject;
        levelUpBar=GetComponentInChildren<HealthBar>();
        levelsToGoGameObject=transform.Find("Remaining Level Text").gameObject;
        newBlockInfoScript = transform.parent.Find("New Block Info").GetComponent<NewBlockInfoScript>();
    }

    void OnEnable(){
        gameObject.SetActive(true);
    }

    void OnDisable(){
        gameObject.SetActive(false);
    }

    public void setNewBlocks(List<SelectionBlock> newSelectionBlocks) {
        newBlockInfoScript.setNewBlocks(newSelectionBlocks);
        newBlockInfoScript.displayBlocks();
    }

    public void levelUp(int levelCounter,int maxLevels){
        levelsToGoGameObject.GetComponent<TextMeshProUGUI>().text=(maxLevels-levelCounter).ToString()+" levels to go";
        levelUpBar.updateHealthBar((float)((float)levelCounter/(float)maxLevels),1f);
    }

    public void unlockBlocks(){
        newBlockInfoScript.enabled = true;
        gameObject.SetActive(false);
    }
}
