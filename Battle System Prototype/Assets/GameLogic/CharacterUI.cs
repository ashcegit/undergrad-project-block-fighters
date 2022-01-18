using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    GameObject player;
    GameObject playerHudObject;
    Hud playerHud;
    GameObject opponent;
    GameObject opponentHudObject;
    Hud opponentHud;

    void Awake(){
        player=transform.Find("Player").gameObject;
        playerHudObject=transform.Find("Player HUD").gameObject;
        playerHud=playerHudObject.GetComponent<Hud>();
        opponent=transform.Find("Opponent").gameObject;
        opponentHudObject=transform.Find("Opponent HUD").gameObject;
        opponentHud=opponentHudObject.GetComponent<Hud>();
    }

    void Start(){

    }

    public void updatePlayerName(string name){
        playerHud.updateName(name);
    }

    public void updateOpponentName(string name){
        opponentHud.updateName(name);
    }

    public void updatePlayerHealth(float health,float maxHealth){
        playerHud.updateHealth(health,maxHealth);
    }

    public void updateOpponentHealth(float health,float maxHealth){
        opponentHud.updateHealth(health,maxHealth);
    }

    public IEnumerator shakePlayer(){
        StartCoroutine(shakeObject(player,0.2f,1f));
        StartCoroutine(shakeObject(playerHudObject,0.1f,1f));
        yield return null;
    }

    public IEnumerator shakeOpponent(){
        StartCoroutine(shakeObject(opponent,0.2f,1f));
        StartCoroutine(shakeObject(opponentHudObject,0.1f,1f));
        yield return null;
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
