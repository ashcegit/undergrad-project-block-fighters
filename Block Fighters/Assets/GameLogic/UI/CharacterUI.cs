using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    GameObject playerImage;
    GameObject playerShadow;
    GameObject playerHudObject;
    Hud playerHud;
    GameObject opponentImage;
    GameObject opponentShadow;
    GameObject opponentHudObject;
    Hud opponentHud;

    void Awake(){
        playerImage=transform.Find("Player").gameObject.transform.Find("Player Image").gameObject;
        playerShadow = transform.Find("Player").gameObject.transform.Find("Player Shadow").gameObject;
        playerHudObject =transform.Find("Player HUD").gameObject;
        playerHud=playerHudObject.GetComponent<Hud>();
        
        opponentImage=transform.Find("Opponent").gameObject.transform.Find("Opponent Image").gameObject;
        opponentShadow = transform.Find("Opponent").gameObject.transform.Find("Opponent Shadow").gameObject;
        opponentHudObject = transform.Find("Opponent HUD").gameObject;
        opponentHud=opponentHudObject.GetComponent<Hud>();
    }

    void Start(){

    }

    public void hideUI(){
        gameObject.SetActive(false);
    }

    public void showUI(){
        gameObject.SetActive(true);
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
        yield return StartCoroutine(shakeCharacter(playerImage,playerShadow,0.5f,5f));
    }

    public IEnumerator shakePlayerHealth(){
        yield return StartCoroutine(playerHud.shakeHealth());
    }

    public IEnumerator shakePlayerMaxHealth(){
        yield return StartCoroutine(playerHud.shakeMaxHealth());
    }

    public IEnumerator shakeOpponent(){
        yield return StartCoroutine(shakeCharacter(opponentImage, opponentShadow, 0.5f, 5f));
    }

    public IEnumerator shakeOpponentHealth(){
        yield return StartCoroutine(opponentHud.shakeHealth());
    }

    public IEnumerator shakeOpponentMaxHealth(){
        yield return StartCoroutine(opponentHud.shakeMaxHealth());
    }

    private IEnumerator shakeCharacter(GameObject image,
                                    GameObject shadow,
                                    float shakeTime,
                                    float shakeDistance) {
        Vector3 initialImagePosition = image.GetComponent<RectTransform>().position;
        Vector3 initialShadowPosition = shadow.GetComponent<RectTransform>().position;
        float elapsedShakeTime = 0f;
        while (elapsedShakeTime < shakeTime) {
            elapsedShakeTime += Time.deltaTime;
            image.GetComponent<RectTransform>().position = initialImagePosition + Random.insideUnitSphere * shakeDistance;
            shadow.GetComponent<RectTransform>().position = initialShadowPosition + Random.insideUnitSphere * shakeDistance;
            yield return null;
        }
        image.GetComponent<RectTransform>().position = initialImagePosition;
        shadow.GetComponent<RectTransform>().position = initialShadowPosition;
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
        yield return null;
    }

}
