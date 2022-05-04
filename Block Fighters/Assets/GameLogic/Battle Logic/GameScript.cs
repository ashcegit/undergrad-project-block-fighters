using System;
using System.Text;
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

    private int levelCounter;

    CharacterUI characterUI;

    void Awake(){
        characterUI=GetComponentInChildren<CharacterUI>();

        computerPlayer=new ComputerPlayer();
    }

    void OnEnable(){
        characterUI.showUI();
    }

    void OnDisable(){
        characterUI.hideUI();
    }

    public void Start(){
        levelCounter=0;              

        player=new Character("Player",
                          100f,
                          20f,
                          25f,
                          20f,
                          5);
        playerLoaded=true;

        opponent=new Character("Opponent",
                              100f,
                              15f,
                              25f,
                              20f,
                              5);
        opponentLoaded=true;
        
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

    public int getLevelCounter(){return levelCounter;}

    public void updatePlayerHUDName() { characterUI.updatePlayerName(player.getCharacterName()); }

    public void initComputerPlayerBlockStack(){
        computerPlayer.initBlockStack(levelCounter);
    }

    public void clearComputerBlockStack(){
        computerPlayer.clearBlockStack();
        opponentChosen=false;
    }

    public Tuple<GameAction?,bool> executeCurrentComputerPlayerBlock(){
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
        return InteractionHandler.getInteraction(gameAction);
    }

    public bool getPlayerFirst(GameAction playerGameAction,GameAction opponentGameAction){
        return InteractionHandler.getPlayerFirst(playerGameAction,opponentGameAction,player.getSpeed(),opponent.getSpeed());
    }

    public IEnumerator dealDamage(Character target,float damage){
        Debug.Log("Deal damage started");
        target.decreaseHealth(Mathf.Round(damage));
        if(target==player){
            Debug.Log("about to shake player");
            StartCoroutine(characterUI.shakePlayer());            
            characterUI.updatePlayerHealth(target.getHealth(),target.getMaxHealth());
            StartCoroutine(characterUI.shakePlayerHealth());
        } else if(target==opponent){
            StartCoroutine(characterUI.shakeOpponent());
            characterUI.updateOpponentHealth(target.getHealth(),target.getMaxHealth());
            StartCoroutine(characterUI.shakeOpponentHealth());
        }
        yield return null;
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

    public Tuple<List<int>,List<int>> nextLevel(){
        levelCounter++;
        playerLoaded=false;
        opponentLoaded=false;

        List<int> playerStatIncreases = new List<int> { 100, 20, 20, 20 };
        List<int> opponentStatIncreases = new List<int>{ 100, 20, 20, 20 };

        HashSet<int> uniqueRandomIndices = new HashSet<int>();

        //shuffle stats then distribute stat increases from a
        //random multiplier against a fixed maximum amount

        int maxMultiplier = 100;
        System.Random random = new System.Random();
        while (uniqueRandomIndices.Count < 4) {
            int statIndex = random.Next(0,4);
            if (!uniqueRandomIndices.Contains(statIndex)) {
                uniqueRandomIndices.Add(statIndex);
                int multiplier;
                if (uniqueRandomIndices.Count == 3) {
                    multiplier = maxMultiplier;
                } else {
                    multiplier = UnityEngine.Random.Range(0, (int)((float)maxMultiplier*0.8f));
                }
                maxMultiplier -= multiplier;
                if (maxMultiplier > 0) { maxMultiplier = 0; }
                playerStatIncreases[statIndex] = (int)((float)playerStatIncreases[statIndex] * (float)multiplier/100f);
            }
        }

        uniqueRandomIndices = new HashSet<int>();
        maxMultiplier = 100;
        random = new System.Random();
        while (uniqueRandomIndices.Count < 4) {
            int statIndex = random.Next(0, 4);
            if (!uniqueRandomIndices.Contains(statIndex)) {
                uniqueRandomIndices.Add(statIndex);
                int multiplier;
                if (uniqueRandomIndices.Count == 3) {
                    multiplier = maxMultiplier;
                } else {
                    multiplier = UnityEngine.Random.Range(0, (int)((float)maxMultiplier * 0.8f));
                }
                maxMultiplier -= multiplier;
                if (maxMultiplier > 0) { maxMultiplier= 0; }
                opponentStatIncreases[statIndex] = (int)((float)opponentStatIncreases[statIndex] * (float)multiplier/100f);
            }
        }

        //Guaranteed health increase of 50 points

        playerStatIncreases[0] += 50;
        opponentStatIncreases[0] += 50;

        player =new Character(player.getCharacterName(),
                          player.getBaseMaxHealth() + playerStatIncreases[0],
                          player.getBaseAttack()+playerStatIncreases[1],
                          player.getBaseDefence()+playerStatIncreases[2],
                          player.getBaseSpeed()+playerStatIncreases[3],
                          player.getStamina()+1);

        opponent = new Character(opponent.getCharacterName(),
                          opponent.getBaseMaxHealth() + opponentStatIncreases[0],
                          opponent.getBaseAttack() + opponentStatIncreases[1],
                          opponent.getBaseDefence() + opponentStatIncreases[2],
                          opponent.getBaseSpeed() + opponentStatIncreases[3],
                          opponent.getStamina() + 1);

        opponentChosen=false;
        playerChosen=false;

        characterUI.updatePlayerHealth(player.getHealth(),player.getMaxHealth());
        characterUI.updateOpponentHealth(opponent.getHealth(),opponent.getMaxHealth());

        playerLoaded=true;
        opponentLoaded=true;

        return new Tuple<List<int>, List<int>>(  playerStatIncreases, opponentStatIncreases ) ;
    }

    public void resetGame() {
        levelCounter = 0;

        player = new Character("Player",
                          100f,
                          20f,
                          25f,
                          20f,
                          5);

        opponent = new Character("Opponent",
                              100f,
                              15f,
                              25f,
                              20f,
                              5);

        opponentChosen = false;
        playerChosen = false;

        characterUI.updatePlayerHealth(player.getHealth(), player.getMaxHealth());
        characterUI.updateOpponentHealth(opponent.getHealth(), opponent.getMaxHealth());

        playerLoaded = true;
        opponentLoaded = true;
    }
}
