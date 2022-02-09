using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    private Character player;
    private GameObject playerMethod;
    private bool playerLoaded;
    private bool playerChosen;
    private bool playerWon;

    private Character opponent;
    private ComputerPlayer computerPlayer;
    private bool opponentLoaded;
    private bool opponentChosen;
    private bool opponentWon;
    private InteractionHandler interactionHandler;

    private int levelCounter;

    CharacterUI characterUI;

    void Awake(){
        characterUI=GetComponentInChildren<CharacterUI>();

        computerPlayer=new ComputerPlayer();
        
        interactionHandler=new InteractionHandler();
    }

    void OnEnable(){
        characterUI.showUI();
    }

    void OnDisable(){
        characterUI.hideUI();
    }

    public void Start(){
        enabled=false;

        levelCounter=0;              

        player=new Character("Player",
                          100f,
                          15f,
                          45f,
                          60f,
                          5);
        playerLoaded=true;

        opponent=new Character("Opponent",
                              100f,
                              15f,
                              45f,
                              60f,
                              5);
        opponentLoaded=true;
        
        enabled=true;

        StopAllCoroutines();
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

    public void initComputerPlayerBlockStack(){
        computerPlayer.initBlockStack(levelCounter);
    }

    public void clearComputerBlockStack(){
        computerPlayer.clearBlockStack();
        opponentChosen=false;
    }

    public ExecutionWrapper executeCurrentComputerPlayerBlock(){
        return computerPlayer.executeCurrentBlock();
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

    

    public Interaction getInteraction(GameAction gameAction){
        return interactionHandler.getInteraction(gameAction);
    }

    public bool getPlayerFirst(GameAction playerGameAction,GameAction opponentGameAction){
        return interactionHandler.getPlayerFirst(playerGameAction,opponentGameAction,player.getSpeed(),opponent.getSpeed());
    }

    public void dealDamage(Character target,float damage){
        target.decreaseHealth(Mathf.Round(damage));
        if(target==player){
            StartCoroutine(characterUI.shakePlayer());
            characterUI.updatePlayerHealth(target.getHealth(),target.getMaxHealth());
        }else if(target==opponent){
            StartCoroutine(characterUI.shakeOpponent());
            characterUI.updateOpponentHealth(target.getHealth(),target.getMaxHealth());
        }
    }
    
    public void heal(Character target,float healingAmount){
        target.increaseHealth(Mathf.Round(healingAmount));
        if(target==player){
            characterUI.updatePlayerHealth(target.getHealth(),target.getMaxHealth());
        }else if(target==opponent){
            characterUI.updateOpponentHealth(target.getHealth(),target.getMaxHealth());
        }
    }

    public void addModifier(Character target,AttributeModifier attributeModifier){
        target.addModifier(attributeModifier);
        if(target==player){
            characterUI.updatePlayerHealth(target.getHealth(),target.getMaxHealth());
        }else if(target==opponent){
            characterUI.updateOpponentHealth(target.getHealth(),target.getMaxHealth());
        }
    }

    public void endTurn(){
        player.endTurn();
        opponent.endTurn();
    }

    public void updatePlayerName(string name){
        characterUI.updatePlayerName(player.getCharacterName());
    }

    public void updateOpponentName(string name){
        characterUI.updateOpponentName(opponent.getCharacterName());
    }

    public void updatePlayerHealth(){
        characterUI.updatePlayerHealth(player.getHealth(),player.getMaxHealth());
    }

    public void updateOpponentHealth(){
        characterUI.updateOpponentHealth(opponent.getHealth(),opponent.getMaxHealth());
    }

    public void nextLevel(){
        levelCounter++;
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

        characterUI.updatePlayerHealth(player.getHealth(),player.getMaxHealth());
        characterUI.updateOpponentHealth(opponent.getHealth(),opponent.getMaxHealth());

        playerLoaded=true;
        opponentLoaded=true;
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

        characterUI.updatePlayerHealth(player.getHealth(),player.getMaxHealth());
        characterUI.updateOpponentHealth(opponent.getHealth(),opponent.getMaxHealth());

        playerLoaded=true;
        opponentLoaded=true;
    }
}
