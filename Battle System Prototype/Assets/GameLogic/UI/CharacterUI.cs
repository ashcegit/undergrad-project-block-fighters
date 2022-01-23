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

    public void hideUI(){
        player.SetActive(false);
        playerHudObject.SetActive(false);
        opponent.SetActive(false);
        opponentHudObject.SetActive(false);
    }

    public void showUI(){
        player.SetActive(true);
        playerHudObject.SetActive(true);
        opponent.SetActive(true);
        opponentHudObject.SetActive(true);
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
        yield return StartCoroutine(shakeObject(player,0.2f,5f));
    }

    public IEnumerator shakePlayerHealth(){
        yield return StartCoroutine(playerHud.shakeHealth());
    }

    public IEnumerator shakePlayerMaxHealth(){
        yield return StartCoroutine(playerHud.shakeMaxHealth());
    }

    public IEnumerator shakeOpponent(){
        yield return StartCoroutine(shakeObject(opponent,0.2f,5f));
    }

    public IEnumerator shakeOpponentHealth(){
        yield return StartCoroutine(opponentHud.shakeHealth());
    }

    public IEnumerator shakeOpponentMaxHealth(){
        yield return StartCoroutine(opponentHud.shakeMaxHealth());
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
