using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    private Player player;
    private GameObject playerObject;
    private SpriteRenderer playerSpriteRenderer;
    private GameObject playerHUDObject;
    private SpriteRenderer playerHUDSpriteRenderer;
    //private Text playerHUDText;
    private GameAction playerGameAction;
    private bool playerLoaded;
    private bool playerChosen;
    private bool playerWon;

    private Opponent opponent;
    private GameObject opponentObject;
    private SpriteRenderer opponentSpriteRenderer;
    private GameObject opponentHUDObject;
    private SpriteRenderer opponentHUDSpriteRenderer;
    //private Text opponentHUDText;
    private GameAction opponentGameAction;
    private bool opponentLoaded;
    private bool opponentChosen;
    private bool opponentWon;

    private Vector2 resolution;

    public void initGameScript(){
        enabled=false;
        playerObject=new GameObject();
        playerObject.transform.parent=GameObject.FindGameObjectWithTag("GameController").transform;
        playerObject.name="Player";
        playerObject.tag="Player";
        playerSpriteRenderer=playerObject.AddComponent<SpriteRenderer>();
        playerSpriteRenderer.sprite=Resources.Load<Sprite>("GameSprites/stickman");
        
        playerHUDObject=new GameObject();
        playerHUDObject.transform.parent=GameObject.FindGameObjectWithTag("GameController").transform;
        playerHUDObject.name="Player HUD";
        playerHUDSpriteRenderer=playerHUDObject.AddComponent<SpriteRenderer>();
        playerHUDSpriteRenderer.sprite=Resources.Load<Sprite>("GameSprites/HUDbox");
        //playerHUDText=playerHUDObject.AddComponent<Text>();

        player=new Player("Player",
                          100f,
                          15f,
                          45f,
                          60f);
        playerLoaded=true;

        opponentObject=new GameObject();
        opponentObject.transform.parent=GameObject.FindGameObjectWithTag("GameController").transform;
        opponentObject.name="Opponent";
        opponentSpriteRenderer=opponentObject.AddComponent<SpriteRenderer>();
        opponentSpriteRenderer.sprite=Resources.Load<Sprite>("GameSprites/stickman");

        opponentHUDObject=new GameObject();
        opponentHUDObject.transform.parent=GameObject.FindGameObjectWithTag("GameController").transform;
        opponentHUDObject.name="Opponent HUD";
        opponentHUDSpriteRenderer=opponentHUDObject.AddComponent<SpriteRenderer>();
        opponentHUDSpriteRenderer.sprite=Resources.Load<Sprite>("GameSprites/HUDbox");
        //playerHUDText=playerHUDObject.AddComponent<Text>();

        opponent=new Opponent("Opponent",
                              100f,
                              15f,
                              45f,
                              60f);
        opponentLoaded=true;

        positionSprites();

        resolution=new Vector2(Screen.width,Screen.height);

        enabled=true;

        StopAllCoroutines();
    }

    void positionSprites(){
        Vector3 playerPosition=new Vector3((Screen.width-Screen.width/10)/2,Screen.height/6);
        playerSpriteRenderer.transform.position=Camera.main.ScreenToWorldPoint(playerPosition);
        playerSpriteRenderer.transform.position+=transform.forward*1;
        Vector3 playerScale=new Vector3(8f,8f);
        playerSpriteRenderer.transform.localScale=playerScale;

        Vector3 playerHUDPosition=new Vector3(Screen.width/4,Screen.height/6);
        playerHUDSpriteRenderer.transform.position=Camera.main.ScreenToWorldPoint(playerHUDPosition);
        playerHUDSpriteRenderer.transform.position+=transform.forward*1;
        Vector3 playerHUDScale=new Vector3(10f,10f);
        playerHUDSpriteRenderer.transform.localScale=playerHUDScale;

        Vector3 opponentPosition=new Vector3(Screen.width/10,Screen.height-Screen.height/5);
        opponentSpriteRenderer.transform.position=Camera.main.ScreenToWorldPoint(opponentPosition);
        opponentSpriteRenderer.transform.position+=transform.forward*1;
        Vector3 opponentScale=new Vector3(8f,8f);
        opponentSpriteRenderer.transform.localScale=opponentScale;

        Vector3 opponentHUDPosition=new Vector3((Screen.width-Screen.width/4)/2,Screen.height-Screen.height/5);
        opponentHUDSpriteRenderer.transform.position=Camera.main.ScreenToWorldPoint(opponentHUDPosition);
        opponentHUDSpriteRenderer.transform.position+=transform.forward*1;
        Vector3 opponentHUDScale=new Vector3(10f,10f);
        opponentHUDSpriteRenderer.transform.localScale=opponentHUDScale;
    }

    void OnGUI(){
        if(resolution.x!=Screen.width||resolution.y!=Screen.height){
            resolution.x=Screen.width;
            resolution.y=Screen.height;
            positionSprites();
        }
    }

    public Player getPlayer(){return player;}
    public Opponent getOpponent(){return opponent;}

    public void setPlayerLoaded(bool playerLoaded){this.playerLoaded=playerLoaded;}
    public bool getPlayerLoaded(){return playerLoaded;}
    public void setOpponentLoaded(bool opponentLoaded){this.opponentLoaded=opponentLoaded;}
    public bool getOpponentLoaded(){return opponentLoaded;}
    public bool getCharactersLoaded(){return playerLoaded&&opponentLoaded;}

    public bool isGameOver(){return hasPlayerLost()||hasOpponentLost();}
    public bool hasPlayerLost(){return player.getHealth()<=0;}
    public bool hasOpponentLost(){return opponent.getHealth()<=0;}

    public void initOpponentGameAction(){
        MethodInfo[] opponentMethods=opponent.GetType().GetMethods(BindingFlags.DeclaredOnly|
                                                                   BindingFlags.Public|
                                                                   BindingFlags.Instance);
        MethodInfo opponentMethod=opponentMethods[UnityEngine.Random.Range(0,opponentMethods.Length)];
        Character target=player;
        opponentGameAction=(GameAction)opponentMethod.Invoke((Opponent)opponent,new object[]{player});
        setOpponentChosen(true);
    }

    public GameAction getOpponentGameAction(){
        initOpponentGameAction();
        return opponentGameAction;
    }

    public void setPlayerGameAction(GameAction playerGameAction){
        this.playerGameAction=playerGameAction;
        playerChosen=true;
    }

    public GameAction getPlayerGameAction(){
        return playerGameAction;
    }

    public void setOpponentChosen(bool opponentChosen){
        this.opponentChosen=opponentChosen;
    }
    public void setPlayerChosen(bool playerChosen){
        this.playerChosen=playerChosen;
    }

    public bool getOpponentChosen(){return opponentChosen;}
    public bool getPlayerChosen(){return playerChosen;}

    public MethodInfo[] getPlayerMethods(){
        return player.GetType().GetMethods();
    }

    private IEnumerator shakeSprite(SpriteRenderer spriteRenderer,
                                    float shakeTime,
                                    float shakeDistance){
        Vector3 initialPosition=spriteRenderer.transform.position;
        float elapsedShakeTime=0f;
        while(elapsedShakeTime<shakeTime){
            elapsedShakeTime+=Time.deltaTime;
            spriteRenderer.transform.position=initialPosition+Random.insideUnitSphere*shakeDistance;
            yield return null;
        }
        spriteRenderer.transform.position=initialPosition;
    }

    public void dealDamage(Character target,float damage){
        StopAllCoroutines();
        target.decreaseHealth(damage);
        if(target==player){
            StartCoroutine(shakeSprite(playerSpriteRenderer,0.2f,1f));
        }else if(target==opponent){
            StartCoroutine(shakeSprite(opponentSpriteRenderer,0.2f,1f));
        }
    }
    
    public void heal(Character target,float healingAmount){
        target.increaseHealth(healingAmount);
        if(target==player){
            //add healing animation using playerSpriteRenderer
        }else if(target==opponent){
            //add healing animation using opponentSpriteRenderer
        }
    }

    public void addModifier(Character target,AttributeModifier attributeModifier){
        target.addModifier(attributeModifier);
        if(target==player){
            //add healing animation using playerSpriteRenderer
        }else if(target==opponent){
            //add healing animation using opponentSpriteRenderer
        }
    }
}
