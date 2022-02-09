using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfoScript : MonoBehaviour
{
    GameObject levelUpGameObject;

    void Awake(){
        levelUpGameObject=GameObject.FindGameObjectWithTag("LevelUp");
    }

    void OnEnable(){
        gameObject.SetActive(true);
    }

    void OnDisable(){
        gameObject.SetActive(false);
    }
}
