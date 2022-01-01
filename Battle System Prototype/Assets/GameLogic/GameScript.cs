using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    private Character player;
    private GameObject playerObject;
    private SpriteRenderer playerSpriteRenderer;

    private GameObject playerHUDObject;
    private SpriteRenderer playerHUDSpriteRenderer;

    private GameObject playerNameTMPObject;
    private TextMeshPro playerNameTMP;
    private float playerNameTMPFontSize=0.5f;
    private Color32 playerNameTMPColour=new Color32(0,0,0,255);

    private GameObject playerHealthTMPContainerObject;

    private GameObject playerHealthTMPObject;
    private TextMeshPro playerHealthTMP;
    private float playerHealthTMPFontSize=0.5f;
    private Color32 playerHealthTMPColour=new Color32(0,0,0,255);

    private GameObject playerMaxHealthTMPObject;
    private TextMeshPro playerMaxHealthTMP;
    private float playerMaxHealthTMPFontSize=0.5f;
    private Color32 playerMaxHealthTMPColour=new Color32(0,0,0,255);

    private GameObject playerMethod;
    private bool playerLoaded;
    private bool playerChosen;
    private bool playerWon;

    private Character opponent;
    private GameObject opponentObject;
    private ComputerPlayer computerPlayer;

    private SpriteRenderer opponentSpriteRenderer;
    private GameObject opponentHUDObject;
    private SpriteRenderer opponentHUDSpriteRenderer;

    private GameObject opponentNameTMPObject;
    private TextMeshPro opponentNameTMP;
    private float opponentNameTMPFontSize=0.5f;
    private Color32 opponentNameTMPColour=new Color32(0,0,0,255);

    private GameObject opponentHealthTMPContainerObject;

    private GameObject opponentHealthTMPObject;
    private TextMeshPro opponentHealthTMP;
    private float opponentHealthTMPFontSize=0.5f;
    private Color32 opponentHealthTMPColour=new Color32(0,0,0,255);

    private GameObject opponentMaxHealthTMPObject;
    private TextMeshPro opponentMaxHealthTMP;
    private float opponentMaxHealthTMPFontSize=0.5f;
    private Color32 opponentMaxHealthTMPColour=new Color32(0,0,0,255);

    private bool opponentLoaded;
    private bool opponentChosen;
    private bool opponentWon;

    private Vector2 resolution;
    private InteractionHandler interactionHandler;

    public void initGameScript(){
        enabled=false;
        playerObject=new GameObject();
        playerObject.transform.SetParent(GameObject.FindGameObjectWithTag("GameController").transform);
        playerObject.name="Player";
        playerObject.tag="Player";
        playerSpriteRenderer=playerObject.AddComponent<SpriteRenderer>();
        playerSpriteRenderer.sprite=Resources.Load<Sprite>("GameSprites/stickman");

        player=new Character("Player1",
                          100f,
                          15f,
                          45f,
                          60f,
                          5);
        playerLoaded=true;
        
        playerHUDObject=new GameObject();
        playerHUDObject.transform.SetParent(GameObject.FindGameObjectWithTag("GameController").transform);
        playerHUDObject.name="Player HUD";
        playerHUDSpriteRenderer=playerHUDObject.AddComponent<SpriteRenderer>();
        playerHUDSpriteRenderer.sprite=Resources.Load<Sprite>("GameSprites/HUDbox");
        

        playerNameTMPObject=new GameObject();
        playerNameTMPObject.name="Player Name TMP Object";
        playerNameTMPObject.transform.SetParent(playerHUDObject.transform);
        playerNameTMP=playerNameTMPObject.AddComponent<TextMeshPro>();
        playerNameTMP.alignment=TextAlignmentOptions.Center;

        playerHealthTMPContainerObject=new GameObject();
        playerHealthTMPContainerObject.name="Player Health TMP Object Container";
        playerHealthTMPContainerObject.transform.SetParent(playerHUDObject.transform);

        playerHealthTMPObject=new GameObject();
        playerHealthTMPObject.name="Player Health TMP Object";
        playerHealthTMPObject.transform.SetParent(playerHealthTMPContainerObject.transform);
        playerHealthTMP=playerHealthTMPObject.AddComponent<TextMeshPro>();
        playerHealthTMP.alignment=TextAlignmentOptions.Center;

        playerMaxHealthTMPObject=new GameObject();
        playerMaxHealthTMPObject.name="Player Max Health TMP Object";
        playerMaxHealthTMPObject.transform.SetParent(playerHealthTMPContainerObject.transform);
        playerMaxHealthTMP=playerMaxHealthTMPObject.AddComponent<TextMeshPro>();
        playerMaxHealthTMP.alignment=TextAlignmentOptions.Center;       

        opponentObject=new GameObject();
        opponentObject.transform.parent=GameObject.FindGameObjectWithTag("GameController").transform;
        opponentObject.name="Opponent";
        opponentSpriteRenderer=opponentObject.AddComponent<SpriteRenderer>();
        opponentSpriteRenderer.sprite=Resources.Load<Sprite>("GameSprites/stickman");

        opponent=new Character("Opponent1",
                              100f,
                              15f,
                              45f,
                              60f,
                              5);
        opponentLoaded=true;
        computerPlayer=new ComputerPlayer();

        opponentHUDObject=new GameObject();
        opponentHUDObject.transform.parent=GameObject.FindGameObjectWithTag("GameController").transform;
        opponentHUDObject.name="Opponent HUD";

        opponentHUDSpriteRenderer=opponentHUDObject.AddComponent<SpriteRenderer>();
        opponentHUDSpriteRenderer.sprite=Resources.Load<Sprite>("GameSprites/HUDbox");

        opponentNameTMPObject=new GameObject();
        opponentNameTMPObject.name="Opponent Name TMP Object";
        opponentNameTMPObject.transform.SetParent(opponentHUDObject.transform);
        opponentNameTMP=opponentNameTMPObject.AddComponent<TextMeshPro>();
        opponentNameTMP.alignment=TextAlignmentOptions.Center;

        opponentHealthTMPContainerObject=new GameObject();
        opponentHealthTMPContainerObject.name="Opponent Health TMP Object Container";
        opponentHealthTMPContainerObject.transform.SetParent(opponentHUDObject.transform);

        opponentHealthTMPObject=new GameObject();
        opponentHealthTMPObject.name="Opponent Health TMP Object";
        opponentHealthTMPObject.transform.SetParent(opponentHealthTMPContainerObject.transform);
        opponentHealthTMP=opponentHealthTMPObject.AddComponent<TextMeshPro>();
        opponentHealthTMP.alignment=TextAlignmentOptions.Center;

        opponentMaxHealthTMPObject=new GameObject();
        opponentMaxHealthTMPObject.name="Opponent Max Health TMP Object";
        opponentMaxHealthTMPObject.transform.SetParent(opponentHealthTMPContainerObject.transform);
        opponentMaxHealthTMP=opponentMaxHealthTMPObject.AddComponent<TextMeshPro>();
        opponentMaxHealthTMP.alignment=TextAlignmentOptions.Center;
        
        interactionHandler=new InteractionHandler();

        positionSprites();
        updateHUD();

        resolution=new Vector2(Screen.width,Screen.height);

        enabled=true;

        StopAllCoroutines();
    }

    void positionSprites(){
        Vector3 playerPosition=new Vector3((Screen.width-Screen.width/10)/2,Screen.height/6);
        playerObject.transform.position=Camera.main.ScreenToWorldPoint(playerPosition);
        playerObject.transform.position+=transform.forward;
        float playerScale=8f;
        Vector3 playerScaleVector=new Vector3(playerScale,playerScale);
        playerObject.transform.localScale=playerScaleVector;

        Vector3 playerHUDPosition=new Vector3(Screen.width/4,Screen.height/6);
        playerHUDObject.transform.position=Camera.main.ScreenToWorldPoint(playerHUDPosition);
        playerHUDObject.transform.position+=transform.forward;
        float playerHUDScale=10f;
        Vector3 playerHUDScaleVector=new Vector3(playerHUDScale,playerHUDScale);
        playerHUDObject.transform.localScale=playerHUDScaleVector;

        playerNameTMPObject.transform.localPosition=new Vector3(-playerHUDSpriteRenderer.size.x/4,
                                                                 playerHUDSpriteRenderer.size.y/4);

        playerHealthTMPContainerObject.transform.localPosition=new Vector3(playerHUDSpriteRenderer.size.x/4,
                                                                           -playerHUDSpriteRenderer.size.y/4);
        playerHealthTMPObject.transform.localPosition=Vector3.left*playerHUDSpriteRenderer.size.x/4;
        playerMaxHealthTMPObject.transform.localPosition=Vector3.right*playerHUDSpriteRenderer.size.x/10;

        Vector3 opponentPosition=new Vector3(Screen.width/10,Screen.height-Screen.height/5);
        opponentObject.transform.position=Camera.main.ScreenToWorldPoint(opponentPosition);
        opponentObject.transform.position+=transform.forward;
        float opponentScale=8f;
        Vector3 opponentScaleVector=new Vector3(opponentScale,opponentScale);
        opponentObject.transform.localScale=opponentScaleVector;

        Vector3 opponentHUDPosition=new Vector3((Screen.width-Screen.width/4)/2,Screen.height-Screen.height/5);
        opponentHUDObject.transform.position=Camera.main.ScreenToWorldPoint(opponentHUDPosition);
        opponentHUDObject.transform.position+=transform.forward;
        float opponentHUDScale=10f;
        Vector3 opponentHUDScaleVector=new Vector3(opponentHUDScale,opponentHUDScale);
        opponentHUDObject.transform.localScale=opponentHUDScaleVector;

        opponentNameTMPObject.transform.localPosition=new Vector3(-opponentHUDSpriteRenderer.size.x/4,
                                                                 opponentHUDSpriteRenderer.size.y/4);

        opponentHealthTMPContainerObject.transform.localPosition=new Vector3(opponentHUDSpriteRenderer.size.x/4,
                                                                           -opponentHUDSpriteRenderer.size.y/4);
        opponentHealthTMPObject.transform.localPosition=Vector3.left*opponentHUDSpriteRenderer.size.x/4;
        opponentMaxHealthTMPObject.transform.localPosition=Vector3.right*opponentHUDSpriteRenderer.size.x/10;
    }

    void updateHUD(){
        playerNameTMP.text=player.getCharacterName();
        playerNameTMP.fontSize=playerNameTMPFontSize;
        playerNameTMP.color=playerNameTMPColour;
        playerHealthTMP.text=player.getHealth().ToString();
        playerHealthTMP.fontSize=playerHealthTMPFontSize;
        playerHealthTMP.color=playerHealthTMPColour;
        playerMaxHealthTMP.text=player.getMaxHealth().ToString();
        playerMaxHealthTMP.fontSize=playerMaxHealthTMPFontSize;
        playerMaxHealthTMP.color=playerMaxHealthTMPColour;

        opponentNameTMP.text=opponent.getCharacterName();
        opponentNameTMP.fontSize=opponentNameTMPFontSize;
        opponentNameTMP.color=opponentNameTMPColour;
        opponentHealthTMP.text=opponent.getHealth().ToString();
        opponentHealthTMP.fontSize=opponentHealthTMPFontSize;
        opponentHealthTMP.color=opponentHealthTMPColour;
        opponentMaxHealthTMP.text=opponent.getMaxHealth().ToString();
        opponentMaxHealthTMP.fontSize=opponentMaxHealthTMPFontSize;
        opponentMaxHealthTMP.color=opponentMaxHealthTMPColour;
    }

    void OnGUI(){
        if(resolution.x!=Screen.width||resolution.y!=Screen.height){
            resolution.x=Screen.width;
            resolution.y=Screen.height;
            positionSprites();
            updateHUD();
        }
    }

    void OnEnable(){
        SpriteRenderer[] spriteRenderers=(SpriteRenderer[])GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer spriteRenderer in spriteRenderers){
            spriteRenderer.enabled=true;
        }
    }

    void OnDisable(){
        SpriteRenderer[] spriteRenderers=(SpriteRenderer[])GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer spriteRenderer in spriteRenderers){
            spriteRenderer.enabled=false;
        }
    }

    public Character getPlayer(){return player;}
    public Character getOpponent(){return opponent;}

    public void setPlayerLoaded(bool playerLoaded){this.playerLoaded=playerLoaded;}
    public bool getPlayerLoaded(){return playerLoaded;}
    public void setOpponentLoaded(bool opponentLoaded){this.opponentLoaded=opponentLoaded;}
    public bool getOpponentLoaded(){return opponentLoaded;}
    public bool getCharactersLoaded(){return playerLoaded&&opponentLoaded;}

    public bool isGameOver(){return hasPlayerLost()||hasOpponentLost();}
    public bool hasPlayerLost(){return player.getHealth()==0;}
    public bool hasOpponentLost(){return opponent.getHealth()==0;}

    public ComputerPlayer getComputerPlayer(){
        return computerPlayer;
    }

    public void clearComputerBlockStack(){
        computerPlayer.clearBlockStack();
        opponentChosen=false;
    }

    public void setPlayerMethod(GameObject playerMethod){
        this.playerMethod=playerMethod;
        playerChosen=true;
    }

    public GameObject getPlayerMethod(){
        return playerMethod;
    }

    public void clearPlayerBlockStack(){
        playerMethod.GetComponent<Block>().clearBlockStack();
        playerChosen=false;
    }

    public void setOpponentChosen(bool opponentChosen){
        this.opponentChosen=opponentChosen;
    }
    public void setPlayerChosen(bool playerChosen){
        this.playerChosen=playerChosen;
    }

    public bool getOpponentChosen(){return opponentChosen;}
    public bool getPlayerChosen(){return playerChosen;}

    private IEnumerator shakeObject(GameObject gameObject,
                                    float shakeTime,
                                    float shakeDistance){
        Vector3 initialPosition=gameObject.transform.position;
        float elapsedShakeTime=0f;
        while(elapsedShakeTime<shakeTime){
            elapsedShakeTime+=Time.deltaTime;
            gameObject.transform.position=initialPosition+Random.insideUnitSphere*shakeDistance;
            yield return null;
        }
        gameObject.transform.position=initialPosition;
        positionSprites();
    }

    public Interaction getInteraction(GameAction gameAction){
        return interactionHandler.getInteraction(gameAction);
    }

    public bool getPlayerFirst(GameAction playerGameAction,GameAction opponentGameAction){
        return interactionHandler.getPlayerFirst(playerGameAction,opponentGameAction,player.getSpeed(),opponent.getSpeed());
    }

    public void dealDamage(Character target,float damage){
        target.decreaseHealth(damage);
        updateHUD();    
        if(target==player){
            StartCoroutine(shakeObject(playerObject,0.2f,1f));
            StartCoroutine(shakeObject(playerHealthTMPObject,0.1f,1f));
        }else if(target==opponent){
            StartCoroutine(shakeObject(opponentObject,0.2f,1f));
            StartCoroutine(shakeObject(opponentHealthTMPObject,0.1f,1f));
        }
    }
    
    public void heal(Character target,float healingAmount){
        target.increaseHealth(healingAmount);
        updateHUD();
        if(target==player){
            //add healing animation using playerSpriteRenderer
        }else if(target==opponent){
            //add healing animation using opponentSpriteRenderer
        }
    }

    public void addModifier(Character target,AttributeModifier attributeModifier){
        target.addModifier(attributeModifier);
        updateHUD();
        if(target==player){
            //add healing animation using playerSpriteRenderer
        }else if(target==opponent){
            //add healing animation using opponentSpriteRenderer
        }
    }

    public void endTurn(){
        player.endTurn();
        opponent.endTurn();
    }

    public void resetGame(){
        playerLoaded=false;
        opponentLoaded=false;
        
        player=new Character("Player",
                          100f,
                          15f,
                          45f,
                          60f,
                          5);

        opponent=new Character("Opponent",
                              100f,
                              15f,
                              45f,
                              60f,
                              5);
        computerPlayer=new ComputerPlayer();

        opponentChosen=false;
        playerChosen=false;

        positionSprites();
        updateHUD();

        playerLoaded=true;
        opponentLoaded=true;
    }
}
